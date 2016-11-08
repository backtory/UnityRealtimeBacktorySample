using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Backtory.core;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class UserChatHistoryResponse {
		public List<ChatMessage> ChatMessageList { get; set; }

		public static UserChatHistoryResponse Get(string responseJson) {
			A a = Backtory.FromJson<A> (responseJson);

			UserChatHistoryResponse response = new UserChatHistoryResponse();
			List<ChatMessage> messageList = new List<ChatMessage>();
			for(int i = 0;i<a.MessageList.Count;i++) {
				ComprehensiveChatMessage cCM = a.MessageList [i];
				string cCMJson = Backtory.ToJson (cCM);
				if (cCM._type == ".SimpleChatMessage") {
					SimpleChatMessage sCM = Backtory.FromJson<SimpleChatMessage> (cCMJson);
					messageList.Add (sCM);
				} else if (cCM._type == ".ChatGroupUserAddedMessage") {
					UserAddedMessage sCM = Backtory.FromJson<UserAddedMessage> (cCMJson);
					messageList.Add (sCM);
				} else if (cCM._type == ".ChatGroupUserJoinedMessage") {
					UserJoinedMessage sCM = Backtory.FromJson<UserJoinedMessage> (cCMJson);
					messageList.Add (sCM);
				} else if (cCM._type == ".ChatGroupUserLeftMessage") {
					UserLeftMessage sCM = Backtory.FromJson<UserLeftMessage> (cCMJson);
					messageList.Add (sCM);
				} else if (cCM._type == ".ChatGroupUserRemovedMessage") {
					UserRemovedMessage sCM = Backtory.FromJson<UserRemovedMessage> (cCMJson);
					messageList.Add (sCM);
				} else if (cCM._type == ".ChatGroupInvitationMessage") {
					ChatInvitationMessage sCM = Backtory.FromJson<ChatInvitationMessage> (cCMJson);
					messageList.Add (sCM);
				} else {
					// ERROR
				}
			}
			response.ChatMessageList = messageList;
			return response;
		}

		internal class A {
			public List<ComprehensiveChatMessage> MessageList { get; set; }
		}
	}
}
