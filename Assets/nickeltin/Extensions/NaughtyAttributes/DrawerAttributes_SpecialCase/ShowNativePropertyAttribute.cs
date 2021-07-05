using System;

namespace nickeltin.Extensions.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ShowNativePropertyAttribute : SpecialCaseDrawerAttribute
	{
	}
}
