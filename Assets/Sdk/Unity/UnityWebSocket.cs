using UnityEngine;
using System.Collections;
using Sdk.Core;
using System;
using Sdk.Unity.WS;
using System.Collections.Generic;
using Assets.Backtory.core;

namespace Sdk.Unity {
	public abstract class UnityWebSocket : BacktoryWebSocketAbstractionLayer {
		// TODO: 7/19/16 AD IMPORTANT: Remove
		public static String token;
		
		private BacktoryStompWebSocket webSocketClient;

		public abstract Dictionary<String, String> getExtraHeaders();

		public UnityWebSocket(String url, WebSocketListener webSocketListener, String xBacktoryInstanceId) : base(url, webSocketListener, xBacktoryInstanceId) {
		}
		
		override
		public void Connect(String matchId) {
			
//			URI uri;
//			try {
//				uri = new URI(url);
//			} catch (URISyntaxException e) {
//				e.printStackTrace();
//				// TODO: 7/13/16 AD What to do?
//				return;
//			}
			Dictionary<String, String> extraHeaders = new Dictionary<String, String>();
			// TODO: uncomment
//			extraHeaders.Add("user-agent", System.getProperty("http.agent"));
			// TODO: 7/13/16 AD WebSocket should gather token in true manner
			//        extraHeaders.put("Authorization", "Bearer " + BacktoryAuth.getAccessToken());
			extraHeaders.Add("Authorization-Bearer", BacktoryUser.GetAccessToken());
			extraHeaders.Add("X-Backtory-Connectivity-Id", X_BACKTORY_CONNECTIVITY_ID);
			if (matchId != null) {
				extraHeaders.Add("X-Backtory-Realtime-Challenge-Id", matchId);
			}

			InnerStompWebSocketEventHandler innerEventHandler = new InnerStompWebSocketEventHandler (this);
			webSocketClient = new InnerBacktoryStompWebSocket (this, url, extraHeaders, innerEventHandler);

			webSocketClient.Connect();
		}

		override
		public void Disconnect() {
			webSocketClient.Disconnect();
		}
		
		override
		public void Send(String destination, String body, Dictionary<String, String> extraHeader) {
			Dictionary<String, String> xBacktoryHeader = new Dictionary<String, String>();
			xBacktoryHeader.Add("X-Backtory-Connectivity-Id", X_BACKTORY_CONNECTIVITY_ID);
			if (extraHeader != null) {
				foreach (KeyValuePair<String, String> header in extraHeader) {
					if (header.Key != null && header.Value != null)
						xBacktoryHeader.Add(header.Key, header.Value);
				}
			}
			webSocketClient.send(destination, xBacktoryHeader, body);
		}
		
		override
		public bool IsAlive() {
			// TODO correct it
//			return webSocketClient.IsAlive();
			return true;
		}
		
		internal class InnerBacktoryStompWebSocket : BacktoryStompWebSocket {

			private UnityWebSocket outClass;

			internal InnerBacktoryStompWebSocket(UnityWebSocket unityWebSocket, 
			                                     String url, 
			                                     Dictionary<String, 
			                                     String> extraHeaders, 
			                                     StompWebSocketEventHandler eventHandlerl) : base(url, extraHeaders, eventHandlerl) {
				outClass = unityWebSocket;
			}

			override
			public Dictionary<String, String> getExtraHeaders() {
				return outClass.getExtraHeaders();
			}

		}

		internal class InnerStompWebSocketEventHandler : StompWebSocketEventHandler {

			UnityWebSocket outClass;

			internal InnerStompWebSocketEventHandler(UnityWebSocket unityWebSocket) {
				outClass = unityWebSocket;
			}

			public void OnMessage(String message) {
				outClass.webSocketListener.OnMessage(message);
			}

			public void OnConnected(String username, String userId) {
				outClass.setUserInformation(username, userId);
				outClass.webSocketListener.OnConnect();
			}

			public void OnDisconnected() {
				outClass.webSocketListener.OnDisconnect ();
			}

			public void OnError(Exception exception) {
				outClass.webSocketListener.OnError(exception);
			}

		}

	}
}
