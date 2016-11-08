using UnityEngine;
using System.Collections;

namespace Sdk.Core.Models.Connectivity {
	public class ChallengedUserInfo {
		public string UserId { get; set; }
		public ChallengeUserState State { get; set; }
	}
}
