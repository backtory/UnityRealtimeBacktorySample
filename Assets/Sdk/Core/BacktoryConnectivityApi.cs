using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using Sdk.Added;
using System;
using Sdk.Core.Models.Connectivity;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Models.Connectivity.Challenge;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Backtory.core;

namespace Sdk.Core
{
	public abstract class BacktoryConnectivityApi : BacktoryApi
	{
		protected MatchmakingListener matchmakingListener;
		protected ChallengeListener challengeListener;
		protected ChatListener chatListener;
		
		override
		internal void NotifyMessageReceived (string jsonMessage, string _type)
		{
			Debug.Log ("==::::=> ");
			if (_type != null) {
				Type clazz = BacktoryConnectivityMessageFactory.GetType (_type);
				BacktoryConnectivityMessage message = (BacktoryConnectivityMessage) Backtory.FromJson(jsonMessage, clazz);
				if (connectorClient.CheckRequestId (jsonMessage, _type)) {
					// TODO IMPORTANT : move this action on main thread
					if (BacktoryConnectivityMessageFactory.GetTypeCategory (_type).Equals (BacktoryConnectivityMessageFactory.TypeCategory.Matchmaking) && matchmakingListener != null) {
						if (_type.Equals (BacktoryConnectivityMessage.MATCH_FOUND_MESSAGE)) {
							GenerateRealtimeMatch ((MatchFoundMessage)message);
						}
						message.OnMessageReceived (matchmakingListener);
					} else if (BacktoryConnectivityMessageFactory.GetTypeCategory (_type).Equals (BacktoryConnectivityMessageFactory.TypeCategory.Challenge) && challengeListener != null) {
						if (_type.Equals (BacktoryConnectivityMessage.CHALLENGE_READY_MESSAGE)) {
							GenerateRealtimeMatch ((ChallengeReadyMessage)message);
						}
						message.OnMessageReceived (challengeListener);
					} else if (BacktoryConnectivityMessageFactory.GetTypeCategory (_type).Equals (BacktoryConnectivityMessageFactory.TypeCategory.Chat) && chatListener != null) {
						message.OnMessageReceived (chatListener);
						JSONNode jsonNode = JSON.Parse (jsonMessage);
						SendDelivery ((string)jsonNode ["deliveryId"]);
					} else if (BacktoryConnectivityMessageFactory.GetTypeCategory (_type).Equals (BacktoryConnectivityMessageFactory.TypeCategory.General) && connectorClient.sdkListener != null) {
						message.OnMessageReceived (connectorClient.sdkListener);
					} else {
						// TODO: 7/20/16 AD What to do?
					}
				}
			} else {
				// TODO: 7/23/16 AD What to do?
//				connectorClient.sdkListener.OnMessage (jsonMessage);
			}
		}
		
		internal abstract void GenerateRealtimeMatch (MatchFoundMessage message);
		
		internal abstract void GenerateRealtimeMatch (ChallengeReadyMessage message);
		
		public void setMatchmakingListener (MatchmakingListener matchmakingListener)
		{
			this.matchmakingListener = matchmakingListener;
		}
		
		public void setChallengeListener (ChallengeListener challengeListener)
		{
			this.challengeListener = challengeListener;
		}
		
		public void setChatListener (ChatListener chatListener)
		{
			this.chatListener = chatListener;
		}
		
		override
		internal string GetServiceUrl ()
		{
			return "wss://ws.backtory.com/connectivity/ws";
			//return "ws://192.168.0.107:8090/ws";
			//        return "ws://192.168.43.72:8090/ws";
		}
	}
}
