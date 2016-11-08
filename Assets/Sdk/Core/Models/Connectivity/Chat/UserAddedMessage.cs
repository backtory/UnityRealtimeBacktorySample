using UnityEngine;
using System.Collections;
using System;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class UserAddedMessage : ChatMessage {
		public String AdderUserId { get; set; }
		public String AddedUserId { get; set; }
		public String GroupId { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChatListener chatListener = (ChatListener) backtoryListener;
			chatListener.OnChatGroupUserAddedMessage(this);
		}
	}
}
