using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System.Collections.Generic;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class ChallengeDeclinedMessage : BacktoryConnectivityMessage {
		public string ChallengeId { get; set; }
		public string ChallengerId { get; set; }
		public string DeclinedId { get; set; }
		public List<ChallengedUserInfo> ChallengedUsers { get; set; }
		public int MinPlayer { get; set; }
		public long CreationDate { get; set; }
		public long WaitTime { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChallengeListener matchmakingListener = (ChallengeListener) backtoryListener;
			matchmakingListener.OnChallengeDeclined(this);
		}
	}
}
