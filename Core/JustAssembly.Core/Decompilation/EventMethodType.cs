﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustAssembly.Core.Decompilation
{
	public enum EventMethodType : byte
	{
		AddMethod = 0,
		RemoveMethod,
		InvokeMethod
	}
}