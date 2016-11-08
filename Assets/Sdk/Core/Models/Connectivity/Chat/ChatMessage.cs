using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Connectivity.Chat {
	public abstract class ChatMessage : BacktoryConnectivityMessage {

		public long Date { get; set; }
		
//		override
//		public void OnMessageReceived(BacktoryListener backtoryListener) {
//		}

	}
}
