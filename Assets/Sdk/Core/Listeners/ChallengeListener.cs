using UnityEngine;
using System.Collections;
using Sdk.Core.Models.Connectivity.Challenge;

namespace Sdk.Core.Listeners {
	public interface ChallengeListener : BacktoryListener {

		void OnChallengeInvitation(ChallengeInvitationMessage challengeInvitationMessage);
		
		void OnChallengeAccepted(ChallengeAcceptedMessage challengeAcceptedMessage);
		
		void OnChallengeDeclined(ChallengeDeclinedMessage challengeDeclinedMessage);
		
		void OnChallengeExpired(ChallengeExpiredMessage challengeExpiredMessage);
		
		void OnChallengeImpossible(ChallengeImpossibleMessage challengeImpossibleMessage);
		
		void OnChallengeReady(ChallengeReadyMessage challengeReadyMessage);
		
		void OnChallengeWithoutYou(ChallengeReadyWithoutYou challengeReadyWithoutYou);

	}
}
