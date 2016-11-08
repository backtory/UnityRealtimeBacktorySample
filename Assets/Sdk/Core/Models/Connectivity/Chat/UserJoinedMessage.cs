using UnityEngine;
using System.Collections;
using System;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class UserJoinedMessage : ChatMessage {
		public String UserId { get; set; }
		public String GroupId { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChatListener chatListener = (ChatListener) backtoryListener;
			chatListener.OnChatGroupUserJoinedMessage(this);
		}
	}
}
