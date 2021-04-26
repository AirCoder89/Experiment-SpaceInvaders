using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static ITweenJob TweenOpacity(this CanvasGroup canvasGroup, float alpha, float duration = 1f)
        {
            return TJSystem.Tween(canvasGroup,JObType.Opacity, alpha, duration);
        }
    }
}