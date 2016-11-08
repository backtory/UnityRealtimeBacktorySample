using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;
using System.Collections.Generic;

namespace Sdk.Core.Models.Realtime.Match {
	public class MatchJoinedMessage : BacktoryMatchMessage {
		public String UserId { get; set; }
		public String Username { get; set; }
		public List<String> ConnectedUserIds { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMatchJoinedMessage(this);
		}
	}
}
