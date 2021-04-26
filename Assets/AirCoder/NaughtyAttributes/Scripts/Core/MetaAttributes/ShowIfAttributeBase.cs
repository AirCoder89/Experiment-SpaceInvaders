using AirCoder.NaughtyAttributes.Scripts.Core.Utility;

namespace AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes
{
	public class ShowIfAttributeBase : MetaAttribute
	{
		public string[] Conditions { get; private set; }
		public EConditionOperator ConditionOperator { get; private set; }
		public bool Inverted { get; protected set; }

		public ShowIfAttributeBase(string condition)
		{
			ConditionOperator = EConditionOperator.And;
			Conditions = new string[1] { condition };
		}

		public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
		{
			ConditionOperator = conditionOperator;
			Conditions = conditions;
		}
	}
}
