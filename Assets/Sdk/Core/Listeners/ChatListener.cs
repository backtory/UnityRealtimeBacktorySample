using UnityEngine;
using System.Collections;
using Sdk.Core.Models.Connectivity.Chat;

namespace Sdk.Core.Listeners {
	public interface ChatListener : BacktoryListener {

		void OnPushMessage(SimpleChatMessage pushMessage);
		
		void OnChatMessage(SimpleChatMessage chatMessage);
		
		void OnGroupPushMessage(SimpleChatMessage groupPushMessage);
		
		void OnGroupChatMessage(SimpleChatMessage groupChatMessage);
		
		void OnChatInvitationMessage(ChatInvitationMessage chatInvitationMessage);
		
		void OnChatGroupUserAddedMessage(UserAddedMessage userAddedMessage);
		
		void OnChatGroupUserJoinedMessage(UserJoinedMessage userJoinedMessage);
		
		void OnChatGroupUserLeftMessage(UserLeftMessage userLeftMessage);
		
		void OnChatGroupUserRemovedMessage(UserRemovedMessage userRemovedMessage);
	
	}
}
