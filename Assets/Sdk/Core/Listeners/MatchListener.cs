using UnityEngine;
using System.Collections;
using Sdk.Core.Models.Realtime.Chat;
using Sdk.Core.Models.Realtime.Match;
using Sdk.Core.Models.Realtime.Webhook;

namespace Sdk.Core.Listeners {
	public interface MatchListener : BacktoryListener {

		void OnMatchJoinedMessage(MatchJoinedMessage joinedMessage);
		
		void OnMatchStartedMessage(MatchStartedMessage startedMessage);
		
		void OnStartedWebhookMessage(StartedWebhookMessage startedMessage);
		
		void OnDirectChatMessage(DirectChatMessage directMessage);
		
		void OnMatchEvent(MatchEvent eventMessage);
		
		void OnMatchChatMessage(MatchChatMessage chatMessage);
		
		void OnMasterMessage(MasterMessage masterMessage);
		
		void OnWebhookErrorMessage(WebhookErrorMessage errorMessage);
		
		void OnJoinedWebhookMessage(JoinedWebhookMessage webhookMessage);
		
		void OnMatchEndedMessage(MatchEndedMessage endedMessage);
		
		void OnMatchDisconnectMessage(MatchDisconnectMessage dcMessage);
		
	}
}
