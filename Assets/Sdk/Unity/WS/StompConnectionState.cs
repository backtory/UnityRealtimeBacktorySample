using UnityEngine;
using System.Collections;

namespace Sdk.Unity.WS
{
	public enum StompConnectionState
	{	
		// WS is not connected (connection request is not received or WS disconnected)
		NotConnected,
		
		// Connect request received and WS connect request sent
		Opening,
		
		//  WS is opened, stomp connect is sent and waiting for response
		Connecting,
		
		// Stomp connected is received
		Connected,
		
		// Disconnect request received and stomp disconnect is sent
		Disconnecting,
		
		// Disconnect response received and WS close sent
		Closing
	}
}
