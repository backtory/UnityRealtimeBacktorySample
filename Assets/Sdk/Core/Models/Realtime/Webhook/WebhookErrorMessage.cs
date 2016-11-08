using UnityEngine;
using System.Collections;
using System;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Realtime.Webhook {
	public class WebhookErrorMessage : BacktoryMatchMessage {
		// TODO: add appropriate class instead of this
//		public HttpStatus HttpStatus { get; set; }
		public String Message { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnWebhookErrorMessage(this);
		}
	}
}
