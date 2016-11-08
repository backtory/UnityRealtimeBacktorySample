using UnityEngine;
using System.Collections;
using System;
using Sdk.Added;
using System.Collections.Generic;
using Sdk.Unity;
using Sdk.Core.Listeners;
using Sdk.Core.Models.Exception;
using Sdk.Core.Models.Connectivity.Chat;
using SimpleJSON;
using Sdk.Core.Models;
using Assets.Backtory.core;

namespace Sdk.Core
{
	public abstract class BacktoryApi
	{

		protected ConnectorClient connectorClient;

		internal abstract string GetConnectivityId ();

		internal abstract string GetServiceUrl ();

		internal abstract void NotifyMessageReceived (string jsonMessage, string type);
		
		public BacktoryApi ()
		{
			connectorClient = new ConnectorClient (this);
		}

		internal BacktoryWebSocketAbstractionLayer CreateWebSocket (string url, string xBacktoryConnectivityId, WebSocketListener webSocketListener)
		{
			return new InnerUnityWebSocket (this, url, webSocketListener, xBacktoryConnectivityId);
		}

		public BacktoryResponse<ConnectResponse> Connect ()
		{
			return connectorClient.Connect ();
		}

		public BacktoryResponse<BacktoryVoid> Disconnect ()
		{
			return connectorClient.Disconnect ();
		}
		
		internal void SendDelivery (string deliveryId)
		{
			string destination = "/app/chat/delivery";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("deliveryId", deliveryId, ref apiParamData);
			BacktorySender sender = GetSender (destination, apiParamData);
			// TODO threading
			sender.SendFast ();
		}
		
		internal void SendDeliveryList (List<string> deliveryIdList)
		{
			string destination = "/app/chat/deliveryList";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("deliveryIdList", deliveryIdList, ref apiParamData);
			BacktorySender sender = GetSender (destination, apiParamData);
			// TODO threading
			sender.SendFast ();
		}

		public void SetRealtimeSdkListener (RealtimeSdkListener realtimeSdkListener)
		{
			connectorClient.SetRealtimeSdkListener (realtimeSdkListener);
		}

		internal void setApiParamData (string key, object value, ref Dictionary<string, object> data)
		{
			if (value != null) {
				// TODO check other types
//				if (value.GetType == typeof(Date)) {
//					data.Add(key, convertDateTostring((Date) value));
//				} else if (value.GetType == typeof(string[])) {
//					List<string> values = new List<string>();
//					for (string str : (string[]) value) {
//						values.Add(str);
//					}
//					data.Add(key, values);
//				} else if (value.GetType == typeof(Enum)) {
//					data.Add(key, value.tostring());
//				} else {
				data.Add (key, value);
//				}
			} 
		}

		internal BacktoryResponse<T> SendAndReceive<T> (string destination, Dictionary<string, object> apiParamData, Type clazz) where T : class
		{
			BacktorySender sender = GetSender (destination, apiParamData);
			string responseStr;
			try {
				responseStr = sender.Send ();
			} catch (Exception) {
				return BacktoryResponse<T>.Error(1000, "Not Connected");
			}
			return GenerateBacktoryResponse<T> (responseStr, clazz);
		}

		private BacktoryResponse<T> GenerateBacktoryResponse<T> (string responseJson, Type clazz) where T : class
		{
			Debug.Log("jsonnode: " + responseJson);
			if (responseJson == null) {
				return BacktoryResponse<T>.Error((int)BacktoryHttpStatusCode.RequestTimeout, null);
			}
			JSONNode jsonNode = JSONNode.Parse (responseJson);
			string _type = jsonNode ["_type"];
			if (_type.Equals (".ExceptionResponse")) {
				ExceptionResponse exceptionResponse = Backtory.FromJson<ExceptionResponse> (responseJson);
				string message = exceptionResponse.Exception.Message;
				int errorCode = (int) exceptionResponse.Exception.Code;
				return BacktoryResponse<T>.Error (errorCode, message);
			} else if (typeof(T).Equals(typeof(GroupChatHistoryResponse))) {
				T body = GroupChatHistoryResponse.Get (responseJson) as T;
				return BacktoryResponse<T>.Success(200, body);
			} else if (typeof(T).Equals(typeof(UserChatHistoryResponse))) {
				T body = UserChatHistoryResponse.Get (responseJson) as T;
				return BacktoryResponse<T>.Success(200, body);
			} else if (typeof(T).Equals(typeof(OfflineMessageResponse))) {
				T body = OfflineMessageResponse.Get (responseJson) as T;
				return BacktoryResponse<T>.Success(200, body);
			} else {
				T body;
				if (clazz != typeof(BacktoryVoid))
					body = Backtory.FromJson(responseJson, clazz) as T;
				else
					body = default(BacktoryVoid) as T;
				return BacktoryResponse<T>.Success(200, body);
			}
		}
		
		virtual
		internal BacktorySender GetSender (string destination, Dictionary<string, object> data)
		{
			return connectorClient.getPlatformAbstractionLayer ().GetSender (connectorClient, destination, data);
		}	

		internal class InnerUnityWebSocket : UnityWebSocket
		{

			BacktoryApi outClass;

			internal InnerUnityWebSocket (BacktoryApi backtoryApi, string url, WebSocketListener webSocketListener, string xBacktoryInstanceId) : base(url, webSocketListener, xBacktoryInstanceId)
			{
				outClass = backtoryApi;
			}

			override
			public Dictionary<string, string> getExtraHeaders ()
			{
				Dictionary<string, string> connectHeaders = new Dictionary<string, string> ();
				connectHeaders.Add ("X-Backtory-Connectivity-Id", X_BACKTORY_CONNECTIVITY_ID);
				if (outClass is BacktoryMatchApi)
					connectHeaders.Add ("X-Backtory-Challenge-Id", ((BacktoryMatchApi)outClass).MatchId);

				return connectHeaders;
			}
		}
	}
}
