using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;

namespace Sdk.Core.Models.Realtime.Webhook {
	public class JoinedWebhookMessage : BacktoryMatchMessage {
		public String Message { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnJoinedWebhookMessage(this);
		}
	}
}
