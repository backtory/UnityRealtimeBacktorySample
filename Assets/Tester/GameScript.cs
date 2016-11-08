using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Listeners;
using Sdk.Unity;
using System;
using System.Collections.Generic;
using Sdk.Core.Models.Exception;
using Sdk.Added;
using Sdk.Core.Models.Realtime.Match;
using Sdk.Core.Models.Realtime.Chat;
using Sdk.Core.Models.Realtime.Webhook;
using Sdk.Core.Models;
using Assets.Backtory.core;

namespace Tester
{
	public class GameScript : MonoBehaviour, RealtimeSdkListener, MatchListener
	{

		public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action> ();
		private BacktoryRealtimeMatchUnityApi matchApi;

		// UI elements
		public Text txt;
		public Button btnEvent;
		public Button btnGroupChat;
		public Button btnDirectChat;
		public Button btnSendResult;
		public InputField ifMsg;
		public InputField ifUser;

		// Use this for initialization
		void Start ()
		{
//			UnityThreadHelper.EnsureHelperForce ();
			
			matchApi = BacktoryRealtimeUnityApi.GetMatchApi (MainScript.LastFoundedGameId);
			matchApi.SetRealtimeSdkListener (this);
			matchApi.SetMatchListener (this);

			addClickListeners ();

			// TODO use better name like JoinAndStart or ConnectAndJoin
			BacktoryResponse<ConnectResponse> response = matchApi.ConnectAndJoin ();
			if (response.Successful)
				writeLine ("---connected: " + response.Body.Username + ":" + response.Body.UserId);
			else
				writeLine ("---connect failed with code: " + response.Code + " and message: " + response.Message);
		}

		public void Update ()
		{
			// dispatch stuff on main thread
			while (ExecuteOnMainThread.Count > 0) {
				ExecuteOnMainThread.Dequeue ().Invoke ();
			}
		}

		private void addClickListeners ()
		{
			btnEvent.onClick.AddListener (() => {
				Dictionary<string, string> data = new Dictionary<string, string> ();
				data.Add ("key1", "added data 1");
				data.Add ("key2", "added data 2");
				matchApi.SendEvent (getMessage (), data);
			});
			
			btnGroupChat.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = matchApi.SendChatToMatch (getMessage ());
				if (response.Successful) {
					writeLine ("-+-match chat sent");
				} else {
					writeLine ("-+-group list failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			
			btnDirectChat.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = matchApi.DirectToUser (getUserId (), getMessage ());
				if (response.Successful) {
					writeLine ("-+-direct chat sent");
				} else {
					writeLine ("-+-direct chat failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			
			btnSendResult.onClick.AddListener (() => {
				List<string> winners = new List<string> ();
				winners.Add (getUserId ());
				BacktoryResponse<BacktoryVoid> response = matchApi.SendMatchResult (winners);
				if (response.Successful) {
					writeLine ("-+-winners sent");
				} else {
					writeLine ("-+-sending winners failed with code: " + response.Code + " and message " + response.Message);
				}
			});
		}

		public void OnMatchJoinedMessage (MatchJoinedMessage message)
		{
			writeLine ("-+-user " + message.UserId + ":" + message.Username + " joined to match. All joined json: " + Backtory.ToJson((message.ConnectedUserIds), true));
		}
		
		public void OnMatchStartedMessage (MatchStartedMessage message)
		{
			writeLine ("-+-match started at " + message.StartedDate);
		}
		
		public void OnStartedWebhookMessage (StartedWebhookMessage message)
		{
			writeLine ("-+-started webhook msg: " + message.Message);
		}
		
		public void OnDirectChatMessage (DirectChatMessage message)
		{
			writeLine ("-+-match direct chat " + message.Message + " from " + message.UserId);
		}
		
		public void OnMatchEvent (MatchEvent evt)
		{
			writeLine ("-+- match event with message " + evt.Message + " from " + evt.UserId + " with data json " + Backtory.ToJson((evt.Data), true));
		}
		
		public void OnMatchChatMessage (MatchChatMessage chatMessage)
		{
			writeLine ("-+-match chat: " + chatMessage.Message + " from " + chatMessage.UserId);
		}
		
		public void OnMasterMessage (MasterMessage masterMessage)
		{
			writeLine ("-+-master message " + masterMessage.Message + " with data json " + Backtory.ToJson((masterMessage), true));
		}
		
		public void OnWebhookErrorMessage (WebhookErrorMessage errorMessage)
		{
			writeLine ("-+-webhook error msg: " + errorMessage.Message);
		}
		
		public void OnJoinedWebhookMessage (JoinedWebhookMessage webhookMessage)
		{
			writeLine ("-+-joined webhook msg: " + webhookMessage.Message);
		}
		
		public void OnMatchEndedMessage (MatchEndedMessage endedMessage)
		{
			writeLine ("-+-match ended. Winners json: " + Backtory.ToJson((endedMessage.Winners), true));
			int previousLevel = PlayerPrefs.GetInt ("MainLevel");
			Application.LoadLevel (previousLevel);
		}
		
		public void OnMatchDisconnectMessage (MatchDisconnectMessage dcMessage)
		{
			writeLine ("-+-user disconnected from match -> " + dcMessage.UserId + ":" + dcMessage.Username);
		}

		public void OnConnected (string username, string userId)
		{
			writeLine ("---connected: " + username + " with id " + userId + " ");
		}
		
		public void OnDisconnect ()
		{
			writeLine ("---match disconnected server side");
		}

		public void OnException (ExceptionMessage exception)
		{
			writeLine ("---Exception: " + Backtory.ToJson((exception), true));
		}

		private string getUserId ()
		{
			string text = ifUser.text;
//			if (text.Equals ("ali")) {
//				return "5720b016e4b0bf11f90cdee6";
//			} else if (text.Equals ("javad")) {
//				return "5720b023e4b0bf11f90cdee9";
//			} else if (text.Equals ("farib")) {
//				return "5720b01ee4b0bf11f90cdee8";
//			} else if (text.Equals ("mamad")) {
//				return "5720b01be4b0bf11f90cdee7";
//			} else if (text.Equals ("user1")) {
//				return "57246401e4b000957b56e4c6";
//			}
			return text;
		}
		
		private string getMessage ()
		{
			return ifMsg.text;
		}
			
		List<string> consoleLines = new List<string> ();

		private void writeLine (string line)
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
