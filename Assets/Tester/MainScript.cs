using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Listeners;
using Sdk.Core.Models.Connectivity.Challenge;
using Sdk.Unity;
using System;
using System.Collections.Generic;
using Sdk.Core.Models.Exception;
using Sdk.Added;
using Sdk.Core.Models.Challenge;
using Sdk.Core.Models;
using Assets.Backtory.core;
using UnityEngine.SceneManagement;

namespace Tester
{
    public class MainScript : MonoBehaviour, ChallengeListener, MatchmakingListener, RealtimeSdkListener
    {
        
        public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action> ();
        public static string LastFoundedGameId;
        
		private BacktoryRealtimeUnityApi backtoryApi;
        private string lastMatchmakingRequestId;
        private string lastChallengeInvitationId;
        private string matchmakingName = "matchmaking1";

        // UI elements
        public Text txt;
		public Button btnLogin;
		public Button btnConnect;
        public Button btnDisconnect;
        public Button btnMatchmaking;
        public Button btnCancelMM;
        public Button btnChallenge;
        public Button btnChallengeList;
        public Button btnAccept;
        public Button btnDecline;
        public Button btnChat;
        public InputField ifUsername;
        public InputField ifPassword;

        // Use this for initialization
        void Start ()
        {
            PlayerPrefs.SetInt ("MainLevel", Application.loadedLevel);

            // TODO: adapt to other parts of sdk
			BacktoryRealtimeUnityApi.Initialize (BacktoryConfig.BacktoryConnectivityInstanceId);

			backtoryApi = BacktoryRealtimeUnityApi.Instance ();
            backtoryApi.SetRealtimeSdkListener (this);
            backtoryApi.SetMatchmakingListener (this);
            backtoryApi.SetChallengeListener (this);

            ifUsername.text = "user1";
            ifPassword.inputType = InputField.InputType.Password;
            ifPassword.text = "a";
			
			addClickListeners ();
        }

        public void Update ()
        {
            // dispatch stuff on main thread
            while (ExecuteOnMainThread.Count > 0) {
                ExecuteOnMainThread.Dequeue ().Invoke ();
            }
        }

        private void connect ()
        {
            BacktoryResponse<ConnectResponse> response = backtoryApi.Connect ();
            if (response.Successful)
                writeLine ("---connected: " + response.Body.Username + ":" + response.Body.UserId);
            else
                writeLine ("---connect failed with code: " + response.Code + " and message: " + response.Message);
        }

        private void addClickListeners ()
        {
			btnLogin.onClick.AddListener (() => {
				string username = ifUsername.text;
				string password = ifPassword.text;
				BacktoryUser.LoginInBackground(username, password, loginResponse =>
					{
						if (loginResponse.Successful)
							writeLine("logged in!");
						else
							writeLine("Unable to login=> " + loginResponse.Code + ":" + loginResponse.Message);
					});
			});

            btnConnect.onClick.AddListener (() => {
				Debug.Log("AccessToken: " + BacktoryUser.GetAccessToken() );
				if (BacktoryUser.GetAccessToken() == null) {
					writeLine("login first");
					return;
				}
                connect ();
            });
            
            btnDisconnect.onClick.AddListener (() => {
                BacktoryResponse<BacktoryVoid> response = backtoryApi.Disconnect ();
                if (response.Successful) {
                    writeLine ("---disconnect successful");
                } else {
                    writeLine ("---disconnect failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnMatchmaking.onClick.AddListener (() => {
				BacktoryResponse<MatchmakingResponse> response = backtoryApi.RequestMatchmaking (matchmakingName, 55, "sample meta data");
                if (response.Successful) {
                    lastMatchmakingRequestId = response.Body.RequestId;
                    writeLine ("---matchmaking successful. id: " + lastMatchmakingRequestId);
                } else {
                    writeLine ("---matchmaking failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnCancelMM.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.CancelMatchmaking (matchmakingName, lastMatchmakingRequestId);
                if (response.Successful) {
                    writeLine ("---matchmaking cancelled successful.");
                } else {
                    writeLine ("---matchmaking cancellation failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnChallenge.onClick.AddListener (() => {
                List<String> users = new List<String> ();
                users.Add ("5720b016e4b0bf11f90cdee6"); // ali
                users.Add ("5720b01be4b0bf11f90cdee7"); // mamad
                users.Add ("5720b01ee4b0bf11f90cdee8"); // farib
				BacktoryResponse<ChallengeResponse> response = backtoryApi.RequestChallenge (users, 15, 2);
                if (response.Successful) {
                    writeLine ("---challenge request successful. challenge id: " + response.Body.ChallengeId);
                } else {
                    writeLine ("---challenge request failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnChallengeList.onClick.AddListener (() => {
				BacktoryResponse<ActiveChallengesListResponse> response = backtoryApi.RequestListOfActiveChallenges ();
                if (response.Successful) {
                    writeLine ("---active challenges list json: " + Backtory.ToJson((response.Body), true));
                } else {
                    writeLine ("---active challenges list failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnAccept.onClick.AddListener (() => {
                BacktoryResponse<BacktoryVoid> response = backtoryApi.AcceptChallenge (lastChallengeInvitationId);
                if (response.Successful) {
                    writeLine ("---challenge accepted successful.");
                } else {
                    writeLine ("---accepting challenge failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnDecline.onClick.AddListener (() => {
                BacktoryResponse<BacktoryVoid> response = backtoryApi.DeclineChallenge (lastChallengeInvitationId);
                if (response.Successful) {
                    writeLine ("---challenge declined successful.");
                } else {
                    writeLine ("---declining challenge failed with code: " + response.Code + " and message " + response.Message);
                }
            });
            
            btnChat.onClick.AddListener (() => SceneManager.LoadScene("ChatScene"));
        }

        public void OnChallengeInvitation (ChallengeInvitationMessage message)
        {
            lastChallengeInvitationId = message.ChallengeId;
            writeLine ("---challenge invitation to " + message.ChallengeId + " by " + message.ChallengerId + " with challenged users " + Backtory.ToJson(message.challengedUsers, true));
        }
        
        public void OnChallengeAccepted (ChallengeAcceptedMessage message)
        {
            writeLine ("---challenge " + message.ChallengeId + " accepted by " + message.AcceptedId + " and all info json: " + Backtory.ToJson(message, true));
        }
        
        public void OnChallengeDeclined (ChallengeDeclinedMessage message)
        {
            writeLine ("---challenge " + message.ChallengeId + " declined by " + message.DeclinedId + " and all info json: " + Backtory.ToJson(message, true));
        }
        
        public void OnChallengeExpired (ChallengeExpiredMessage message)
        {
            writeLine ("---challenge expired: " + message.ChallengeId);
        }
        
        public void OnChallengeImpossible (ChallengeImpossibleMessage message)
        {
            writeLine ("---challenge impossible: " + message.ChallengeId);
        }
        
        public void OnChallengeReady (ChallengeReadyMessage message)
        {
            writeLine ("---challenge ready " + message.ChallengeId + " and all info json: " + Backtory.ToJson(message, true));
            LastFoundedGameId = message.MatchId;
            ExecuteOnMainThread.Enqueue (() => { 
                SceneManager.LoadScene("GameScene");
            });
        }
        
        public void OnChallengeWithoutYou (ChallengeReadyWithoutYou message)
        {
            writeLine ("---challenge ready without you " + message.ChallengeId);
        }

        public IEnumerator ThisWillBeExecutedOnTheMainThread ()
        {
            Debug.LogError ("This is executed from the main thread");
            yield return null;
        }

        public void OnMatchFound (MatchFoundMessage message)
        {
            LastFoundedGameId = message.MatchId;
            writeLine ("---match found with json: " + Backtory.ToJson(message, true));
			SceneManager.LoadScene ("GameScene");
        }
        
        public void OnMatchUpdate (MatchUpdateMessage message)
        {
            writeLine ("---match update with json " + Backtory.ToJson(message, true));
        }
        
        public void OnMatchNotFound (MatchNotFoundMessage message)
        {
            writeLine ("---match not found with json " + Backtory.ToJson(message, true));
        }

        public void OnDisconnect ()
        {
            writeLine ("---disconnected server side");
        }

        public void OnException (ExceptionMessage exception)
        {
			writeLine ("---Exception: " + Backtory.ToJson((exception), true));
        }

        List<String> consoleLines = new List<String> ();

        private void writeLine (String line)
        {
            consoleLines.Add (line);
            if (consoleLines.Count == 31)
                consoleLines.RemoveAt (0);


            txt.text = "";
            for (int i = 0; i<consoleLines.Count; i++) {
                txt.text += consoleLines [i] + "\n";
            }
        }
    }
}
