using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;
using System.Collections.Generic;

namespace Sdk.Core.Models.Connectivity.Matchmaking {
	public class MatchUpdateMessage : BacktoryConnectivityMessage {
		public List<MatchUpdateParticipant> Participants { get; set; }
		public String RequestId { get; set; }
		public String ExtraMessage { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchmakingListener matchmakingListener = (MatchmakingListener) backtoryListener;
			matchmakingListener.OnMatchUpdate(this);
		}
	}
}
