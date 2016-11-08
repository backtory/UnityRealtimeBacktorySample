using UnityEngine;
using System.Collections;
using Sdk.Core;
using System;

namespace Sdk.Unity {
	public abstract class BacktoryMatchUnityApi : BacktoryMatchMethods {

		internal BacktoryMatchUnityApi(String address, String matchId) {
			this.Address = address;
			this.MatchId = matchId;
		}

		override
		internal String GetServiceUrl() {
			//        return "wss://" + address + "ws";
			return "ws://" + Address + "ws";
		}
	}
}
