using System;

namespace nickeltin.Extensions.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
	{
	}
}
