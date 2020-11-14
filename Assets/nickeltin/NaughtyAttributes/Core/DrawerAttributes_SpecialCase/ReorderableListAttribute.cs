using System;

namespace nickeltin.Editor.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReorderableListAttribute : SpecialCaseDrawerAttribute
	{
	}
}
