using UnityEngine;
using System.Collections;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class ChatGroupMember {
		public string UserId;
		public MemberRole Role;

		public enum MemberRole {
			Owner,
			Member
		}
	
	}
}
