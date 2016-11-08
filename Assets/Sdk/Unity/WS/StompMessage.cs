using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sdk.Unity.WS
{
	public class StompMessage
	{
		
		private String command;
		private Dictionary<String, String> headers;
		private String body;
		
		public StompMessage (String command, Dictionary<String, String> headers, String body)
		{
			this.command = command;
			this.headers = headers != null ? headers : new Dictionary<String, String> ();
			this.body = body != null ? body : "";
		}
		
		public String getCommand ()
		{
			return command;
		}
		
		public Dictionary<String, String> getHeaders ()
		{
			return headers;
		}
		
		public String getBody ()
		{
			return body;
		}

		private String toStringg ()
		{
			String strLines = this.command;
			strLines += MyByte.LF;
			foreach (KeyValuePair<String, String> entry in headers) {
				String key = entry.Key;
				strLines += key + ":" + this.headers [key];
				strLines += MyByte.LF;
			}
			strLines += MyByte.LF;
			strLines += this.body;
			strLines += MyByte.NULL;
			
			return strLines;
		}

		public static StompMessage fromString (String data)
		{
			List<String> contents = data.Split (MyByte.LF).ToList ();

			while (contents.Count > 0 && contents[0].Equals("")) {
				contents.RemoveAt (0);
			}
			
			if (contents.Count == 0) {
				return null;
			}
			
			String command = contents [0];
			Dictionary<String, String> headers = new Dictionary<String, String> ();
			String body = "";
			
			contents.RemoveAt (0);
			bool hasHeaders = false;
			foreach (String line in contents) {
				if (hasHeaders) {
					for (int i = 0; i < line.Length; i++) {
						char c = line [i];
						if (!c.Equals ('\0'))
							body += c;
					}
				} else {
					if (line.Equals ("")) {
						hasHeaders = true;
					} else {
						String[] header = line.Split (':');
						headers.Add (header [0], header [1]);
					}
				}
			}
			return new StompMessage (command, headers, body);
		}
		
		public static String marshall (String command, Dictionary<String, String> headers, String body)
		{
			StompMessage frame = new StompMessage (command, headers, body);
			return frame.toStringg ();
		}
		
		internal class MyByte
		{
			public const char LF = '\n';
			public const char NULL = '\0';
		}
	}
}