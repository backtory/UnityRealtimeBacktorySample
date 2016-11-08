using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class ChallengeExpiredMessage : BacktoryConnectivityMessage {

		public string ChallengeId { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChallengeListener matchmakingListener = (ChallengeListener) backtoryListener;
			matchmakingListener.OnChallengeExpired(this);
		}

	}
}
