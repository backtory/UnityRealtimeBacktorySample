using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Unity.WS
{
	public interface StompWebSocketEventHandler
	{
	
		void OnMessage (String message);
		
		void OnConnected (String username, String userId);
		
		void OnError (Exception exception);

		void OnDisconnected ();

	}
}
