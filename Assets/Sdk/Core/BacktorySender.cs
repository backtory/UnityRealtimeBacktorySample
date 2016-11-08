using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

namespace Sdk.Core
{
	public class BacktorySender
	{
		internal String destination;
		internal Dictionary<String, object> data;
		internal ConnectorClient connectorClient;
		internal Completer completer;
		internal Dictionary<String, String> extraHeader;
		
		public BacktorySender (ConnectorClient connectorClient,
		                      String destination,
		                      Dictionary<String, String> extraHeader,
		                      Dictionary<String, object> data,
		                      Completer completer)
		{
			this.connectorClient = connectorClient;
			this.destination = destination;
			this.extraHeader = extraHeader;
			this.data = data;
			this.completer = completer;
		}
		
		public String Send ()
		{
			String response = connectorClient.Send (destination, data, extraHeader);
			CompleteResponse (response);
			return response;
		}
		
		public void SendFast ()
		{
			connectorClient.SendFast (destination, data, extraHeader);
		}
		
		private void CompleteResponse (JSONNode response)
		{
			if (completer != null) {
				completer.Execute (response);
			}
		}
	}
}
