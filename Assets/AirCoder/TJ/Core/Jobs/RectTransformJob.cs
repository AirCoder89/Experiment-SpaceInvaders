using AirCoder.TJ.Core.Ease;
using UnityEngine;

namespace AirCoder.TJ.Core.Jobs
{
    public class RectTransformJob : TweenJobBase
    {
        private RectTransform _rectTransform;

        private Vector2 _startVector3;
        private Vector2 _targetVector3;

        public override void ReleaseReferences()
        {
            base.ReleaseReferences();
            _rectTransform = null;
        }

        //convention parameters : 0[targetValue] / 1[duration]
        public override ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters)
        {
            currentJobType = job;
            _rectTransform = targetInstance as RectTransform;
            if(selectedEase == null) selectedEase = Easing.GetEase(EaseType.Linear);
            
            switch (currentJobType)
            {
                case JObType.Position:  jobAction = () => SetupTweenPosition((Vector2) parameters[0]); break;
                case JObType.Scale:     jobAction = () => SetupTweenScale((Vector2) parameters[0]);    break;
                case JObType.Rotation:  jobAction = () => SetupTweenRotation((Vector2) parameters[0]); break;
                case JObType.Size:      jobAction = () => SetupTweenSize((Vector2) parameters[0]); break;
                default: ThrowInvalidJobType();                                                        break;
            }
            
            duration = (float) parameters[1];
            return this;
        }

        public override void Tick(float deltaTime)
        {
            if(!IsPlaying) return;
            
            if(_rectTransform == null) ThrowMissingReferenceException(_rectTransform);
            currentTime += deltaTime;
            this.normalizedTime = currentTime / duration;
            if (currentTime >= duration) currentTime = duration;

            switch (currentJobType)
            {
                case JObType.Scale:    InterpolateScale();    break;
                case JObType.Position: InterpolatePosition(); break;
                case JObType.Rotation: InterpolateRotation(); break;
                case JObType.Size:     InterpolateSize();     break;
                default:               ThrowInvalidJobType(); break;
            }
            
            base.RaiseOnTick(); //raise the onUpdate event
            base.CheckJobInterpolationComplete();
        }
        
        #region Setup

        public override void SetupRewind()
            {
                switch (currentJobType)
                {
                    case JObType.Position:  jobAction = () => SetupTweenPosition(_startVector3); break;
                    case JObType.Scale:     jobAction = () => SetupTweenScale(_startVector3);    break;
                    case JObType.Rotation:  jobAction = () => SetupTweenRotation(_startVector3); break;
                    case JObType.Size:      jobAction = () => SetupTweenSize(_startVector3); break;
                    default: ThrowInvalidJobType();                                              break;
                }
            }
        
            private void SetupTweenPosition(Vector2 targetPosition)
            {
                _startVector3 = _rectTransform.anchoredPosition;
                _targetVector3 = targetPosition - _startVector3;
            }
                
            private void SetupTweenScale(Vector2 targetScale)
            {
                _startVector3 = _rectTransform.localScale;
                _targetVector3 = targetScale - _startVector3;
            }
            
            private void SetupTweenRotation(Vector2 targetRotation)
            {
                _startVector3 = _rectTransform.eulerAngles;
                _targetVector3 = targetRotation - _startVector3;
            }
            
            private void SetupTweenSize(Vector2 targetRotation)
            {
                _startVector3 = _rectTransform.sizeDelta;
                _targetVector3 = targetRotation - _startVector3;
            }
            
            
        #endregion
        
        #region Interpolation
            private void InterpolatePosition()
            {
                var pos = TweenVector2(_startVector3, _targetVector3, this.currentTime, duration);
                _rectTransform.anchoredPosition = pos;
            }
                
            private void InterpolateScale()
            {
                 var scale = TweenVector2(_startVector3, _targetVector3, this.currentTime, duration);
                _rectTransform.localScale = scale;
            }
                
            private void InterpolateRotation()
            {
                var rotation = TweenVector2(_startVector3, _targetVector3, this.currentTime, duration);
                _rectTransform.eulerAngles = rotation;
            }
            
            private void InterpolateSize()
            {
                var size = TweenVector2(_startVector3, _targetVector3, this.currentTime, duration);
                _rectTransform.sizeDelta = size;
            }
        #endregion
    }
}