using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sdk.Core.Models.Exception;
using Sdk.Core;
using Sdk.Core.Models.Realtime.Match;
using Sdk.Core.Models.Realtime.Chat;
using Sdk.Core.Models.Realtime.Webhook;

namespace Sdk.Core.Models.Realtime {
	public class BacktoryRealtimeMessageFactory {
		
		private static readonly Dictionary<string, Type> TYPE_CLASS = new Dictionary<string, Type> {
			{ BacktoryMessage.EXCEPTION_MESSAGE, typeof(ExceptionMessage) },

			{ BacktoryMatchMessage.CHALLENGE_JOINED, typeof(MatchJoinedMessage) },
			{ BacktoryMatchMessage.CHALLENGE_STARTED, typeof(MatchStartedMessage) },
			{ BacktoryMatchMessage.CHALLENGE_ENDED, typeof(MatchEndedMessage) },
			{ BacktoryMatchMessage.CHALLENGE, typeof(MatchEvent) },
			{ BacktoryMatchMessage.CHALLENGE_DISCONNECT, typeof(MatchDisconnectMessage) },

			{ BacktoryMatchMessage.CHAT, typeof(MatchChatMessage) },
			{ BacktoryMatchMessage.DIRECT, typeof(DirectChatMessage) },
			{ BacktoryMatchMessage.MASTER, typeof(MasterMessage) },

			{ BacktoryMatchMessage.JOINED_WEBHOOK, typeof(JoinedWebhookMessage) },
			{ BacktoryMatchMessage.STARTED_WEBHOOK, typeof(StartedWebhookMessage) },
			{ BacktoryMatchMessage.WEBHOOK_ERROR, typeof(WebhookErrorMessage) }
		};
		
		private static readonly Dictionary<string, TypeCategory> TYPE_TYPE = new Dictionary<string, TypeCategory> {
			{ BacktoryMessage.EXCEPTION_MESSAGE, TypeCategory.General },

			{ BacktoryMatchMessage.CHALLENGE_JOINED, TypeCategory.Challenge },
			{ BacktoryMatchMessage.CHALLENGE_STARTED, TypeCategory.Challenge },
			{ BacktoryMatchMessage.CHALLENGE_ENDED, TypeCategory.Challenge },
			{ BacktoryMatchMessage.CHALLENGE, TypeCategory.Challenge },
			{ BacktoryMatchMessage.CHALLENGE_DISCONNECT, TypeCategory.Challenge },
			
			{ BacktoryMatchMessage.CHAT, TypeCategory.Challenge },
			{ BacktoryMatchMessage.DIRECT, TypeCategory.Challenge },
			{ BacktoryMatchMessage.MASTER, TypeCategory.Challenge },
			
			{ BacktoryMatchMessage.JOINED_WEBHOOK, TypeCategory.Challenge },
			{ BacktoryMatchMessage.STARTED_WEBHOOK, TypeCategory.Challenge },
			{ BacktoryMatchMessage.WEBHOOK_ERROR, TypeCategory.Challenge }
		};
		
		public static Type GetType(String _type) {
			Type returnVal;
			TYPE_CLASS.TryGetValue(_type, out returnVal);
			return returnVal;
		}
		
		public static TypeCategory GetTypeCategory(String _type) {
			TypeCategory returnVal;
			TYPE_TYPE.TryGetValue(_type, out returnVal);
			return returnVal;
		}
		
		public enum TypeCategory {
			General,
			Challenge
		}
	}
}
