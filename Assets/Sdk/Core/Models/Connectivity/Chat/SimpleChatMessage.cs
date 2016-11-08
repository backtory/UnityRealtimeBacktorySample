using System;
using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class SimpleChatMessage : ChatMessage {
		public String GroupId { get; set; }
		public String SenderId { get; set; }
		public String ReceiverId { get; set; }
		public String Message { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChatListener chatListener = (ChatListener) backtoryListener;
			
			if (SenderId == null && GroupId == null) {
				chatListener.OnPushMessage(this);
			} else if (SenderId != null && GroupId == null) {
				chatListener.OnChatMessage(this);
			} else if (SenderId == null && GroupId != null) {
				chatListener.OnGroupPushMessage(this);
			} else if (SenderId != null && GroupId != null) {
				chatListener.OnGroupChatMessage(this);
			}
		}
	}
}
