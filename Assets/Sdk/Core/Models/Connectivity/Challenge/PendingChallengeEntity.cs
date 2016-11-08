using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sdk.Core.Models.Connectivity.Challenge {
	public class PendingChallengeInfo {

		public string ChallengeId { get; set; }
		public string ChallengerId { get; set; }
		public List<ChallengedUserInfo> ChallengedUsers { get; set; }
		public int MinPlayer { get; set; }
		public long CreationDate { get; set; }
		public long WaitTime { get; set; }
	
	}
}
