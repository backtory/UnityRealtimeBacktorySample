using UnityEngine;
using System.Collections;
using Sdk.Core;
using System;
using System.Collections.Generic;

namespace Sdk.Unity {
	public class UnityPlatform : PlatformAbstractionLayer {

		private const String TAG = "BacktorySdk->";

		public BacktorySender GetSender(ConnectorClient connectorClient, String destination, Dictionary<String, object> data) {
			return new BacktorySender(connectorClient, destination, null, data, null);
		}

		public BacktorySender GetSender(ConnectorClient connectorClient, String destination, Dictionary<String, object> data, Completer completer) {
			return new BacktorySender(connectorClient, destination, null, data, completer);
		}
		
		public BacktorySender GetSender(ConnectorClient connectorClient, String destination, Dictionary<String, String> extraHeader, Dictionary<String, object> data) {
			return new BacktorySender(connectorClient, destination, extraHeader, data, null);
		}
		
		public BacktorySender GetSender(ConnectorClient connectorClient, String destination, Dictionary<String, String> extraHeader, Dictionary<String, object> data, Completer completer) {
			return new BacktorySender(connectorClient, destination, extraHeader, data, completer);
		}

		public void LogError(String message) {
			Debug.Log (TAG + message);
		}

		public bool IsNetworkAvailable() {
			// TODO implement base on unity ability
			return true;
		}
	}
}
