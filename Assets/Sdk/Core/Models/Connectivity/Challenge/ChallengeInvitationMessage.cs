using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class ChallengeInvitationMessage : BacktoryConnectivityMessage {
		public string ChallengeId { get; set; }
		public string ChallengerId { get; set; }
		public List<string> challengedUsers { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChallengeListener matchmakingListener = (ChallengeListener) backtoryListener;
			matchmakingListener.OnChallengeInvitation(this);
		}
	}
}
