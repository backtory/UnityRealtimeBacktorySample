using Assets.Backtory.core;
using Assets.BacktorySDK.core;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BacktoryUser
{
    private static BacktoryUser currentUser;
    private const string KeyGuestPassword = "guest password";
    private const string KeyLoginInfo = "login info";
    private const string KeyAuthorization = "Authorization";
    private const string KeyCurrentUser = "current user";
    private const string KeyUsername = "username key";


    #region private storing methods
    // Important note: 

    private static void SaveLoginInfo(LoginResponse loginResponse)
    {
        Backtory.Storage.Put(KeyLoginInfo, Backtory.ToJson(loginResponse));
    }
    private static void DispatchSaveLoginInfo(LoginResponse loginResponse)
    {
        Backtory.Dispatch(() => { SaveLoginInfo(loginResponse); });
    }

    private static void SaveGuestPassword(string guestPassword)
    {
        Backtory.Storage.Put(KeyGuestPassword, guestPassword);
    }
    private static void DispatchSaveGuestPassword(string guestNewPassword)
    {
        Backtory.Dispatch(() => { SaveGuestPassword(guestNewPassword); });
    }

    private static void SaveAsCurrentUserInMemoryAndStorage(BacktoryUser user)
    {
        currentUser = user;
        Backtory.Storage.Put(KeyUsername, user.Username);
        Backtory.Storage.Put(KeyCurrentUser, Backtory.ToJson(user));
    }
    private static void DispatchSaveCurrentUser(BacktoryUser backtoryUser)
    {
        Backtory.Dispatch(() => { SaveAsCurrentUserInMemoryAndStorage(backtoryUser); });
    }

    #endregion

    #region guest register + login
    private static RestRequest GuestRegisterRequest()
    {
//		var request = Backtory.RestRequest("guest-users", Method.POST);
		var request = Backtory.RestRequest("auth/guest-users", Method.POST);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        return request;
    }

    public static void LoginAsGuestInBackground(Action<BacktoryResponse<LoginResponse>> callback)
    {
        // Simplifying this by removing "BacktoryUser" type parameter leads to error though compiler suggests, but why?!
        Backtory.RestClient.ExecuteAsync<BacktoryUser>(GuestRegisterRequest(), response =>
        {
            // **this part will be called on background thread! (Since we're using restsharp async method)**

            // if guest register failed don't proceed to login 
            if (!response.IsSuccessful())
            {
                Backtory.Dispatch(() => BacktoryResponse<LoginResponse>.Unknown(response.ErrorMessage));
                return;
            }

            var guest = response.Data;
            var loginResponse = Backtory.Execute<LoginResponse>(LoginRequest(guest.Username, guest.Password));

            if (loginResponse.Successful)
            {
                DispatchSaveCurrentUser(guest);
                DispatchSaveGuestPassword(guest.Password);
                DispatchSaveLoginInfo(loginResponse.Body);
            }

            Backtory.Dispatch(() => callback(loginResponse));
        });
    }

    

    public static BacktoryResponse<LoginResponse> LoginAsGuest()
    {
        var regResponse = Backtory.Execute<BacktoryUser>(GuestRegisterRequest());
        if (!regResponse.Successful)
        {
            return BacktoryResponse.Error<BacktoryUser, LoginResponse>(regResponse);
        }
        var guest = regResponse.Body;
        var loginResponse = Backtory.Execute<LoginResponse>(LoginRequest(guest.Username, guest.Password));

        if (loginResponse.Successful)
        {
            DispatchSaveCurrentUser(guest);
            DispatchSaveGuestPassword(guest.Password);
            DispatchSaveLoginInfo(loginResponse.Body);
        }

        return loginResponse;
    }
    #endregion

    #region register
    private static RestRequest RegisterRequest(BacktoryUser registrationParams)
    {
//		var request = Backtory.RestRequest("users", Method.POST);
		var request = Backtory.RestRequest("auth/users", Method.POST);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddJsonBody(registrationParams);
        return request;
    }



    public void RegisterInBackground(Action<BacktoryResponse<BacktoryUser>> callback)
    {
        Backtory.ExecuteAsync(RegisterRequest(this), callback);
    }
    #endregion

    #region login
    // restsharp normally doesn't support multipart request without file
    // by setting AlwaysMultipart we force it to do so
    private static RestRequest LoginRequest(string username, string password)
    {
//		var loginRequest = Backtory.RestRequest("login", Method.POST);
		var loginRequest = Backtory.RestRequest("auth/login", Method.POST);
        loginRequest.AlwaysMultipartFormData = true;
        loginRequest.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        loginRequest.AddHeader("X-Backtory-Authentication-Key", BacktoryConfig.BacktoryAuthClientKey);
        loginRequest.AddParameter("username", username, ParameterType.GetOrPost);
        loginRequest.AddParameter("password", password, ParameterType.GetOrPost);

        return loginRequest;
    }

    

    public static BacktoryResponse<LoginResponse> Login(string username, string password)
    {
        var loginResponse = Backtory.Execute<LoginResponse>(LoginRequest(username, password));
        if (loginResponse.Successful)
        {
            var userResponse = Backtory.Execute<BacktoryUser>(UserByUsernameRequest(username, loginResponse.Body.AccessToken));
            if (userResponse.Successful)
            {
                DispatchSaveCurrentUser(userResponse.Body);
                DispatchSaveLoginInfo(loginResponse.Body);
            }
            else
            {
                BacktoryResponse<LoginResponse>.Unknown(userResponse.Message);
                Debug.Log("error getting user info by username\n" + userResponse.Message);
            }
        }
        return loginResponse;
    }


    public static void LoginInBackground(string username, string password,
        Action<BacktoryResponse<LoginResponse>> callback)
    {
        Backtory.ExecuteAsync<LoginResponse>(LoginRequest(username, password), loginResopnse =>
        {
            // this will be called in main thread since we're using backtory API
            if (loginResopnse.Successful)
            {
                Backtory.ExecuteAsync<BacktoryUser>(UserByUsernameRequest(username, loginResopnse.Body.AccessToken), userResponse =>
                {
                    // also on main thread
                    if (userResponse.Successful)
                    {
                        //DispatchSaveCurrentUser(userResponse.Body);
                        //DispatchSaveLoginInfo(loginResopnse.Body);
                        SaveAsCurrentUserInMemoryAndStorage(userResponse.Body);
                        SaveLoginInfo(loginResopnse.Body);
                        callback(loginResopnse);
                    }
                    else
                        callback(BacktoryResponse<LoginResponse>.Unknown(userResponse.Message));

                });
            }
            else
                callback(loginResopnse);
        });
    }
    #endregion

    #region current user
    public static BacktoryUser GetCurrentUser()
    {
        // from memory
        if (currentUser != null)
        {
            return currentUser;
        }
        // from storage (mostly disk)
        string userJson = Backtory.Storage.Get(KeyCurrentUser);
        if (!userJson.IsEmpty())
        {
            return Backtory.FromJson<BacktoryUser>(userJson);
        }
        // indicating a login is required because a user info must be exist in all conditions if user
        // access token is present in storage
        return null;
    }
    #endregion

    #region user by username
    internal static RestRequest UserByUsernameRequest(string username, string accessToken = null)
    {
//		var request = Backtory.RestRequest("users/by-username/{username}", Method.GET);
		var request = Backtory.RestRequest("auth/users/by-username/{username}", Method.GET);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddHeader(KeyAuthorization, accessToken != null ? "Bearer " + accessToken : AuthorizationHeader());
        request.AddParameter("username", username, ParameterType.UrlSegment);

        return request;
    }

    internal static BacktoryResponse<BacktoryUser> GetUserByUsername(string username)
    {
        return Backtory.Execute<BacktoryUser>(UserByUsernameRequest(username));
    }
    #endregion

    #region complete guest register
    private RestRequest CompleteRegRequest(GuestCompletionParam guestRegistrationParam)
    {
//		var request = Backtory.RestRequest("guest-users/complete-registration", Method.POST);
		var request = Backtory.RestRequest("auth/guest-users/complete-registration", Method.POST);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddHeader(KeyAuthorization, AuthorizationHeader());
        request.AddJsonBody(guestRegistrationParam);
        return request;
    }

    public void CompleteRegistrationInBackgrond(GuestCompletionParam guestRegistrationParam, Action<BacktoryResponse<BacktoryUser>> callback)
    {
        Backtory.ExecuteAsync<BacktoryUser>(CompleteRegRequest(guestRegistrationParam), completeRegResponse =>
        {
            if (completeRegResponse.Successful)
                SaveAsCurrentUserInMemoryAndStorage(completeRegResponse.Body);
            callback(completeRegResponse);
        });
    }

    public BacktoryResponse<BacktoryUser> CompleteRegistration(GuestCompletionParam guestRegistrationParam)
    {
        var completeRegResponse = Backtory.Execute<BacktoryUser>(CompleteRegRequest(guestRegistrationParam));
        if (completeRegResponse.Successful)
            DispatchSaveCurrentUser(completeRegResponse.Body);
        return completeRegResponse;
    }
    #endregion

    #region change password
    private RestRequest ChangePassRequest(string oldPassword, string newPassword)
    {
//		var request = Backtory.RestRequest("change-password", Method.POST);
		var request = Backtory.RestRequest("auth/change-password", Method.POST);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddHeader(KeyAuthorization, AuthorizationHeader());
        //sdfljasjglkjksladg // Why content type in response is null despite line below???
        request.OnBeforeDeserialization = response => {
            response.ContentType = "text/plain";
        };
        request.AddJsonBody(new Dictionary<string, string>()
        {
            // it's common to refer as "oldPassword" not "lastPassword". But what can I do? :)
            { "lastPassword", oldPassword }, { "newPassword", newPassword }
        });

        return request;
    }

    public void ChangePasswordInBackground(string oldPassword, string newPassword, Action<BacktoryResponse<object>> callback)
    {
        if (Guest)
            throw new InvalidOperationException("guest user con not change it's password");
        Backtory.ExecuteAsync(ChangePassRequest(oldPassword, newPassword), callback);
    }

    public BacktoryResponse<object> ChangePassword(string oldPassword, string newPassword)
    {
        if (Guest)
            throw new InvalidOperationException("guest user con not change it's password");
        return Backtory.Execute<object>(ChangePassRequest(oldPassword, newPassword));
    }
    #endregion

    #region update user
    private RestRequest UpdateUserRequest(BacktoryUser toBeUpdateUser) {
//		var request = Backtory.RestRequest("users/{user_id}", Method.PUT);
		var request = Backtory.RestRequest("auth/users/{user_id}", Method.PUT);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddHeader(KeyAuthorization, AuthorizationHeader());
        request.AddParameter("user_id", toBeUpdateUser.UserId, ParameterType.UrlSegment);
        request.AddJsonBody(toBeUpdateUser);
        return request;
    }

    public void UpdateUserInBackground(Action<BacktoryResponse<BacktoryUser>> callback) {
        Backtory.ExecuteAsync<BacktoryUser>(UpdateUserRequest(this), updateResponse => {
            if (updateResponse.Successful)
            {
                SaveAsCurrentUserInMemoryAndStorage(updateResponse.Body);
            }
            callback(updateResponse);
        });
    }

    public BacktoryResponse<BacktoryUser> UpdateUser()
    {
        var updateResponse = Backtory.Execute<BacktoryUser>(UpdateUserRequest(this));
        if (updateResponse.Successful)
            DispatchSaveCurrentUser(updateResponse.Body);
        return updateResponse;
    }
    #endregion

    #region logout
    private static RestRequest LogoutRequest(string refreshToken)
    {
//		var request = Backtory.RestRequest("logout", Method.DELETE);
		var request = Backtory.RestRequest("auth/logout", Method.DELETE);
        request.AddHeader(Backtory.AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
        request.AddParameter("refresh-token", refreshToken, ParameterType.QueryString);
        request.OnBeforeDeserialization = response => { response.ContentType = "text/plain"; };
        return request;
    }

    /// <summary>
    /// We must clear everything first, because logout is independent from server and expiration of refresh-token
    /// but if doing that, we can't get refresh token from storage because it's already cleared.
    /// </summary>
    private static string ClearStorageAndReturnRefreshToken()
    {
        var refreshToken = GetRefreshToken();
        ClearBacktoryStoredData();
        return refreshToken;
    }

    public static void LogoutInBackground()
    {
        Backtory.ExecuteAsync<object>(LogoutRequest(ClearStorageAndReturnRefreshToken()), null);
    }

    public static void Logout()
    {
        Backtory.Execute<object>(LogoutRequest(ClearStorageAndReturnRefreshToken()));
    }

    internal static void ClearBacktoryStoredData()
    {
        currentUser = null;
        Backtory.Storage.Clear();
    }
    #endregion

    #region properties

    [SerializeAs(Name = "userId")]
    [DeserializeAs(Name = "userId")]
    public string UserId { get; internal set; }

    [SerializeAs(Name = "username")]
    [DeserializeAs(Name = "username")]
    public string Username { get; set; }

    [SerializeAs(Name = "password")]
    [DeserializeAs(Name = "password")]
    public string Password { get; set; }

    [SerializeAs(Name = "firstName")]
    [DeserializeAs(Name = "firstName")]
    public string FirstName { get; set; }

    [SerializeAs(Name = "lastName")]
    [DeserializeAs(Name = "lastName")]
    public string LastName { get; set; }

    [SerializeAs(Name = "email")]
    [DeserializeAs(Name = "email")]
    public string Email { get; set; }

    [SerializeAs(Name = "phoneNumber")]
    [DeserializeAs(Name = "phoneNumber")]
    public string PhoneNumber { get; set; }

    [SerializeAs(Name = "guest")]
    [DeserializeAs(Name = "guest")]
    public bool Guest { get; internal set; }

    [SerializeAs(Name = "active")]
    [DeserializeAs(Name = "active")]
    public bool Active { get; internal set; }

    #endregion

    #region Builder
    public class Builder
    {
        private string firstName;
        private string lastName;
        private string username;
        private string password;
        private string email;
        private string phoneNumber;

        public Builder SetFirstName(string firstName)
        {
            this.firstName = firstName;
            return this;
        }

        public Builder SetLastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        public Builder SetUsername(string username)
        {
            this.username = username;
            return this;
        }

        public Builder SetPassword(string password)
        {
            this.password = password;
            return this;
        }

        public Builder SetEmail(string email)
        {
            this.email = email;
            return this;
        }

        public Builder SetPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
            return this;
        }

        public BacktoryUser build()
        {
            return new BacktoryUser(
                firstName, lastName,
                Utils.checkNotNull(username, "user name can not be null"),
                password, email, phoneNumber);
        }
    }
    #endregion

    #region constructors
    public BacktoryUser() { }
    public BacktoryUser(string firstName, string lastName, string username,
                      string password, string email, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    #endregion

    #region login response POCO
    public class LoginResponse
    {
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; internal set; }

        [DeserializeAs(Name = "type_token")]
        public string TypeToken { get; internal set; }

        [DeserializeAs(Name = "refresh_token")]
        public string RefreshToken { get; internal set; }

        [DeserializeAs(Name = "expires_in")]
        public string ExpiresIn { get; internal set; }

        [DeserializeAs(Name = "scope")]
        public string scope { get; internal set; }

        [DeserializeAs(Name = "jti")]
        public string jti { get; internal set; }
    }
    #endregion

    #region guest completion parameters POCO
    public class GuestCompletionParam
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastPassword { get; set; }
        public string NewUsername { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
    #endregion

    #region move to BacktoryAuth class
    internal static string AuthorizationHeader()
    {
        string accessToke = GetAccessToken();
        return accessToke != null ? "Bearer " + GetAccessToken() : null;
    }

    internal static string GetAccessToken()
    {
        try
        {
            return GetLoginResponse().AccessToken;
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    internal static string GetRefreshToken()
    {
        try
        {
            return GetLoginResponse().RefreshToken;
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    internal static LoginResponse GetLoginResponse()
    {
        // TODO: store in ram to prevent every time deserialization
        return GetStoredLoginResponse();
    }

    private static LoginResponse GetStoredLoginResponse()
    {
        string loginResponseString = Backtory.Storage.Get(KeyLoginInfo);
        /*if (loginResponseString == null)
          throw new IllegalStateException("no auth token exists");*/
        return Backtory.FromJson<LoginResponse>(loginResponseString);
    }

    internal static string getGuestPassword()
    {
        return Backtory.Storage.Get(KeyGuestPassword);
    }
    #endregion
}