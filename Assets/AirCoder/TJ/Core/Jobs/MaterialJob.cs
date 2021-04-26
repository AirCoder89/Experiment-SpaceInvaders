using AirCoder.TJ.Core.Ease;
using UnityEngine;

namespace AirCoder.TJ.Core.Jobs
{
    public class MaterialJob : TweenJobBase
    {
        private Material _material;
        private float _startFloat;
        private float _targetFloat;
        private Vector3 _startVector3;
        private Vector3 _targetVector3;
        private Color _targetColor;
        private Color _startColor;
        private string _propertyName;

        public override void ReleaseReferences()
        {
            base.ReleaseReferences();
            _material = null;
            //TODO
        }

        //-convention parameters : 0[targetValue] / 1[propertyName] / 2[Duration]

        public override ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters)
        {
            currentJobType = job;
            _material = targetInstance as Material;
            if(selectedEase == null) selectedEase = Easing.GetEase(EaseType.Linear);
            
            _propertyName = (string) parameters[1]; //assign property name
            switch (currentJobType)
            {
                case JObType.Offset:   jobAction = () => SetupTweenOffset((Vector2) parameters[0]); break;
                case JObType.Color:    jobAction = () => SetupTweenColor((Color) parameters[0]);    break;
                case JObType.Opacity:  jobAction = () => SetupTweenOpacity((float) parameters[0]);  break;
                default: ThrowInvalidJobType();                                                     break;
            }
            duration = (float) parameters[2];
            return this;
        }

        public override void Tick(float deltaTime)
        {
            if(!IsPlaying) return;
            if(_material == null) ThrowMissingReferenceException(_material);
            currentTime += deltaTime;
            this.normalizedTime = currentTime / duration;
            if (currentTime >= duration) currentTime = duration;

            
            switch (currentJobType)
            {
                case JObType.Offset:  InterpolateOffset();  break;
                case JObType.Color:   InterpolateColor();   break;
                case JObType.Opacity: InterpolateOpacity(); break;
                default:              ThrowInvalidJobType();break;
            }
            
            base.RaiseOnTick(); //raise the onUpdate event
            base.CheckJobInterpolationComplete();
        }
        
        #region Setup

        public override void SetupRewind()
            {
                switch (currentJobType)
                {
                    case JObType.Offset:   jobAction = () => SetupTweenOffset(_startVector3); break;
                    case JObType.Color:    jobAction = () => SetupTweenColor(_startColor);    break;
                    case JObType.Opacity:  jobAction = () => SetupTweenOpacity(_startFloat);  break;
                    default: ThrowInvalidJobType();                                           break;
                }
            }
            private void SetupTweenOpacity(float opacityAmount)
            {
                _startFloat = _material.GetColor(_propertyName).a;
                _targetFloat = opacityAmount - _startFloat;
            }
    
            private void SetupTweenColor(Color targetColor)
            {
                _startColor = _material.GetColor(_propertyName);
                _targetColor = targetColor - _startColor;
            }
    
            private void SetupTweenOffset(Vector2 endOffsetValue)
            {
                _startVector3 = _material.GetTextureOffset(_propertyName);
                _targetVector3 = endOffsetValue - (Vector2)_startVector3;
            }
        #endregion

        #region Interpolation
            private void InterpolateOffset()
            {
                var offset = TweenVector3(_startVector3, _targetVector3, currentTime, duration);
                _material.SetTextureOffset(_propertyName, (Vector2)offset);
            }
                
            private void InterpolateColor()
            {
                var color = TweenColor(_startColor, _targetColor, currentTime, duration);
                _material.SetColor(_propertyName, color);
            }
            
            private void InterpolateOpacity()
            {
                var alpha = InterpolateFloat(_startFloat, _targetFloat, currentTime, duration);
                var finalColor = _material.GetColor(_propertyName);
                finalColor.a = alpha;
                _material.SetColor(_propertyName, finalColor);
            }
        #endregion
       
    }
}