using UnityEngine;
using System.Collections;
using Sdk.Core;

namespace Sdk.Unity {
	public abstract class BacktoryConnectivityUnityApi : BacktoryConnectivityMethods {

		internal BacktoryConnectivityUnityApi(UnityPlatform unityPlatform) {
			connectorClient.setPlatformAbstractionLayer (unityPlatform);
		}
	}
}
