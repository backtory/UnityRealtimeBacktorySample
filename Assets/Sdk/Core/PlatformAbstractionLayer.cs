using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Sdk.Core
{
	public interface PlatformAbstractionLayer
	{
		BacktorySender GetSender (ConnectorClient connectorClient, String destination, Dictionary<String, object> data);
		
		BacktorySender GetSender (ConnectorClient connectorClient, String destination, Dictionary<String, object> data, Completer completer);
		
		BacktorySender GetSender (ConnectorClient connectorClient, String destination, Dictionary<String, String> extraHeader, Dictionary<String, object> data);
		
		BacktorySender GetSender (ConnectorClient connectorClient, String destination, Dictionary<String, String> extraHeader, Dictionary<String, object> data, Completer completer);
		
		void LogError (String message);

		bool IsNetworkAvailable ();
	}
}
