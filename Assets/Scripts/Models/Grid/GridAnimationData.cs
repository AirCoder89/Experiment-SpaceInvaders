using System;
using Models.Animations;
using Utils.Array2D;

namespace Models.Grid
{
    [Serializable]
    public class GridAnimationData
    {
        public MatrixLine            basedLine;
        public OpeningAnimationData  openingAnimation;
        public TweenData             killAnimation;
    }
}