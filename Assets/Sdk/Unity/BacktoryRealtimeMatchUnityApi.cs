using System;
using Assets.Backtory.core;
using Sdk.Core.Models;
using System.Collections.Generic;

namespace Sdk.Unity
{
	public class BacktoryRealtimeMatchUnityApi : BacktoryMatchUnityApiWrapper
	{
		internal static String X_BACKTORY_CONNECTIVITY_ID;

		public BacktoryRealtimeMatchUnityApi (string address, string matchId, string connectivityId)
		{
			X_BACKTORY_CONNECTIVITY_ID = connectivityId;
			backtoryMatchApi = new InnerBacktoryMatchUnityApi (address, matchId);
		}

		public BacktoryResponse<ConnectResponse> ConnectAndJoin() 
		{
			return backtoryMatchApi.Connect ();
		}

		public void SendEvent(string message, Dictionary<string, string> data) 
		{
			backtoryMatchApi.SendEvent(message, data);
		}

		internal class InnerBacktoryMatchUnityApi : BacktoryMatchUnityApi {
			internal InnerBacktoryMatchUnityApi(String address, String matchId) : base(address, matchId) {
			}

			override
			internal String GetConnectivityId() {
				return X_BACKTORY_CONNECTIVITY_ID;
			}
		}

	}
}
