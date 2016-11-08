using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Core
{
	public interface WebSocketListener
	{
		void OnConnect ();
		
		void OnMessage (String message);
		
		void OnDisconnect ();
		
		void OnError (Exception exceptino);
	}
}
