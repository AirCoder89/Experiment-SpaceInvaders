using AirCoder.TJ.Core.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace AirCoder.TJ.Core.Extensions
{
    public static class GraphicExtensions
    {
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }

        public static ITweenJob TweenOpacity(this Graphic graphic, float alpha, float duration = 1f)
        {
            return TJSystem.Tween(graphic,JObType.Opacity, alpha, duration);
        }
        
        public static ITweenJob TweenColor(this Graphic graphic, Color targetColor, float duration = 1f)
        {
            return TJSystem.Tween(graphic,JObType.Color, targetColor, duration);
        }
    }
    
    //- Concrete Graphic Types
    public static class ImageExtensions
    {
        public static ITweenJob TweenFillAmount(this Image image, float fillAmount, float duration = 1f)
        {
            return TJSystem.Tween((Graphic)image,JObType.FillAmount, fillAmount, duration);
        }
    }
}