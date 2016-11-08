using UnityEngine;
using System.Collections;
using System;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Matchmaking {
	public class MatchNotFoundMessage : BacktoryConnectivityMessage {
		private String MatchmakingName { get; set; }
		private String RequestId { get; set; }
		private int? Skill { get; set; }
		private String MetaData { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchmakingListener matchmakingListener = (MatchmakingListener) backtoryListener;
			matchmakingListener.OnMatchNotFound(this);
		}
	}
}
