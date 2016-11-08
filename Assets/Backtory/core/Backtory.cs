using Assets.BacktorySDK.core;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Backtory.core
{
    public class Backtory
    {
		public const string BacktoryBaseAddress = "https://api.backtory.com/"; 
//		public const string BacktoryBaseAddress = "http://192.168.99.100:8045/";
        internal const string ContentTypeString = "Content-Type";
        internal const string ApplicationJson = "application/json";
        internal const string AuthInstanceIdString = "X-Backtory-Authentication-Id";
        internal const string AuthClientKeyString = "X-Backtory-Authentication-Key";
        


        internal static readonly RestClient RestClient = new RestClient(BacktoryBaseAddress);
        //internal static readonly RestClient RestClient;
        public static IStorage Storage
        {
            internal get; set;
        }
        //public static Newtonsoft.Json.JsonSerializer JsonDonNetInstance { get; private set; } = new 

        static Backtory()
        {
            Storage = new PlayerPrefsStorage();
            
            //using NewtonSoft Json.Net
            //RestClient = new RestClient(BacktoryBaseAddress);
            //var JsonDotNetHandler = new NewtonsoftJsonSerializer();
            //RestClient.AddHandler("application/json", JsonDotNetHandler);
            //RestClient.AddHandler("text/json", JsonDotNetHandler);
            //RestClient.AddHandler("text/x-json", JsonDotNetHandler);
            //RestClient.AddHandler("text/javascript", JsonDotNetHandler);
            //RestClient.AddHandler("*+json", JsonDotNetHandler);
        }
        // this is absolutely terrible! some facts
        // * SimpleJson doesn't care about any serialize attribute (annotation in Java) so serialization faces with problem
        // * SimpleJson is an internal class in restsharp assembly so I doesn't have access and cat's set serializationStrategy
        // * Json.Net port for unity is big in size (450 kB) and have issues in versioning
        // * This one makes me miss Retrofit so much! I can't set global serializer so I must to attach my own serializer to every *** request 
        internal static RestRequest RestRequest(string segmentUrl, Method method)
        {
            return new RestRequest(segmentUrl, method)
            {
                JsonSerializer = new NewtonsoftJsonSerializer() /*MyJsonSerializer();*/
            };
        }



        ///// <summary>
        ///// Handles the HTTP response based on <see cref="P:Restsharp.IRestResponse.ErrorException"/>
        ///// and dispatches the result (wrapped in a BacktoryReponse) on the Unity main thread.
        ///// </summary>
        ///// <returns>action able to convert rest sharp responses to backtory responses and dispatching them</returns>
        //internal static Action<IRestResponse<T>> ResponseDispatcher<T>(Action<) where T : class
        //{
        //    return (response) =>
        //    {
        //        var result = response.ErrorException != null ?
        //            BacktoryResponse<T>.error((int)response.StatusCode, response.ErrorException) :
        //            BacktoryResponse<T>.success((int)response.StatusCode, response.Data);
        //        Debug.Log("Receiving login response");

        //        Dispatcher.Instance.Invoke(() => action(result));
        //    };
        //}

        internal static BacktoryResponse<T> Execute<T>(RestRequest request) where T : class, new()
        {
            var response = RestClient.Execute<T>(request);
            BacktoryResponse<T> result = RawResponseToBacktoryResponse(response);
            return result;
        }

        internal static void ExecuteAsync<T>(RestRequest request, Action<BacktoryResponse<T>> callback) where T : class, new()
        {
            RestClient.ExecuteAsync<T>(request, response =>
            {
                // will be executed in background thread
                BacktoryResponse<T> result = RawResponseToBacktoryResponse(response);

                // avoiding NullReferenceException on requests with null callback like logout
                if (callback != null)
                    BacktoryManager.Instance.Invoke(() => callback(result));
            });
        }

        private static BacktoryResponse<T> RawResponseToBacktoryResponse<T>(IRestResponse<T> response) where T : class, new()
        {
            BacktoryResponse<T> result;
            if (response.ErrorException != null || !response.IsSuccessful())
            {
                if ((int)response.StatusCode == (int)BacktoryHttpStatusCode.Unauthorized)
                    response = Handle401StatusCode(response);
                result = BacktoryResponse<T>.Error((int)response.StatusCode, response.ErrorMessage);
            }
            else
                result = BacktoryResponse<T>.Success((int)response.StatusCode, response.Data);
            Debug.Log("Receiving response of: " + typeof(T).Name + " with code: " + result.Code + "\ncause: " + response.ErrorMessage);
            Debug.Log(response.ResponseUri);
            return result;
        }

        /// <summary>
        /// A 401 error mostly indicates access-token is expired. (Only exception is login which 401 shows incorrect username/password)
        /// We must refresh the access-token using refresh-token and retry the original request with new access-token
        /// If on refreshing access-token we get another 401, it indicates refresh-token is expired, too
        /// On that case, if current user is guest we must login with stored username-pass and if not we must force the user to login 
        /// </summary>
        /// <typeparam name="T">type of response body</typeparam>
        /// <param name="response">raw response containing error 401</param>
        /// <returns></returns>
        private static IRestResponse<T> Handle401StatusCode<T>(IRestResponse<T> response) where T : class, new()
        {
            // in response of login request (no access token yet!) return the original response
            if (response.Request.Resource.Contains("login"))
                return response;
            // getting new access-token
            var tokenResponse = RestClient.Execute<BacktoryUser.LoginResponse>(NewAccessTokenRequest());

            if (tokenResponse.ErrorException != null || !response.IsSuccessful())
            {
                // failed to get new token
                if ((int)tokenResponse.StatusCode == (int)BacktoryHttpStatusCode.Unauthorized)
                {
                    // refresh token itself is expired
                    if (BacktoryUser.GetCurrentUser().Guest)
                    {
                        // if guest, first login with stored username/pass and the retry the request
                        // new token is stored and after this we can simply call original request again which uses new token 
                        BacktoryUser.Login(BacktoryUser.GetCurrentUser().Username, BacktoryUser.getGuestPassword());
                    }

                    // normal user must login again
                    // TODO: clean way for forcing user to login. How to keep his/her progress? How to retry original request?
                    else
                    {
                        BacktoryUser.ClearBacktoryStoredData();

                        // On this case return value is not important
                        // TODO: may be changing the response error message
                        BacktoryManager.Instance.GlobalEventListener.OnEvent(BacktorySDKEvent.LogoutEvent());
                    }
                }
                
                // successfully gotten new token
            }
            return RestClient.Execute<T>(response.Request);
        }      

        private static IRestRequest NewAccessTokenRequest()
        {
            var request = RestRequest("auth/login", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddHeader(AuthInstanceIdString, BacktoryConfig.BacktoryAuthInstanceId);
            request.AddHeader(AuthClientKeyString, BacktoryConfig.BacktoryAuthClientKey);
            request.AddHeader("X-Backtory-Authentication-Refresh", "1");
            request.AddParameter("refresh_token", BacktoryUser.GetRefreshToken(), ParameterType.GetOrPost);
            return request;
        }

        #region json converting
        internal static T FromJson<T>(string jsonString)
        {
            // I'm forced to create a dummy restresponse to make the deserializer work! 
            // Because it doesn't have a method getting string as parameter.
            return new JsonDeserializer().Deserialize<T>(
                            new RestResponse
                            {
                                Content = jsonString
                            });
        }

        // Not tested :)
        internal static object FromJson(string jsonString, Type t)
        {
            //return JsonConvert.DeserializeObject(jsonString, t);
            return new NewtonsoftJsonSerializer().Deserialize(jsonString, t);
        }

        internal static string ToJson(object obj, bool pretty = false)
        {
            var jsonSerializer = new NewtonsoftJsonSerializer(pretty);
            return jsonSerializer.Serialize(obj);
            //var JsonNet = new Newtonsoft.Json.JsonSerializer();
            //if (pretty)
            //    JsonNet.Formatting = Newtonsoft.Json.Formatting.Indented;
            //using (var stringWriter = new StringWriter())
            //{
            //    using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            //    {
            //        JsonNet.Serialize(jsonTextWriter, obj);

            //        return stringWriter.ToString();
            //    }
            //}
        }
        #endregion

        internal static void Dispatch(Action action)
        {
            BacktoryManager.Instance.Invoke(action);
        }
        //internal static BacktoryConfig Config { get; private set; }

        //internal static void init(Config config)
        //{
        //    //Core.init(context);
        //    Config = config;
        //    BacktoryAuth.SetupAuth(config.BacktoryAuthInstanceId);
        //    //BacktoryCloudCode.setupCloudCode(config.BacktoryCloudcodeInstanceId);
        //    //BacktoryGame.setupGame(config.BacktoryGameInstanceId);
        //}
    }

    #region rest response extensions
    internal static class restSharpResponseSuccessfulExtension
    {
        internal static bool IsSuccessful(this IRestResponse restResponse)
        {
            return (int)restResponse.StatusCode < 300 && 
                (int)restResponse.StatusCode >= 200 &&
                restResponse.ResponseStatus == ResponseStatus.Completed;
        }
    }
    #endregion

    
}
