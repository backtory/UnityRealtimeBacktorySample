using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Core.Models.Connectivity.Matchmaking {
	public class MatchFoundParticipant {
		private String UserId { get; set; }
		private int? Skill { get; set; }
		private String MetaData { get; set; }
	}
}