using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core {
	public abstract class BacktoryMessage {

		public static string EXCEPTION_MESSAGE = ".ExceptionMessage";
		
		public abstract void OnMessageReceived(BacktoryListener backtoryListener);

	}
}
