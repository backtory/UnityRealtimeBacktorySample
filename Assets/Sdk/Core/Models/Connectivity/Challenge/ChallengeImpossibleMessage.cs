using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class ChallengeImpossibleMessage : BacktoryConnectivityMessage {

		public string ChallengeId;
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChallengeListener matchmakingListener = (ChallengeListener) backtoryListener;
			matchmakingListener.OnChallengeImpossible(this);
		}
	}
}
