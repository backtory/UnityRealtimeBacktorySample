using UnityEngine;
using System.Collections;
using System;
using Realtime.Messaging.Internal;
using Sdk.Core.Listeners;
using System.Collections.Generic;
using Sdk.Added;
using Sdk.Core.Models;
using Assets.Backtory.core;
using SimpleJSON;

namespace Sdk.Core
{
	public class ConnectorClient
	{

		private static string TAG = "BacktorySdk->" + typeof(ConnectorClient).Name + " ";
		private static PlatformAbstractionLayer platformAbstractionLayer;
		private const int TIMEOUT_IN_SECONDS = 3;
		private const int LATCH_COUNT = 1;
		// TODO add params:
		private ConcurrentDictionary<string, DataReceivedEvent> pendingRequests = new ConcurrentDictionary<string, DataReceivedEvent> ();//ConcurrentHashMap(10, 0.75f, 1);
		internal CountDownLatch connectLatch;
		internal CountDownLatch disconnectLatch;
		internal ConnectorStateEngine connectorStateEngine;
		internal BacktoryWebSocketAbstractionLayer webSocket;
		private BacktoryApi backtoryApi;
		private BacktoryApi BacktoryApi;
		internal RealtimeSdkListener sdkListener;
		private List<string> MatchmakingRequestIdList = new List<string> ();
		
		public ConnectorClient (BacktoryApi backtoryApi)
		{
			this.backtoryApi = backtoryApi;
			this.connectorStateEngine = new ConnectorStateEngine (this);
		}
		
		public BacktoryResponse<ConnectResponse> Connect ()
		{
			// TODO: 8/10/16 AD User better response code and message for fail returns

			if (connectorStateEngine.getConnectionState () == ConnectorStateEngine.ConnectorState.CONNECTED) {
				return new BacktoryResponse<ConnectResponse> (1000, "Connected now", null, false);
			}
			
			if (connectorStateEngine.getConnectionState () != ConnectorStateEngine.ConnectorState.STOPPED) {
				return new BacktoryResponse<ConnectResponse> (1000, "Action is running", null, false);
			}
			
			if (!platformAbstractionLayer.IsNetworkAvailable ()) {
				return new BacktoryResponse<ConnectResponse> (1000, "Network Unavailable", null, false);
			}

			return connectSynchronized ();
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		private BacktoryResponse<ConnectResponse> connectSynchronized() {
			connectLatch = new CountDownLatch (LATCH_COUNT);
			Debug.Log (TAG + "connecting");
			connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.CONNECT);
			try {
				if (connectLatch.Wait (TIMEOUT_IN_SECONDS * 1000)) {
					if (connectorStateEngine.getConnectionState() == ConnectorStateEngine.ConnectorState.CONNECTED) 
					{
						ConnectResponse response = new ConnectResponse (webSocket.userId, webSocket.username);
						return BacktoryResponse<ConnectResponse>.Success((int)BacktoryHttpStatusCode.OK, response);
					} else {
						return new BacktoryResponse<ConnectResponse> (1000, "Error connecting", default(ConnectResponse), false);
					}
				} else {
					Debug.LogError (TAG + "Timeout waiting to connect");
					connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.GENERAL_ERROR);
					return new BacktoryResponse<ConnectResponse> (1000, "Timeout waiting to connect", default(ConnectResponse), false);
				}
			} catch (Exception e) {
				Debug.LogError (TAG + e.Data);
				return new BacktoryResponse<ConnectResponse> (1000, "Error while connecting", default(ConnectResponse), false);
			}
		}

		public BacktoryResponse<BacktoryVoid> Disconnect ()
		{
			// TODO: 8/10/16 AD User better response code and message for fail returns
			if (connectorStateEngine.getConnectionState () == ConnectorStateEngine.ConnectorState.STOPPED) {
				return new BacktoryResponse<BacktoryVoid> (1000, "You are not connected", default(BacktoryVoid), false);
			}
			
			if (connectorStateEngine.getConnectionState () != ConnectorStateEngine.ConnectorState.CONNECTED) {
				return new BacktoryResponse<BacktoryVoid> (1000, "Action is running", default(BacktoryVoid), false);
			}

			return disconnectSyncronized ();
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		private BacktoryResponse<BacktoryVoid> disconnectSyncronized() {
			disconnectLatch = new CountDownLatch (LATCH_COUNT);
			Debug.Log (TAG + "disconnecting");
			connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.DISCONNECT);
			try {
				if (disconnectLatch.Wait (TIMEOUT_IN_SECONDS * 1000)) {
					return BacktoryResponse<BacktoryVoid>.Success(200, default(BacktoryVoid));
				} else {
					Debug.LogError (TAG + "Timeout waiting to connect");
					connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.GENERAL_ERROR);
					return new BacktoryResponse<BacktoryVoid> (1000, "Timeout waiting to connect", default(BacktoryVoid), false);
				}
			} catch (Exception e) {
				Debug.LogError (TAG + e.Data);
				return new BacktoryResponse<BacktoryVoid> (1000, "Error while connecting", default(BacktoryVoid), false);
			}
		}

		internal void SendDisconnect() {
			webSocket.Disconnect();
		}

		internal void OnDisconnect() {
			if (connectorStateEngine.getConnectionState() == ConnectorStateEngine.ConnectorState.DISCONNECTING /*&& disconnectLatch.getCount() == 1*/) {
				disconnectLatch.Signal();
			} else {
				Backtory.Dispatch( () => sdkListener.OnDisconnect() );
			}
			connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.CONNECTED);
		}

		internal void CreateWSAndConnect (string url, string xBacktoryConnectivityId)
		{
			
			// I commented this line, because I think that if previous websocket be connected,
			// createWSAndConnect will not be called
//			if (webSocket != null) {
//				webSocket.Disconnect ();
//			}

			webSocket = backtoryApi.CreateWebSocket (url, xBacktoryConnectivityId, new InnerWebSocketListener (this));

			if (backtoryApi is BacktoryConnectivityApi) {
				webSocket.Connect (null);
			} else {
				string matchId = ((BacktoryMatchApi)backtoryApi).MatchId;
				webSocket.Connect (matchId);
			}
		}
		
		// Message Processing Part ---------------------------------------------------------------------
		internal void ProcessMessage (string message)
		{
			if ((message != null) && (message.Length != 0)) {
				try {
					JSONNode jsonNode = JSONNode.Parse (message);
					string clazz = (string)jsonNode ["_type"];
					if (clazz != null) {
						string clientRequestId = (string)jsonNode ["clientRequestId"];
						if (clientRequestId != null) {
							sendResponseToMainThread (message, clazz, clientRequestId);
						} else {
							Debug.Log("************");
							Backtory.Dispatch (() => backtoryApi.NotifyMessageReceived (message, clazz));
						}
					} else {
						Debug.Log (TAG + "_type field not found in message");
					}
				} catch (Exception e) {
					Debug.Log (TAG + e.Data);
				}
			} else {
				// TODO
			}
		}
		
		internal bool CheckRequestId (JSONNode jsonNode, string _type)
		{
			string requestId = (string)jsonNode ["requestId"];
			if (requestId == null) {
				return true;
			}
			if (!MatchmakingRequestIdList.Contains (requestId)) {
				return false;
			}
			if (_type.Equals (BacktoryConnectivityMessage.MATCH_FOUND_MESSAGE) ||
				_type.Equals (BacktoryConnectivityMessage.MATCH_NOT_FOUND_MESSAGE)) {
				MatchmakingRequestIdList.Remove (requestId);
				return true;
			}
			if (_type.Equals (BacktoryConnectivityMessage.MATCH_UPDATE_MESSAGE)) {
				return true;
			}
			return false;
		}
		
		private void sendResponseToMainThread (string message, string clazz, string clientRequestId)
		{
			JSONNode jsonNode = JSONNode.Parse (message);
			if (clazz.Equals (".OfflineChatMessageListResponse")) {
				List<string> deliveryIdList = getDeliveryIdList (jsonNode);
				if (deliveryIdList.Count > 0)
					backtoryApi.SendDeliveryList (deliveryIdList);
			}
			
			string requestId = jsonNode ["requestId"];
			if (requestId != null && !requestId.Equals ("")) {
				MatchmakingRequestIdList.Add (requestId);
			}
			
			DataReceivedEvent receivedEvent = pendingRequests [clientRequestId];
			if (receivedEvent != null) {
				receivedEvent.data = message;
				// TODO CHECK
				receivedEvent.latch.Signal ();
			} else {
				Debug.Log (TAG + "No data found for request id: " + clientRequestId);
			}
		}
		
		// TODO use main sdk json methods
		private List<string> getDeliveryIdList (JSONNode jsonNode)
		{
			List<string> deliveryIdList = new List<string> ();
			JSONArray array = (JSONArray)jsonNode ["messageList"];
			for (int i = 0; i < array.Count; i++) {
				JSONNode obj = (JSONNode)array [i];
				deliveryIdList.Add ((string)obj ["deliveryId"]);
			}
			return deliveryIdList;
		}

		// Sending Part --------------------------------------------------------------------------------		
		private class DataReceivedEvent
		{
			internal CountDownLatch latch;
			internal string requestId;
			internal string data;
			
			internal DataReceivedEvent (string requestId)
			{
				this.requestId = requestId;
				this.latch = new CountDownLatch (LATCH_COUNT);
			}
		}
		
		public string Send (string destination, Dictionary<string, object> jsonData, Dictionary<string, string> extraHeader)
		{
			if (!isConnected ())
				throw new Exception ("WS is not connected");
			
			try {
				DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				string clientRequestId = ((long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds).ToString();
				jsonData.Add ("clientRequestId", clientRequestId);
				DataReceivedEvent dataReceivedEvent = new DataReceivedEvent (clientRequestId);
				pendingRequests.TryAdd (clientRequestId, dataReceivedEvent);
				sendNoWait (destination, jsonData, extraHeader);
				waitForResponseFromServer (dataReceivedEvent);
				
				return dataReceivedEvent.data;
			} catch (Exception) {
				// TODO uncomment up lines
				return null;
			}
		}
		
		public void SendFast (string destination, Dictionary<string, object> jsonData, Dictionary<string, string> extraHeader)
		{
			if (!isConnected ())
				platformAbstractionLayer.LogError ("WS is not connected");
			
			try {
				sendNoWait (destination, jsonData, extraHeader);
			} catch (Exception) {
				// TODO uncomment up lines
				platformAbstractionLayer.LogError ("Error while sending");
			}
		}
		
		public Dictionary<string, object> sendNoWait (string destination, Dictionary<string, object> jsonData, Dictionary<string, string> extraHeader)
		{
			if (!isConnected ())
				throw new Exception ("WS is not connected");
			
			string jsonstring = Backtory.ToJson(jsonData);
			webSocket.Send (destination, jsonstring, extraHeader);
			Dictionary<string, object> result = new Dictionary<string, object> ();
			result.Add ("success", "message sent");
			return result;
		}
		
		private void waitForResponseFromServer (DataReceivedEvent dataReceivedEvent)
		{
			try {
				dataReceivedEvent.latch.Wait (TIMEOUT_IN_SECONDS * 1000);
			} catch (Exception e) {
				// TODO
				Debug.Log (TAG + e.Data);
			} finally {
				// TODO don't pass new object as second parameters
				DataReceivedEvent temp = new DataReceivedEvent ("");
				pendingRequests.TryRemove (dataReceivedEvent.requestId, out temp);
			}
		}
		
		// Getters & Setters ---------------------------------------------------------------------------
		public void setPlatformAbstractionLayer (PlatformAbstractionLayer backtoryPlatform)
		{
			platformAbstractionLayer = backtoryPlatform;
		}
		
		private bool isConnected ()
		{
			return connectorStateEngine.getConnectionState () == ConnectorStateEngine.ConnectorState.CONNECTED;
		}
		
		public string GetConnectivityId ()
		{
			return backtoryApi.GetConnectivityId ();
		}
		
		public string GetServiceUrl ()
		{
			return backtoryApi.GetServiceUrl ();
		}
		
		public PlatformAbstractionLayer getPlatformAbstractionLayer ()
		{
			return platformAbstractionLayer;
		}
		
		public void SetRealtimeSdkListener (RealtimeSdkListener realtimeSdkListener)
		{
			this.sdkListener = realtimeSdkListener;
		}
		
	}

	// Inner Classes -----------------------------------------------------------------------------------
	class InnerWebSocketListener : WebSocketListener
	{

		private ConnectorClient outClass;
		private static string TAG = "BacktorySdk->" + typeof(InnerWebSocketListener).Name + " ";

		public InnerWebSocketListener (ConnectorClient connectorClient)
		{
			outClass = connectorClient;
		}

		public void OnConnect ()
		{
			Debug.Log (TAG + "WebSocket connected");
			outClass.connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.CONNECTED);		
			outClass.connectLatch.Signal ();
		}
		
		public void OnMessage (string message)
		{
			outClass.ProcessMessage (message);
		}

		public void OnDisconnect ()
		{
			outClass.OnDisconnect ();
		}

		public void OnError (Exception exception)
		{
			bool isConnecting = outClass.connectorStateEngine.getConnectionState() == ConnectorStateEngine.ConnectorState.CONNECTING;
			outClass.connectorStateEngine.ChangeState(ConnectorStateEngine.StateChangeEvent.GENERAL_ERROR);
			if (isConnecting) {
				outClass.connectLatch.Signal();
			}
			if (outClass.webSocket.IsAlive ()) {
				outClass.connectorStateEngine.ChangeState (ConnectorStateEngine.StateChangeEvent.GENERAL_ERROR);
			}
		}
	}

}
