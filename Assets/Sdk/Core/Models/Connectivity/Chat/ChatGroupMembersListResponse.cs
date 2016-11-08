using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class ChatGroupMembersListResponse {
		public string GroupId { get; set; }
		public List<ChatGroupMember> GroupMemberList { get; set; }
	}
}
