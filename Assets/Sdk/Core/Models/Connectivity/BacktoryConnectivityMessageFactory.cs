using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sdk.Core.Models.Exception;
using Sdk.Core;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Models.Connectivity.Challenge;
using Sdk.Core.Models.Connectivity.Chat;

namespace Sdk.Core.Models.Connectivity {
	public class BacktoryConnectivityMessageFactory {

		private static readonly Dictionary<string, Type> TYPE_CLASS = new Dictionary<string, Type> {
			{ BacktoryMessage.EXCEPTION_MESSAGE, typeof(ExceptionMessage) },
			
			{ BacktoryConnectivityMessage.MATCH_FOUND_MESSAGE, typeof(MatchFoundMessage) },
			{ BacktoryConnectivityMessage.MATCH_NOT_FOUND_MESSAGE, typeof(MatchNotFoundMessage) },
			{ BacktoryConnectivityMessage.MATCH_UPDATE_MESSAGE, typeof(MatchUpdateMessage) },
			
			{ BacktoryConnectivityMessage.CHALLENGE_INVITATION, typeof(ChallengeInvitationMessage) },
			{ BacktoryConnectivityMessage.CHALLENGE_ACCEPTED_MESSAGE, typeof(ChallengeAcceptedMessage) },
			{ BacktoryConnectivityMessage.CHALLENGE_READY_MESSAGE, typeof(ChallengeReadyMessage) },
			{ BacktoryConnectivityMessage.CHALLENGE_READY_WITHOUT_YOU, typeof(ChallengeReadyWithoutYou) },
			{ BacktoryConnectivityMessage.CHALLENGE_EXPIRED_MESSAGE, typeof(ChallengeExpiredMessage) },
			{ BacktoryConnectivityMessage.CHALLENGE_IMPOSSIBLE_MESSAGE, typeof(ChallengeImpossibleMessage) },
			{ BacktoryConnectivityMessage.CHALLENGE_DECLINED_MESSAGE, typeof(ChallengeDeclinedMessage) },
			
			{ BacktoryConnectivityMessage.SIMPLE_CHAT_MESSAGE, typeof(SimpleChatMessage) },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_ADDED_MESSAGE, typeof(UserAddedMessage) },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_JOINED_MESSAGE, typeof(UserJoinedMessage) },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_LEFT_MESSAGE, typeof(UserLeftMessage) },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_REMOVED_MESSAGE, typeof(UserRemovedMessage) },
			{ BacktoryConnectivityMessage.CHAT_GROUP_INVITATION_MESSAGE, typeof(ChatInvitationMessage) }
		};
		
		private static readonly Dictionary<string, TypeCategory> TYPE_TYPE = new Dictionary<string, TypeCategory> {
			{ BacktoryMessage.EXCEPTION_MESSAGE, TypeCategory.General },
			
			{ BacktoryConnectivityMessage.MATCH_FOUND_MESSAGE, TypeCategory.Matchmaking },
			{ BacktoryConnectivityMessage.MATCH_NOT_FOUND_MESSAGE, TypeCategory.Matchmaking },
			{ BacktoryConnectivityMessage.MATCH_UPDATE_MESSAGE, TypeCategory.Matchmaking },
			
			{ BacktoryConnectivityMessage.CHALLENGE_INVITATION, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_ACCEPTED_MESSAGE, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_READY_MESSAGE, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_READY_WITHOUT_YOU, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_EXPIRED_MESSAGE, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_IMPOSSIBLE_MESSAGE, TypeCategory.Challenge },
			{ BacktoryConnectivityMessage.CHALLENGE_DECLINED_MESSAGE, TypeCategory.Challenge },
			
			{ BacktoryConnectivityMessage.SIMPLE_CHAT_MESSAGE, TypeCategory.Chat },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_ADDED_MESSAGE, TypeCategory.Chat },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_JOINED_MESSAGE, TypeCategory.Chat },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_LEFT_MESSAGE, TypeCategory.Chat },
			{ BacktoryConnectivityMessage.CHAT_GROUP_USER_REMOVED_MESSAGE, TypeCategory.Chat },
			{ BacktoryConnectivityMessage.CHAT_GROUP_INVITATION_MESSAGE, TypeCategory.Chat }
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
			Chat,
			Matchmaking,
			Challenge
		}
	}
}
