using AirCoder.TJ.Core.Ease;
using UnityEngine;

namespace AirCoder.TJ.Core.Jobs
{
    public class TransformJob : TweenJobBase
    {
        private Transform _transform;
        private Vector3 _startVector3;
        private Vector3 _targetVector3;

        public override void ReleaseReferences()
        {
            base.ReleaseReferences();
            _transform = null;
        }
        
        //convention parameters : 0[targetValue] / 1[duration]
        public override ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters)
        {
            currentJobType = job;
            _transform = targetInstance as Transform;
            if(selectedEase == null) selectedEase = Easing.GetEase(EaseType.Linear);
            
            switch (currentJobType)
            {
                case JObType.Position:      jobAction = () => SetupTweenPosition((Vector3) parameters[0]);         break;
                case JObType.Scale:         jobAction = () => SetupTweenScale((Vector3) parameters[0]);            break;
                case JObType.Rotation:      jobAction = () => SetupTweenRotation((Vector3) parameters[0]);         break;
                case JObType.LocalPosition: jobAction = () => SetupTweenLocalPosition((Vector3) parameters[0]);    break;
                case JObType.LocalRotation: jobAction = () => SetupTweenLocalRotation((Vector3) parameters[0]);    break;
                default: ThrowInvalidJobType();                                                                    break;
            }
            
            duration = (float) parameters[1];
            return this;
        }

        public override void Tick(float deltaTime)
        {
            if(!IsPlaying) return;
            if(_transform == null) ThrowMissingReferenceException(_transform);
            currentTime += deltaTime;
            this.normalizedTime = currentTime / duration;
            if (currentTime >= duration) currentTime = duration;

            switch (currentJobType)
            {
                case JObType.Scale:             InterpolateScale();             break;
                case JObType.Position:          InterpolatePosition();          break;
                case JObType.Rotation:          InterpolateRotation();          break;
                case JObType.LocalPosition:     InterpolateLocalPosition();     break;
                case JObType.LocalRotation:     InterpolateLocalRotation();     break;
                default:                        ThrowInvalidJobType();          break;
            }
            
            base.RaiseOnTick(); //raise the onUpdate event
            base.CheckJobInterpolationComplete();
        }
        
        
        #region Setup

            public override void SetupRewind()
            {
                switch (currentJobType)
                {
                    case JObType.Position:           jobAction = () => SetupTweenPosition(_startVector3);       break;
                    case JObType.Scale:              jobAction = () => SetupTweenScale(_startVector3);          break;
                    case JObType.Rotation:           jobAction = () => SetupTweenRotation(_startVector3);       break;
                    case JObType.LocalPosition:      jobAction = () => SetupTweenLocalPosition(_startVector3);  break;
                    case JObType.LocalRotation:      jobAction = () => SetupTweenLocalRotation(_startVector3);  break;
                    default: ThrowInvalidJobType();                                                             break;
                }
            }
        
            private void SetupTweenPosition(Vector3 targetPosition)
            {
                _startVector3 = _transform.position;
                _targetVector3 = targetPosition - _startVector3;
            }
            
            private void SetupTweenLocalPosition(Vector3 targetPosition)
            {
                _startVector3 = _transform.localPosition;
                _targetVector3 = targetPosition - _startVector3;
            }
                
            private void SetupTweenScale(Vector3 targetScale)
            {
                _startVector3 = _transform.localScale;
                _targetVector3 = targetScale - _startVector3;
            }
            
            private void SetupTweenRotation(Vector3 targetRotation)
            {
                _startVector3 = _transform.eulerAngles;
                _targetVector3 = targetRotation - _startVector3;
            }
            
            private void SetupTweenLocalRotation(Vector3 targetRotation)
            {
                _startVector3 = _transform.localEulerAngles;
                _targetVector3 = targetRotation - _startVector3;
            }
            
        #endregion
        
        #region Interpolation
            private void InterpolatePosition()
            {
                var pos = TweenVector3(_startVector3, _targetVector3, this.currentTime, duration);
                _transform.position = pos;
            }
            
            private void InterpolateLocalPosition()
            {
                var pos = TweenVector3(_startVector3, _targetVector3, this.currentTime, duration);
                _transform.localPosition = pos;
            }
                
            private void InterpolateScale()
            {
                 var scale = TweenVector3(_startVector3, _targetVector3, this.currentTime, duration);
                 _transform.localScale = scale;
            }
                
            private void InterpolateRotation()
            {
                var rotation = TweenVector3(_startVector3, _targetVector3, this.currentTime, duration);
                _transform.eulerAngles = rotation;
            }
            
            private void InterpolateLocalRotation()
            {
                var rotation = TweenVector3(_startVector3, _targetVector3, this.currentTime, duration);
                _transform.localEulerAngles = rotation;
            }
        #endregion
        
    }
}