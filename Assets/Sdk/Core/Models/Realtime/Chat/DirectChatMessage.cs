using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;

namespace Sdk.Core.Models.Realtime.Chat {
	public class DirectChatMessage : BacktoryMatchMessage {
		public String Message { get; set; }
		public String UserId { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnDirectChatMessage(this);
		}
	}
}
