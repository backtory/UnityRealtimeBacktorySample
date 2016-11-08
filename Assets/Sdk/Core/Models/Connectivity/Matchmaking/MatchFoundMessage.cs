using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Matchmaking {
	public class MatchFoundMessage : BacktoryConnectivityMessage {

		public String MatchmakingName { get; set; }
		public String RequestId { get; set; }
		public List<MatchFoundParticipant> Participants { get; set; }
		public String RealtimeChallengeId { private get; set; }
		public String Address { get; set; }
		public String ExtraMessage { get; set; }

		public String MatchId {
			get { return RealtimeChallengeId; }
		}
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchmakingListener matchmakingListener = (MatchmakingListener) backtoryListener;
			matchmakingListener.OnMatchFound(this);
		}

//		private string _MatchmkingName;
//		public string MatchmakingName {
//			get { return _MatchmkingName; }
//			set { _MatchmkingName = value; }
//		}
	}
}