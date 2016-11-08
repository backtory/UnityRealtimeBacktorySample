using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Sdk.Core {
	public abstract class WebSocketAbstractionLayer {
		protected WebSocketListener webSocketListener;
		protected String url;
		
		public WebSocketAbstractionLayer(String url, WebSocketListener webSocketListener) {
			this.url = url;
			this.webSocketListener = webSocketListener;
		}
		
		public abstract void Connect(String matchId);
		
		public abstract void Disconnect();
		
		public abstract void Send(String destination, String body, Dictionary<String, String> extraHeader);
		
		public abstract bool IsAlive();
	}
}
