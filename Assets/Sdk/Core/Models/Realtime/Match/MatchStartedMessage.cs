using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Realtime.Match {
	public class MatchStartedMessage : BacktoryMatchMessage {
		public long StartedDate { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMatchStartedMessage(this);
		}
	}
}
