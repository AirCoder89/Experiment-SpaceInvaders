using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace AirCoder.NaughtyAttributes.Scripts.Test
{
	public class ShowNativePropertyTest : MonoBehaviour
	{
		[ShowNativeProperty]
		private Transform Transform
		{
			get
			{
				return transform;
			}
		}

		[ShowNativeProperty]
		private Transform ParentTransform
		{
			get
			{
				return transform.parent;
			}
		}
	}
}
