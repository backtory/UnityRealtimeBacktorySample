using UnityEngine;
using System.Collections;
using Sdk.Core.Listeners;

namespace Sdk.Core.Models.Exception {
	public class ExceptionMessage : BacktoryConnectivityMessage {
		private BacktoryException exception;

		public int Status { get; set; }
		public int? Code { get; set; }
		public string Name { get; set; }
		public string Message { get; set; }
		public long Timestamp { get; set; }

		override
		public void OnMessageReceived(BacktoryListener backtoryListener) {
			RealtimeSdkListener generalListener = (RealtimeSdkListener) backtoryListener;

			this.Status = exception.Status;
			this.Code = exception.Code;
			this.Name = exception.Name;
			this.Message = exception.Message;
			this.Timestamp = exception.Timestamp;

			generalListener.OnException(this);
		}

	}
}