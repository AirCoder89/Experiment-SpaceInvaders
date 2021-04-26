using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Extensions
{
    public static class SpriteRendererExtensions
    {
        public static void SetAlpha(this SpriteRenderer spRenderer, float alpha)
        {
            var color = spRenderer.color;
            color.a = alpha;
            spRenderer.color = color;
        }
        
        public static ITweenJob TweenOpacity(this SpriteRenderer spRenderer, float alpha, float duration = 1f)
        {
            return TJSystem.Tween(spRenderer,JObType.Opacity, alpha, duration);
        }
        
        public static ITweenJob TweenColor(this SpriteRenderer spRenderer, Color targetColor, float duration = 1f)
        {
            return TJSystem.Tween(spRenderer, JObType.Color, targetColor, duration);
        }
    }
}