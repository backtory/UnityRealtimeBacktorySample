using System;
using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public class ComprehensiveChatMessage {

		public String _type { get; set; }
		public String GroupId { get; set; }
		public String SenderId { get; set; }
		public String ReceiverId { get; set; }
		public String Message { get; set; }
		public long Date { get; set; }
		public String AdderUserId { get; set; }
		public String AddedUserId { get; set; }
		public String UserId { get; set; }
		public String RemoverUserId { get; set; }
		public String RemovedUserId { get; set; }
		public string CallerId { get; set; }
		public string GroupName { get; set; }

	}
}
