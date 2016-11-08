using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Timers;
using WebSocketSharp;

namespace Sdk.Unity.WS
{
	internal abstract class BacktoryStompWebSocket : StompWebSocket
	{

		private static String TAG = "BacktorySdk->" + typeof(BacktoryStompWebSocket).Name + " ";
		private double heartbeatPeriod = 40 * 1000; // 40 seconds

		private Timer heartbeatTimer;
		
		internal BacktoryStompWebSocket (String url, Dictionary<String, String> extraHeaders, StompWebSocketEventHandler eventHandler) : base(url, extraHeaders, eventHandler)
		{
		}
		
		override
		internal sealed void OnConnected (StompMessage stompMessage)
		{
			base.OnConnected (stompMessage);
			StartHeartbeatTimer ();
		}

		override
		public sealed void Disconnect ()
		{
			if (!IsAlive)
				return;

			StopHeartbeatTimer ();
			base.Disconnect ();
		}
		
		internal void SetHeartbeatPeriod (double heartbeatPeriod)
		{
			this.heartbeatPeriod = heartbeatPeriod;
		}
		
		internal void StartHeartbeatTimer ()
		{
			if (heartbeatTimer != null) {
				heartbeatTimer.Close ();
			}
			heartbeatTimer = new Timer ();
			heartbeatTimer.Interval = heartbeatPeriod;
			heartbeatTimer.Elapsed += heartbeatAction;
			heartbeatTimer.Start ();
		}
		
		internal void StopHeartbeatTimer ()
		{
			heartbeatTimer.Close ();
		}

		private void heartbeatAction (object sender, EventArgs args)
		{
			if (IsAlive) {
				try {
					sendHeartbeat ();
					Debug.Log (TAG + "heartbeat sent");
				} catch (WebSocketException e) {
					heartbeatTimer.Close ();
					Debug.Log (TAG + "Error sending heartbeat" + e.Data);
				}
			}
		}
	}
}
