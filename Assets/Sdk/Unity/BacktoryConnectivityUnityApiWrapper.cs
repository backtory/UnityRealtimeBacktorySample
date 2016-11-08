using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using System;
using Sdk.Core.Models.Connectivity.Matchmaking;
using System.Collections.Generic;
using Sdk.Core.Models.Connectivity.Challenge;
using Sdk.Core.Models.Challenge;
using Sdk.Core.Models.Connectivity.Chat;
using Sdk.Added;
using Assets.Backtory.core;

namespace Sdk.Unity {
	public class BacktoryConnectivityUnityApiWrapper {

		internal static BacktoryConnectivityUnityApi backtoryConnectivityApi;
		
		public void SetMatchmakingListener(MatchmakingListener matchmakingListener) {
			backtoryConnectivityApi.setMatchmakingListener(matchmakingListener);
		}
		
		public void SetChallengeListener(ChallengeListener challengeListener) {
			backtoryConnectivityApi.setChallengeListener(challengeListener);
		}
		
		public void SetChatListener(ChatListener chatListener) {
			backtoryConnectivityApi.setChatListener(chatListener);
		}
		
		public void SetRealtimeSdkListener(RealtimeSdkListener realtimeSdkListener) {
			backtoryConnectivityApi.SetRealtimeSdkListener(realtimeSdkListener);
		}
		
		public BacktoryResponse<BacktoryVoid> Disconnect() {
			return backtoryConnectivityApi.Disconnect ();
		}

		public BacktoryResponse<MatchmakingResponse> RequestMatchmaking(string matchmakingName, int skill, string metaData) {
			return backtoryConnectivityApi.MatchmakingRequest(matchmakingName, skill, metaData);
		}

		public BacktoryResponse<BacktoryVoid> CancelMatchmaking(string matchmakingName, string requestId) {
			return backtoryConnectivityApi.MatchmakingCancellationRequest(matchmakingName, requestId);
		}
		
		public BacktoryResponse<ChallengeResponse> RequestChallenge(List<string> challengedUsers, int waitTime, int? minPlayer) {
			return backtoryConnectivityApi.ChallengeRequest(challengedUsers, waitTime, minPlayer);
		}
		
		public BacktoryResponse<BacktoryVoid> AcceptChallenge(string challengeId) {
			return backtoryConnectivityApi.AcceptChallenge(challengeId);
		}
		
		public BacktoryResponse<BacktoryVoid> DeclineChallenge(string challengeId) {
			return backtoryConnectivityApi.DeclineChallenge(challengeId);
		}
		
		public BacktoryResponse<ActiveChallengesListResponse> RequestListOfActiveChallenges() {
			return backtoryConnectivityApi.ActiveChallengeListRequest();
		}
		
		public BacktoryResponse<ChatGroupCreationResponse> CreateChatGroup(string groupName,
		                                                                   ChatGroupType groupType) {
			return backtoryConnectivityApi.CreateChatGroup(groupName, groupType);
		}
		
		public BacktoryResponse<ChatGroupsListResponse> RequestListOfChatGroups() {
			return backtoryConnectivityApi.ChatGroupsListRequest();
		}
		
		public BacktoryResponse<ChatGroupMembersListResponse> RequestListOfChatGroupMembers(string groupId) {
			return backtoryConnectivityApi.ChatGroupMembersListRequest(groupId);
		}
		
		public BacktoryResponse<GroupChatHistoryResponse> RequestGroupChatHistory(string groupId, long lastDate) {
			return backtoryConnectivityApi.GroupChatHistoryRequest(groupId, lastDate);
		}

		public BacktoryResponse<UserChatHistoryResponse> RequestUserChatHistory(long lastDate) {
			return backtoryConnectivityApi.UserChatHistoryRequest(lastDate);
		}
		
		public BacktoryResponse<OfflineMessageResponse> RequestOfflineMessages() {
			return backtoryConnectivityApi.OfflineMessagesRequest();
		}

		public BacktoryResponse<BacktoryVoid> AddMemberToChatGroup(string groupId, string userId) {
			return backtoryConnectivityApi.AddMemberToChatGroup(groupId, userId);
		}
		
		public BacktoryResponse<BacktoryVoid> RemoveMemberFromChatGroup(string groupId, string userId) {
			return backtoryConnectivityApi.RemoveMemberFromChatGroup(groupId, userId);
		}
		
		public BacktoryResponse<BacktoryVoid> AddOwnerToChatGroup(string groupId, string userId) {
			return backtoryConnectivityApi.AddOwnerToChatGroup(groupId, userId);
		}

		public BacktoryResponse<BacktoryVoid> JoinChatGroup(string groupId) {
			return backtoryConnectivityApi.JoinChatGroup(groupId);
		}

		public BacktoryResponse<BacktoryVoid> LeaveChatGroup(string groupId) {
			return backtoryConnectivityApi.LeaveChatGroup(groupId);
		}
		
		public BacktoryResponse<BacktoryVoid> InviteUserToChatGroup(string groupId, string userId) {
			return backtoryConnectivityApi.InviteUserToChatGroup(groupId, userId);
		}
		
		public BacktoryResponse<BacktoryVoid> SendChatToUser(string userId, string message) {
			return backtoryConnectivityApi.SendChatToUser(userId, message);
		}
		
		public BacktoryResponse<BacktoryVoid> SendChatToGroup(string groupId, string message) {
			return backtoryConnectivityApi.SendChatToGroup(groupId, message);
		}
		
	}
}
