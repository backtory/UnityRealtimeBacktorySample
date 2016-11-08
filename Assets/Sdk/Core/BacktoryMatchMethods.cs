using Assets.Backtory.core;
using Sdk.Added;
using System;
using System.Collections.Generic;

namespace Sdk.Core
{
	public abstract class BacktoryMatchMethods : BacktoryMatchApi
	{

		public BacktoryResponse<BacktoryVoid> DirectToUser (string userId, string message)
		{
			string destination = "/app/direct." + userId;
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("message", message, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> SendChatToMatch (string message)
		{
			string destination = "/app/chat." + MatchId;
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("message", message, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public BacktoryResponse<BacktoryVoid> SendMatchResult (List<string> winners)
		{
			string destination = "/app/challenge.result." + MatchId;
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("winners", winners, ref apiParamData);
			return SendAndReceive<BacktoryVoid> (destination, apiParamData, typeof(BacktoryVoid));
//			BacktorySender sender = GetSender(destination, apiParamData);
//			string responseStr = sender.Send();
//			return GenerateBacktoryResponse<BacktoryVoid>(responseStr, typeof(BacktoryVoid));
		}
		
		public void SendEvent (string message, Dictionary<string, string> data)
		{
			string destination = "/app/challenge." + MatchId;
			Dictionary<string, object> apiParamData = new Dictionary<string, object> ();
			setApiParamData ("message", message, ref apiParamData);
			setApiParamData ("data", data, ref apiParamData);
			BacktorySender sender = GetSender (destination, apiParamData);
			sender.SendFast ();
		}

	}
}
