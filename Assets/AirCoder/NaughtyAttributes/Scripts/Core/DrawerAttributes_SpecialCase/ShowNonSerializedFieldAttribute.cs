using System;

namespace AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
	{
	}
}
