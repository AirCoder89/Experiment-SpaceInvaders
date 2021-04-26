using System.Collections.Generic;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using UnityEngine;

namespace AirCoder.NaughtyAttributes.Scripts.Test
{
	//[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
	public class _NaughtyScriptableObject : ScriptableObject
	{
		public bool enableMyInt = true;
		public bool showMyFloat = true;

		[EnableIf("enableMyInt")]
		public int myInt;

		[ShowIf("showMyFloat")]
		public float myFloat;

		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 mySlider;

		//[ReorderableList]
		public List<int> list;

		[Button]
		private void IncrementMyInt()
		{
			myInt++;
		}

		[Button("Decrement My Int")]
		private void DecrementMyInt()
		{
			myInt--;
		}
	}
}
