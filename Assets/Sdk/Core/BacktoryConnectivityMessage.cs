using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Core {
	public abstract class BacktoryConnectivityMessage : BacktoryMessage {

		// ToDo: un-comment
//		protected JsonElement jsonElement;
		
		public static string MATCH_FOUND_MESSAGE = ".MatchFoundMessage";
		public static string MATCH_UPDATE_MESSAGE = ".MatchUpdateMessage";
		public static string MATCH_NOT_FOUND_MESSAGE = ".MatchNotFoundMessage";
		
		public static string CHALLENGE_READY_MESSAGE = ".ChallengeReadyMessage";
		public static string CHALLENGE_INVITATION = ".ChallengeInvitationMessage";
		public static string CHALLENGE_EXPIRED_MESSAGE = ".ChallengeExpiredMessage";
		public static string CHALLENGE_ACCEPTED_MESSAGE = ".ChallengeAcceptedMessage";
		public static string CHALLENGE_DECLINED_MESSAGE = ".ChallengeDeclinedMessage";
		public static string CHALLENGE_READY_WITHOUT_YOU = ".ChallengeReadyWithoutYou";
		public static string CHALLENGE_IMPOSSIBLE_MESSAGE = ".ChallengeImpossibleMessage";
		
		public static string SIMPLE_CHAT_MESSAGE = ".SimpleChatMessage";
		public static string CHAT_GROUP_USER_ADDED_MESSAGE = ".ChatGroupUserAddedMessage";
		public static string CHAT_GROUP_USER_JOINED_MESSAGE = ".ChatGroupUserJoinedMessage";
		public static string CHAT_GROUP_USER_LEFT_MESSAGE = ".ChatGroupUserLeftMessage";
		public static string CHAT_GROUP_USER_REMOVED_MESSAGE = ".ChatGroupUserRemovedMessage";
		public static string CHAT_GROUP_INVITATION_MESSAGE = ".ChatGroupInvitationMessage";

	}
}
