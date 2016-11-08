using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;

namespace Sdk.Core.Models.Realtime.Match {
public class MatchDisconnectMessage : BacktoryMatchMessage {
		public String UserId { get; set; }
		public String Username { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMatchDisconnectMessage(this);
		}
	}
}
