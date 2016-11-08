using UnityEngine;
using System.Collections;
using Sdk.Core.Models.Exception;
using System.Collections.Generic;

namespace Sdk.Core.Listeners
{
	public interface RealtimeSdkListener : BacktoryListener
	{
		void OnDisconnect ();

//		void OnMessage (string message);
		
		void OnException (ExceptionMessage exception);
	}
}
