using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Core {
	public abstract class BacktoryWebSocketAbstractionLayer : WebSocketAbstractionLayer {
		internal String X_BACKTORY_CONNECTIVITY_ID;
		internal String username;
		internal String userId;
		
		public BacktoryWebSocketAbstractionLayer(String url, WebSocketListener webSocketListener, String xBacktoryConnectivityId) : base(url, webSocketListener) {
			X_BACKTORY_CONNECTIVITY_ID = xBacktoryConnectivityId;
		}
		
		public void setUserInformation(String username, String userId) {
			this.username = username;
			this.userId = userId;
		}
	}
}
