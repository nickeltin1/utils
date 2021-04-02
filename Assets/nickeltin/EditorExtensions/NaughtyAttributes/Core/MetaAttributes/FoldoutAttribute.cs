﻿using System;

namespace nickeltin.Editor.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FoldoutAttribute : MetaAttribute, IGroupAttribute
	{
		public string Name { get; private set; }

		public FoldoutAttribute(string name)
		{
			Name = name;
		}
	}
}
