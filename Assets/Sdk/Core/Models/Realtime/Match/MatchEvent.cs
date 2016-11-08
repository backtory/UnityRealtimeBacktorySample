using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;
using System.Collections.Generic;

namespace Sdk.Core.Models.Realtime.Match {
	public class MatchEvent : BacktoryMatchMessage {
		public Dictionary<String, String> Data { get; set; }
		public String Message { get; set; }
		public String UserId { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMatchEvent(this);
		}
	}
}
