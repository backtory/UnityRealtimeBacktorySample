using System.Collections;
using Sdk.Core.Listeners;
using System.Collections.Generic;
using System;

namespace Sdk.Core.Models.Realtime.Chat {
	public class MasterMessage : BacktoryMatchMessage {
		public String Message { get; set; }
		public Dictionary<String, String> Data { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			MatchListener matchListener = (MatchListener) backtoryListener;
			matchListener.OnMasterMessage(this);
		}

	}
}
