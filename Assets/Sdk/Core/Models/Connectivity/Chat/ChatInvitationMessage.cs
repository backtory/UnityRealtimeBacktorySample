using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class ChatInvitationMessage : ChatMessage {
		public string GroupId { get; set; }
		public string CallerId { get; set; }
		public string GroupName { get; set; }
		
		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			ChatListener chatListener = (ChatListener) backtoryListener;
			chatListener.OnChatInvitationMessage(this);
		}
	}
}
