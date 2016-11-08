using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;
using System.Collections.Generic;

namespace Sdk.Core.Models.Realtime.Match {
	public class MatchEndedMessage : BacktoryMatchMessage {
		public List<String> Winners { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMatchEndedMessage(this);
		}
	}
}
