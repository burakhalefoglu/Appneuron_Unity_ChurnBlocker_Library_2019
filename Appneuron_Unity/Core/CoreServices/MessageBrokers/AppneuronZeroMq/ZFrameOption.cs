using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AppneuronZeroMq
{
	public enum ZFrameOption : int
	{
		MORE = 1,
		SRCFD = 2,
		SHARED = 3,
	}
}