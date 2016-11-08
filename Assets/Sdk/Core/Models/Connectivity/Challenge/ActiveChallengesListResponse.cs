using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sdk.Core.Models.Connectivity.Challenge;

namespace Sdk.Core.Models.Challenge {
	public class ActiveChallengesListResponse {
		public List<PendingChallengeInfo> ActiveChallengeList { get; set; }
	}
}
