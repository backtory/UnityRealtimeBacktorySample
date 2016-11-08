using UnityEngine;
using System.Collections;
using System;
using Sdk.Core.Listeners;
using Sdk.Added;
using System.Collections.Generic;
using Sdk.Core.Models.Realtime;
using Assets.Backtory.core;

namespace Sdk.Core
{
	public abstract class BacktoryMatchApi : BacktoryApi
	{
		internal string Address { get; set; }

		internal string MatchId { get; set; }

		private MatchListener MatchListener;

		// Alireza: inja type ro as string misze ke hatman ye child az BacktoryMatchMessage e.
		override internal void NotifyMessageReceived (string jsonMessage, string _type)
		{
			if (_type != null) {
				Type clazz = BacktoryRealtimeMessageFactory.GetType (_type);
				// TODO uncomment, if it is important???????
//				JsonElement mJson =  new JsonParser().parse(jsonMessage.toJSONstring());
//				if (mJson == null) {
//					// TODO: 7/18/16 AD What to do?
//				}
				var message = (BacktoryMatchMessage) Backtory.FromJson(jsonMessage, clazz); //(new Gson()).fromJson(mJson, clazz);
				// TODO: 7/18/16 AD Use better mechanism for checking requestId:
				if (connectorClient.CheckRequestId (jsonMessage, _type)) {
					// TODO IMPORTANT : move this action on main thread
					if (BacktoryRealtimeMessageFactory.GetTypeCategory (_type).Equals (BacktoryRealtimeMessageFactory.TypeCategory.Challenge) && MatchListener != null) {
						message.OnMessageReceived (MatchListener);
					} else if (BacktoryRealtimeMessageFactory.GetTypeCategory (_type).Equals (BacktoryRealtimeMessageFactory.TypeCategory.General) && connectorClient.sdkListener != null) {
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
		
		public void SetMatchListener (MatchListener matchListener)
		{
			this.MatchListener = matchListener;
		}
		
		override
		internal BacktorySender GetSender (string destination, Dictionary<string, object> data)
		{
			Dictionary<string, string> extraHeader = new Dictionary<string, string> ();
			extraHeader.Add ("X-Backtory-Challenge-Id", MatchId);
			return connectorClient.getPlatformAbstractionLayer ().GetSender (connectorClient, destination, extraHeader, data);
		}
	}
}
