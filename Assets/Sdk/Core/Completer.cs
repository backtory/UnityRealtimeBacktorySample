using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

namespace Sdk.Core {
	public interface Completer {
		void Execute(JSONNode response);
	}
}
