using System;
using Models.Animations;

namespace Models.Grid
{
    [Serializable]
    public class GridAnimationData
    {
        public OpeningAnimationData  openingAnimation;
        public TweenData             killAnimation;
    }
}