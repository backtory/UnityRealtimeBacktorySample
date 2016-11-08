using UnityEngine;
using System.Collections;
using Sdk.Core.Models.Connectivity.Matchmaking;

namespace Sdk.Core.Listeners {
	public interface MatchmakingListener : BacktoryListener {

		void OnMatchFound(MatchFoundMessage matchFoundMessage);
		
		void OnMatchUpdate(MatchUpdateMessage matchUpdateMessage);
		
		void OnMatchNotFound(MatchNotFoundMessage matchNotFoundMessage);
	
	}
}
