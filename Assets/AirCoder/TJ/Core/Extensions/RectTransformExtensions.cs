using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Extensions
{
    public static class RectTransformExtensions
    {
        public static ITweenJob TweenScale(this RectTransform rt, Vector2 targetScale, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Scale, targetScale, duration);
        }
        public static ITweenJob TweenAnchorPosition(this RectTransform rt, Vector2 targetPosition, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Position, targetPosition, duration);
        }
        public static ITweenJob TweenRotation(this RectTransform rt, Vector2 targetRotation, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Rotation, targetRotation, duration);
        }
        public static ITweenJob TweenSize(this RectTransform rt, Vector2 targetScale, float duration = 1f)
        {
            return TJSystem.Tween(rt, JObType.Size, targetScale, duration);
        }
    }
}
