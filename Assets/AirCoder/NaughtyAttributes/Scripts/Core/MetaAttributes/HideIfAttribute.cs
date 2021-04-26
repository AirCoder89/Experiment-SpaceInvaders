using System;
using AirCoder.NaughtyAttributes.Scripts.Core.Utility;

namespace AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class HideIfAttribute : ShowIfAttributeBase
	{
		public HideIfAttribute(string condition)
			: base(condition)
		{
			Inverted = true;
		}

		public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Inverted = true;
		}
	}
}
