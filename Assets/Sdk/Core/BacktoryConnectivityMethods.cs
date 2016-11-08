using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Sdk.Core.Models.Connectivity.Matchmaking;
using Sdk.Core.Models.Exception;
using Sdk.Core.Models.Connectivity.Chat;
using Sdk.Core.Models.Connectivity.Challenge;
using Sdk.Core.Models.Challenge;
using Sdk.Added;
using SimpleJSON;
using Assets.Backtory.core;

namespace Sdk.Core
{
	public abstract class BacktoryConnectivityMethods : BacktoryConnectivityApi
	{

		public BacktoryResponse<MatchmakingResponse> MatchmakingRequest (string matchmakingName, int skill, string metaData)
		{
			string destination = "/app/MatchmakingRequest";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("matchmakingName", matchmakingName, ref apiParamData);
			setApiParamData ("skill", skill, ref apiParamData);
			setApiParamData ("metaData", metaData, ref apiParamData);
			return SendAndReceive<MatchmakingResponse> (destination, apiParamData, typeof(MatchmakingResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<MatchmakingResponse>(responseStr, typeof(MatchmakingResponse));
		}

		public BacktoryResponse<BacktoryVoid> MatchmakingCancellationRequest (string matchmakingName, string requestId)
		{
			string destination = "/app/MatchmakingCancellation";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("matchmakingName", matchmakingName, ref apiParamData);
			setApiParamData ("requestId", requestId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<ChallengeResponse> ChallengeRequest (List<string> challengedUsers, int waitTime, int? minPlayer)
		{
			string destination = "/app/ChallengeRequest";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("challengedUsers", challengedUsers, ref apiParamData);
			setApiParamData ("minPlayer", minPlayer, ref apiParamData);
			setApiParamData ("waitTime", waitTime, ref apiParamData);
			return SendAndReceive<ChallengeResponse> (destination, apiParamData, typeof(ChallengeResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<ChallengeResponse>(responseStr, typeof(ChallengeResponse));
		}
		
		public BacktoryResponse<BacktoryVoid> AcceptChallenge (string challengeId)
		{
			string destination = "/app/AcceptChallenge";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("challengeId", challengeId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> DeclineChallenge (string challengeId)
		{
			string destination = "/app/DeclineChallenge";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("challengeId", challengeId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<ActiveChallengesListResponse> ActiveChallengeListRequest ()
		{
			string destination = "/app/ActiveChallengesListRequest";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			return SendAndReceive<ActiveChallengesListResponse> (destination, apiParamData, typeof(ActiveChallengesListResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<ActiveChallengesListResponse>(responseStr, typeof(ActiveChallengesListResponse));
		}
		
		public BacktoryResponse<ChatGroupCreationResponse> CreateChatGroup (string groupName, ChatGroupType groupType)
		{
			string destination = "/app/chat/group/create";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("name", groupName, ref apiParamData);
			setApiParamData ("type", groupType, ref apiParamData);
			return SendAndReceive<ChatGroupCreationResponse> (destination, apiParamData, typeof(ChatGroupCreationResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<ChatGroupCreationResponse>(responseStr, typeof(ChatGroupCreationResponse));
		}
		
		public BacktoryResponse<ChatGroupsListResponse> ChatGroupsListRequest ()
		{
			string destination = "/app/chat/group/list";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			return SendAndReceive<ChatGroupsListResponse> (destination, apiParamData, typeof(ChatGroupsListResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<ChatGroupsListResponse>(responseStr, typeof(ChatGroupsListResponse));
		}
		
		public BacktoryResponse<ChatGroupMembersListResponse> ChatGroupMembersListRequest (string groupId)
		{
			string destination = "/app/chat/group/listMembers";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			return SendAndReceive<ChatGroupMembersListResponse> (destination, apiParamData, typeof(ChatGroupMembersListResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<ChatGroupMembersListResponse>(responseStr, typeof(ChatGroupMembersListResponse));
		}
		
		public BacktoryResponse<GroupChatHistoryResponse> GroupChatHistoryRequest (string groupId, long lastDate)
		{
			string destination = "/app/chat/group/history";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("lastDate", lastDate, ref apiParamData);
			return SendAndReceive<GroupChatHistoryResponse> (destination, apiParamData, typeof(GroupChatHistoryResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<GroupChatHistoryResponse>(responseStr, typeof(GroupChatHistoryResponse));
		}
		
		public BacktoryResponse<UserChatHistoryResponse> UserChatHistoryRequest (long lastDate)
		{
			string destination = "/app/chat/history";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("lastDate", lastDate, ref apiParamData);
			return SendAndReceive<UserChatHistoryResponse> (destination, apiParamData, typeof(UserChatHistoryResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<UserChatHistoryResponse>(responseStr, typeof(UserChatHistoryResponse));
		}
		
		public BacktoryResponse<OfflineMessageResponse> OfflineMessagesRequest ()
		{
			string destination = "/app/chat/offline";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			return SendAndReceive<OfflineMessageResponse> (destination, apiParamData, typeof(OfflineMessageResponse));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<OfflineMessageResponse>(responseStr, typeof(OfflineMessageResponse));
		}
		
		public BacktoryResponse<BacktoryVoid> AddMemberToChatGroup (string groupId, string userId)
		{
			string destination = "/app/chat/group/addMember";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("userId", userId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> RemoveMemberFromChatGroup (string groupId, string userId)
		{
			string destination = "/app/chat/group/removeMember";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("userId", userId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> AddOwnerToChatGroup (string groupId, string userId)
		{
			string destination = "/app/chat/group/addOwner";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("userId", userId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> JoinChatGroup (string groupId)
		{
			string destination = "/app/chat/group/join";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> LeaveChatGroup (string groupId)
		{
			string destination = "/app/chat/group/leave";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));		
		}
		
		public BacktoryResponse<BacktoryVoid> InviteUserToChatGroup (string groupId, string userId)
		{
			string destination = "/app/chat/group/inviteUser";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("userId", userId, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> SendChatToUser (string userId, string message)
		{
			string destination = "/app/chat/send";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("userId", userId, ref apiParamData);
			setApiParamData ("message", message, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> SendChatToGroup (string groupId, string message)
		{
			string destination = "/app/chat/group/send";
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("groupId", groupId, ref apiParamData);
			setApiParamData ("message", message, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
	}
}
