using System;
using AirCoder.NaughtyAttributes.Scripts.Core.Utility;

namespace AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class EnableIfAttribute : EnableIfAttributeBase
	{
		public EnableIfAttribute(string condition)
			: base(condition)
		{
			Inverted = false;
		}

		public EnableIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Inverted = false;
		}
	}
}
