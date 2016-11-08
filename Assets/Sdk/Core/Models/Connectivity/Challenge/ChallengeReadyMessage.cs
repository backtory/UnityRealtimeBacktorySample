using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System.Collections.Generic;
using System;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class ChallengeReadyMessage : BacktoryConnectivityMessage {

		public List<string> Participants { get; set; }
		public string ChallengeId { get; set; }
		public string RealtimeChallengeId { private get; set; }
		public string Address { get; set; }
		public string ExtraMessage { get; set; }

		public String MatchId {
			get { return RealtimeChallengeId; }
		}
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChallengeListener matchmakingListener = (ChallengeListener) backtoryListener;
			matchmakingListener.OnChallengeReady(this);
		}

	}
}
