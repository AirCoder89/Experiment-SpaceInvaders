using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Extensions
{
    public static class TransformExtensions
    {
        public static ITweenJob TweenScale(this Transform rt, Vector3 targetScale, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Scale, targetScale, duration);
        }
        public static ITweenJob TweenPosition(this Transform rt, Vector3 targetPosition, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Position, targetPosition, duration);
        }
        public static ITweenJob TweenRotation(this Transform rt, Vector3 targetRotation, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Rotation, targetRotation, duration);
        }
        public static ITweenJob TweenLocalPosition(this Transform rt, Vector3 targetPosition, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.LocalPosition, targetPosition, duration);
        }
        public static ITweenJob TweenLocalRotation(this Transform rt, Vector3 targetRotation, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.LocalRotation, targetRotation, duration);
        }
    }
}