using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;
using Sdk.Unity;
using System;
using Sdk.Core.Models.Connectivity.Chat;
using UnityEngine.UI;
using System.Collections.Generic;
using Sdk.Added;
using Assets.Backtory.core;

namespace Tester {
	public class ChatScript : MonoBehaviour, ChatListener {

		BacktoryRealtimeUnityApi backtoryApi;
		public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();
		long lastGroupDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		long lastDirectDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		public Button btnCreatePri;
		public Button btnCreatePub;
		public Button btnListGroups;
		public Button btnListMembers;
		public Button btnAddMember;
		public Button btnRemoveMember;
		public Button btnAddOwner;
		public Button btnGroupChat;
		public Button btnDirectChat;
		public Button btnJoin;
		public Button btnLeave;
		public Button btnInvite;
		public Button btnOffline;
		public Button btnGroupHist;
		public Button btnDirectHist;
		public Button btnReturn;

		public Text txt;
		public InputField ifUserId;
		public InputField ifGroupId;
		public InputField ifChatMessage;

		// Use this for initialization
		void Start () {
			backtoryApi = BacktoryRealtimeUnityApi.Instance();
			backtoryApi.SetChatListener(this);

			addClickListeners ();
//			btnOffline.onClick.Invoke ();
		}

		void Update () {
			while (ExecuteOnMainThread.Count > 0)
			{
				ExecuteOnMainThread.Dequeue().Invoke();
			}
		}
		
		private void addClickListeners() {
			btnCreatePri.onClick.AddListener (() => {
				createChatGroup(ChatGroupType.Private);
			});
			btnCreatePub.onClick.AddListener (() => {
				createChatGroup(ChatGroupType.Public);
			});
			btnListGroups.onClick.AddListener (() => {
				BacktoryResponse<ChatGroupsListResponse> response = backtoryApi.RequestListOfChatGroups();
				if (response.Successful) {
					writeLine("--+group list returned with json: " + Backtory.ToJson(response.Body, true));
				} else {
					writeLine("--+group list failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnListMembers.onClick.AddListener (() => {
				BacktoryResponse<ChatGroupMembersListResponse> response = backtoryApi.RequestListOfChatGroupMembers(getGroupId());
				if (response.Successful) {
					writeLine("--+group member list returned with json: " + Backtory.ToJson(response.Body, true));
				} else {
					writeLine("--+group member list failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnAddMember.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.AddMemberToChatGroup(getGroupId(), getUserId());
				if (response.Successful) {
					writeLine("--+member added");
				} else {
					writeLine("--+member adding failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnRemoveMember.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.RemoveMemberFromChatGroup(getGroupId(), getUserId());
				if (response.Successful) {
					writeLine("--+member removed");
				} else {
					writeLine("--+member removing failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnAddOwner.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.AddOwnerToChatGroup(getGroupId(), getUserId());
				if (response.Successful) {
					writeLine("--+owner added");
				} else {
					writeLine("--+owner adding failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnGroupChat.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.SendChatToGroup(getGroupId(), getMessage());
				if (response.Successful) {
					writeLine("--+group chat sent");
				} else {
					writeLine("--+group chat sending failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnDirectChat.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.SendChatToUser(getUserId(), getMessage());
				if (response.Successful) {
					writeLine("--+direct chat sent");
				} else {
					writeLine("--+direct chat sending failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnJoin.onClick.AddListener (() => {
				string groupId = getGroupId();
				BacktoryResponse<BacktoryVoid> response = backtoryApi.JoinChatGroup(groupId);
				if (response.Successful) {
					writeLine("--+you joined to group with id " + groupId);
				} else {
					writeLine("--+group creation failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnLeave.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.LeaveChatGroup(getGroupId());
				if (response.Successful) {
					writeLine("--+you left chat group");
				} else {
					writeLine("--+group leaving failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnInvite.onClick.AddListener (() => {
				BacktoryResponse<BacktoryVoid> response = backtoryApi.InviteUserToChatGroup(getGroupId(), getUserId());
				if (response.Successful) {
					writeLine("--+invitation sent");
				} else {
					writeLine("--+invitation sending failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnOffline.onClick.AddListener (() => {
				BacktoryResponse<OfflineMessageResponse> response = backtoryApi.RequestOfflineMessages();
				if (response.Successful) {
					writeLine("--+offline list: ");
					List<ChatMessage> list = response.Body.ChatMessageList;
					for (int i = 0; i < list.Count ; i++)
						list[i].OnMessageReceived(this);
				} else {
					writeLine("--+offline request failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnGroupHist.onClick.AddListener (() => {
				BacktoryResponse<GroupChatHistoryResponse> response = backtoryApi.RequestGroupChatHistory(getGroupId(), lastGroupDate);
				if (response.Successful) {
					writeLine("--+group history");
					List<ChatMessage> list = response.Body.ChatMessageList;
					if (list.Count > 0) {
						lastGroupDate = list[list.Count - 1].Date;
					} else {
						lastGroupDate = 0;
					}
					for (int i = 0; i < list.Count ; i++)
						list[i].OnMessageReceived(this);
				} else {
					writeLine("--+group history request failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnDirectHist.onClick.AddListener (() => {
				BacktoryResponse<UserChatHistoryResponse> response = backtoryApi.RequestUserChatHistory(lastDirectDate);
				if (response.Successful) {
					writeLine("--+direct history");
					List<ChatMessage> list = response.Body.ChatMessageList;
					if (list.Count > 0) {
						lastDirectDate = list[list.Count - 1].Date;
					} else {
						lastDirectDate = 0;
					}
					for (int i = 0; i < list.Count; i++)
						list[i].OnMessageReceived(this);
				} else {
					writeLine("--+direct history request failed with code: " + response.Code + " and message " + response.Message);
				}
			});
			btnReturn.onClick.AddListener (() => {
				int previousLevel = PlayerPrefs.GetInt( "MainLevel" );
				Application.LoadLevel( previousLevel );
			});
		}

		private void createChatGroup(ChatGroupType type) {
			BacktoryResponse<ChatGroupCreationResponse> response = backtoryApi.CreateChatGroup(getMessage(), type);
			if (response.Successful) {
				writeLine("--+group created. id: " + response.Body.GroupId);
			} else {
				writeLine("--+group creation failed with code: " + response.Code + " and message " + response.Message);
			}
		}

		public void OnPushMessage(SimpleChatMessage message) {
			writeLine ("--+push message " + message.Message + " from null sender: " + message.SenderId + " to " + message.ReceiverId + " at " + message.Date + " and with null groupId: " + message.GroupId);
		}
		
		public void OnChatMessage(SimpleChatMessage chatMessage) {
			writeLine ("--+chat message " + chatMessage.Message + " from " + chatMessage.SenderId + " to " + chatMessage.ReceiverId + " at " + chatMessage.Date + " and with null groupId " + chatMessage.GroupId);
		}
		
		public void OnGroupPushMessage(SimpleChatMessage message) {
			writeLine ("--+group push message " + message.Message + " from null sender: " + message.SenderId + " to " + message.ReceiverId + " at " + message.Date + " and to group " + message.GroupId);
		}
		
		public void OnGroupChatMessage(SimpleChatMessage groupChatMessage) {
			writeLine ("--+group chat message " + groupChatMessage.Message + " from " + groupChatMessage.SenderId + " to " + groupChatMessage.GroupId + " at " + groupChatMessage.Date + " with ReceiverId " + groupChatMessage.ReceiverId);
		}

		public void OnChatInvitationMessage(ChatInvitationMessage message) {
			writeLine ("--+invitation from " + message.CallerId + " to group " + message.GroupId + " : " + message.GroupName + " at " + message.Date);
		}
		
		public void OnChatGroupUserAddedMessage(UserAddedMessage userAddedMessage) {
			writeLine ("--+user with id " + userAddedMessage.AddedUserId + " added to chat group " + userAddedMessage.GroupId + " by " + userAddedMessage.AdderUserId + " at " + userAddedMessage.Date);
		}
		
		public void OnChatGroupUserJoinedMessage(UserJoinedMessage message) {
			writeLine ("--+user joined with id " + message.UserId + " to " + message.GroupId + " at " + message.Date);
		}
		
		public void OnChatGroupUserLeftMessage(UserLeftMessage message) {
			writeLine ("--+user left with id " + message.UserId + " from " + message.GroupId + " at " + message.Date);
		}
		
		public void OnChatGroupUserRemovedMessage(UserRemovedMessage userRemovedMessage) {
			writeLine ("--+user with id " + userRemovedMessage.RemovedUserId + " removed from chat group " + userRemovedMessage.GroupId + " by " + userRemovedMessage.RemoverUserId + " at " + userRemovedMessage.Date);
		}

		private string getUserId() {
			string text = ifUserId.text;
//			if (text.Equals("ali")) {
//				return "5720b016e4b0bf11f90cdee6";
//			} else if (text.Equals("javad")) {
//				return "5720b023e4b0bf11f90cdee9";
//			} else if (text.Equals("farib")) {
//				return "5720b01ee4b0bf11f90cdee8";
//			} else if (text.Equals("mamad")) {
//				return "5720b01be4b0bf11f90cdee7";
//			} else if (text.Equals("user1")) {
//				return "57246401e4b000957b56e4c6";
//			}
			return text;
		}
		
		private string getGroupId() {
			string text = ifGroupId.text;
//			if (text.Equals("Friends")) {
//				return "57a46b855b66588bdc34a874";
//			} else if (text.Equals("FaribPrivate")) {
//				return "57a46b475b66588bdc34a872";
//			} else if (text.Equals("JavadPublic")) {
//				return "57a46b6d5b66588bdc34a873";
//			}
			return text;
		}
		
		private string getMessage() {
			return ifChatMessage.text;
		}
		
		List<string> consoleLines = new List<string>();
		private void writeLine (string line) {
			Debug.Log (line);
			consoleLines.Add (line);
			if (consoleLines.Count == 31)
				consoleLines.RemoveAt (0);
			
			
			txt.text = "";
			for (int i = 0; i<consoleLines.Count; i++) {
				txt.text += consoleLines[i] + "\n";
			}
		}
	}
}
