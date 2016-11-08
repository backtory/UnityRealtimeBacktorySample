using System;
using Sdk.Core.Listeners;
using Assets.Backtory.core;
using Sdk.Added;
using System.Collections.Generic;

namespace Sdk.Unity
{
	public class BacktoryMatchUnityApiWrapper
	{

		internal static BacktoryMatchUnityApi backtoryMatchApi;

		public void SetRealtimeSdkListener(RealtimeSdkListener realtimeSdkListener) {
			backtoryMatchApi.SetRealtimeSdkListener(realtimeSdkListener);
		}

		public void SetMatchListener(MatchListener matchListener) {
			backtoryMatchApi.SetMatchListener(matchListener);
		}

		public BacktoryResponse<BacktoryVoid> DirectToUser(String userId, String message) {
			return backtoryMatchApi.DirectToUser(userId, message);
		}

		public BacktoryResponse<BacktoryVoid> SendChatToMatch(string message) {
			return backtoryMatchApi.SendChatToMatch(message);
		}

		public BacktoryResponse<BacktoryVoid> SendMatchResult(List<string> winners) {
			return backtoryMatchApi.SendMatchResult(winners);
		}

	}
}
