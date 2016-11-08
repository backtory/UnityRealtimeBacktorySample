using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System;
using System.Collections.Generic;
using Sdk.Core;
using WebSocketSharp.Net;
using System.Net.Sockets;

namespace Sdk.Unity.WS
{
	internal abstract  class StompWebSocket : WebSocket
	{
		private static String TAG = "BacktorySdk->" + typeof(StompWebSocket).Name + " ";

		public abstract Dictionary<String, String> getExtraHeaders ();

		private StompConnectionState stompConnectionState;
		private StompWebSocketEventHandler eventHandler;
		private const String COMMAND_CONNECT = "CONNECT";
		private const String COMMAND_CONNECTED = "CONNECTED";
		private const String COMMAND_MESSAGE = "MESSAGE";
		private const String COMMAND_RECEIPT = "RECEIPT";
		private const String COMMAND_ERROR = "ERROR";
		private const String COMMAND_DISCONNECT = "DISCONNECT";
		private const String COMMAND_SEND = "SEND";
		private const String SUBSCRIPTION_DESTINATION = "destination";
		private const int maxWebSocketFrameSize = 16 * 1024;

		internal StompWebSocket (String url, Dictionary<String, String> extraHeaders, StompWebSocketEventHandler eventHandler) : base(url)
		{
			this.eventHandler = eventHandler;
			stompConnectionState = StompConnectionState.NotConnected;

			foreach (KeyValuePair<String, String> header in extraHeaders) {
				Debug.Log ("==> " + header.Key + " " + header.Value);
				SetCookie (new Cookie (header.Key, header.Value));
			}

			OnClose += new EventHandler<CloseEventArgs> (onClose);
			OnOpen += new EventHandler (onOpen);
			OnMessage += new EventHandler<MessageEventArgs> (onMessage);
			OnError += new EventHandler<ErrorEventArgs> (onError);
		}

		internal virtual void OnConnected (StompMessage stompMessage)
		{
			String username = stompMessage.getHeaders () ["user-name"];
			String userId = stompMessage.getHeaders () ["user-id"];
			String heartbeat = stompMessage.getHeaders () ["heart-beat"];
			if (heartbeat != null) {
				String[] splittedHeartbeat = heartbeat.Split (',');
				int? heartbeatPeriod = Int32.Parse (splittedHeartbeat [1]);
				if (heartbeatPeriod != null && heartbeatPeriod > 0) {
					((BacktoryStompWebSocket)this).SetHeartbeatPeriod ((double)heartbeatPeriod);
				}
			}

			Debug.Log (TAG + "Web Socket connected to server");
			stompConnectionState = StompConnectionState.Connected;
			eventHandler.OnConnected (username, userId);
		}

		private void onDisconnected ()
		{
			stompConnectionState = StompConnectionState.Closing;
			base.Close ();
		}

		private void transmit (String command, Dictionary<String, String> headers, String body)
		{
			String outMsg = StompMessage.marshall (command, headers, body);
			Debug.Log (TAG + ">>> " + outMsg);
			while (true) {
				if (outMsg.Length > maxWebSocketFrameSize) {
					base.Send (outMsg.Substring (0, maxWebSocketFrameSize));
					outMsg = outMsg.Substring (maxWebSocketFrameSize);
				} else {
					base.Send (outMsg);
					break;
				}
			}
		}

		#pragma warning disable 108
		internal void Connect ()
		{
			if (stompConnectionState != StompConnectionState.NotConnected)
				return;
			
			stompConnectionState = StompConnectionState.Opening;
			Debug.Log (TAG + "Opening Web Socket...");
			try {
				base.Connect ();
			} catch (Exception) {
				stompConnectionState = StompConnectionState.NotConnected;
				// TODO: 8/10/16 AD Inform about fail
			}
		}
		#pragma warning restore 108
		
		public virtual void Disconnect ()
		{
			if (stompConnectionState != StompConnectionState.Connected)
				return;
			
			stompConnectionState = StompConnectionState.Disconnecting;
			transmit (COMMAND_DISCONNECT, null, null);
		}
		
		public void sendHeartbeat ()
		{
			base.Send ("\n");
		}
		
		public void send (String destination, Dictionary<String, String> headers, String body)
		{
			if (stompConnectionState == StompConnectionState.Connected) {
				if (headers == null)
					headers = new Dictionary<String, String> ();
				
				if (body == null)
					body = "";
				
				headers.Add (SUBSCRIPTION_DESTINATION, destination);
				
				transmit (COMMAND_SEND, headers, body);
			} else {
				// TODO: 8/10/16 AD throw appropriate exception
			}
		}
		
		public StompConnectionState getStompConnectionState ()
		{
			return stompConnectionState;
		}

		private void onOpen (object sender, EventArgs e)
		{
			stompConnectionState = StompConnectionState.Connecting;
			transmit (COMMAND_CONNECT, getExtraHeaders (), null);
			Debug.Log (TAG + "...Web Socket Opened");
		}
		
		private void onClose (object sender, EventArgs e)
		{
			stompConnectionState = StompConnectionState.NotConnected;
			Debug.Log (TAG + "Web Socket closed");
			eventHandler.OnDisconnected ();
		}
		
		private void onMessage (object sender, MessageEventArgs e)
		{
			String messageText = e.Data;
			Debug.Log (TAG + "<<< " + messageText);
			
			StompMessage stompMessage = StompMessage.fromString (messageText);
			if (stompMessage == null) {
				return;
			}
			if (stompMessage.getCommand ().Equals (COMMAND_CONNECTED)) {
				OnConnected (stompMessage);
			} else if (stompMessage.getCommand ().Equals (COMMAND_MESSAGE)) {
				eventHandler.OnMessage (stompMessage.getBody ());
			} else if (stompMessage.getCommand ().Equals (COMMAND_RECEIPT)) {
				String receiptId = stompMessage.getHeaders () ["receipt-id"];
				if ("disconnect-ack".Equals (receiptId))
					onDisconnected ();
			} else if (stompMessage.getCommand ().Equals (COMMAND_ERROR)) {
				String errorMessage = stompMessage.getHeaders () ["message"];
				Debug.LogError (TAG + "Stomp error with message: " + errorMessage);
			} else {
				Debug.LogError ("Undefined command " + stompMessage.getCommand ());
			}
		}
		
		private void onError (object sender, ErrorEventArgs e)
		{
//			Exception exception = e.Exception;
//			if (exception is SocketException) {
//				SocketException socketException = (SocketException)exception;
//				int errorCode = socketException.ErrorCode;
//				if (errorCode == 10061) {
//					return;
//				}
//			}
			Debug.LogError (TAG + "WS error with exception message: " + e.Message);
			eventHandler.OnError (e.Exception);
		}

	}
}
