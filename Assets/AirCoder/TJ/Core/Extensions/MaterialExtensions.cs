using AirCoder.TJ.Core.Jobs;
using UnityEngine;

namespace AirCoder.TJ.Core.Extensions
{
    public static class MaterialExtensions
    {
        private static string mainTexture = "_MainTex";
        
        //-convention parameters : 0[targetValue] / 1[propertyName] / 2[Duration]
        public static ITweenJob TweenOpacity(this Material material, float alpha, float duration = 1f)
        {
            return TJSystem.Tween(material,JObType.Opacity, alpha, mainTexture, duration);
        }
        public static ITweenJob TweenOpacity(this Material material, float alpha,string property, float duration = 1f)
        {
            return TJSystem.Tween(material, JObType.Opacity,alpha,property, duration);
        }
        
        public static ITweenJob TweenColor(this Material material, Color targetColor, float duration = 1f)
        {
            return TJSystem.Tween(material, JObType.Color,targetColor, mainTexture, duration);
        }
        public static ITweenJob TweenColor(this Material material, Color targetColor,string property, float duration = 1f)
        {
            return TJSystem.Tween(material, JObType.Color,targetColor,property, duration);
        }
        
        public static ITweenJob TweenOffset(this Material material, Vector2 targetOffset, float duration = 1f)
        {
            return TJSystem.Tween(material, JObType.Offset,targetOffset, mainTexture, duration);
        }
        
        public static ITweenJob TweenOffset(this Material material, Vector2 targetOffset, string property, float duration = 1f)
        {
            return TJSystem.Tween(material, JObType.Offset,targetOffset,property, duration);
        }
    }
}