﻿using System;

namespace nickeltin.Editor.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReorderableListAttribute : SpecialCaseDrawerAttribute
	{
		public string itemName;

		public ReorderableListAttribute()
		{
			itemName = string.Empty;
		}

		public ReorderableListAttribute(string itemName) : base()
		{
			this.itemName = itemName;
		}
	}
}
