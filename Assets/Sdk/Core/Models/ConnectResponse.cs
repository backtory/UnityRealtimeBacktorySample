using UnityEngine;
using System.Collections;

namespace Sdk.Core.Models
{
	public class ConnectResponse
	{
		public string UserId { get; private set; }

		public string Username { get; private set; }

		public ConnectResponse (string userId, string username)
		{
			this.UserId = userId;
			this.Username = username;
		}
	}
}
