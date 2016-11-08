using UnityEngine;
using System.Collections;
using System;

namespace Sdk.Core
{
	public class ConnectorStateEngine
	{

		private static String TAG = "BacktorySdk->" + typeof(ConnectorStateEngine).Name + " ";
		private ConnectorState connectionState = ConnectorState.STOPPED;
		private ConnectorClient connectorClient;

		public ConnectorStateEngine (ConnectorClient connectorClient)
		{
			this.connectorClient = connectorClient;
		}

		internal enum ConnectorState
		{
			STOPPED,
			CONNECTING,
			CONNECTED,
			DISCONNECTING
		}
		
		internal enum StateChangeEvent
		{
			CONNECT,
			CONNECTED,
			DISCONNECT,
			DISCONNECTED,
			GENERAL_ERROR
		}

		internal ConnectorState getConnectionState ()
		{
			return connectionState;
		}
		
		internal void ChangeState (StateChangeEvent changeEvent)
		{
			switch (connectionState) {
			case ConnectorState.STOPPED:
				processStoppedEvents (changeEvent);
				break;
					
			case ConnectorState.CONNECTING:
				processConnectingEvents (changeEvent);
				break;
					
			case ConnectorState.CONNECTED:
				processConnectedEvents (changeEvent);
				break;
					
			case ConnectorState.DISCONNECTING:
				processDisconnectingEvents (changeEvent);
				break;
					
			default:
				break;
			}
		}
		
		private void changeStateTo (ConnectorState state)
		{
			connectionState = state;
			Debug.Log (TAG + "New state: " + connectionState);
		}
		
		void processStoppedEvents (StateChangeEvent changeEvent)
		{
			
			switch (changeEvent) {
			case StateChangeEvent.CONNECT:
				changeStateTo (ConnectorState.CONNECTING);
				connectorClient.CreateWSAndConnect (connectorClient.GetServiceUrl (), connectorClient.GetConnectivityId ());
				break;
				
			case StateChangeEvent.CONNECTED:
				throw new Exception ("Invalid connected event, when state is stopped");
				
			case StateChangeEvent.DISCONNECT:
				throw new Exception ("Invalid disconnect event, when state is stopped");
				
			default:
				break;
				
			}
		}
		
		void processConnectingEvents (StateChangeEvent changeEvent)
		{
			
			switch (changeEvent) {
				
			case StateChangeEvent.CONNECT:
				throw new Exception ("Invalid connect event, when state is connecting");
				
			case StateChangeEvent.CONNECTED:
				changeStateTo (ConnectorState.CONNECTED);
				break;
				
			case StateChangeEvent.DISCONNECT:
				throw new Exception ("Invalid disconnect event, when state is connecting");
				
			case StateChangeEvent.GENERAL_ERROR:
				changeStateTo (ConnectorState.STOPPED);
				connectorClient.Disconnect ();
				break;
				
			default:
				break;
				
			}
		}
		
		void processConnectedEvents (StateChangeEvent changeEvent)
		{
			
			switch (changeEvent) {
				
			case StateChangeEvent.CONNECT:
				throw new Exception ("Invalid connect event, when state is connected");
				
			case StateChangeEvent.CONNECTED:
				throw new Exception ("Invalid connected event, when state is connected");
				
			case StateChangeEvent.DISCONNECT:
				connectorClient.SendDisconnect ();
				changeStateTo (ConnectorState.DISCONNECTING);
				break;
				
			case StateChangeEvent.DISCONNECTED:
				changeStateTo (ConnectorState.STOPPED);
				break;
				
			default:
				break;
				
			}
		}
		
		void processDisconnectingEvents (StateChangeEvent changeEvent)
		{
			
			switch (changeEvent) {
				
			case StateChangeEvent.CONNECT:
				throw new Exception ("Invalid connect event, when state is disconnecting");
				
			case StateChangeEvent.CONNECTED:
				throw new Exception ("Invalid connected event, when state is disconnecting");
				
			case StateChangeEvent.DISCONNECT:
				throw new Exception ("Invalid disconnect event, when state is disconnecting");
				
			case StateChangeEvent.DISCONNECTED:
				changeStateTo (ConnectorState.STOPPED);
				break;
				
			case StateChangeEvent.GENERAL_ERROR:
				changeStateTo (ConnectorState.STOPPED);
				break;
				
			default:
				break;
				
			}
		}
	}
}
