using UnityEngine;
using System.Collections;

namespace Sdk.Core.Models.Exception {
	public class BacktoryException {
		
		public int Status { get; set; }
		public int? Code { get; set; }
		public string Name { get; set; }
		public string Message { get; set; }
		public long Timestamp { get; set; }

	}
}
