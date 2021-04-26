using AirCoder.TJ.Core.Ease;
using UnityEngine;

namespace AirCoder.TJ.Core.Jobs
{
    public class CanvasGroupJob : TweenJobBase
    {
        private CanvasGroup _canvasGroup;
        private float _startFloat;
        private float _targetFloat;

        public override void ReleaseReferences()
        {
            base.ReleaseReferences();
            _canvasGroup = null;
        }

        internal void Test()
        {
            Debug.Log($"Test");
        }
        
        public override ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters)
        {
            currentJobType = job;
            _canvasGroup = targetInstance as CanvasGroup;
            if(selectedEase == null) selectedEase = Easing.GetEase(EaseType.Linear);
            
            switch (currentJobType) //setup
            {
                case JObType.Opacity:     jobAction = () => SetupTweenOpacity((float) parameters[0]);     break;
                default: ThrowInvalidJobType();                                                           break;
            }
            duration = (float) parameters[1];
            return this;
        }

        public override void Tick(float deltaTime)
        {
            if(!IsPlaying) return;
            if(_canvasGroup == null) ThrowMissingReferenceException(_canvasGroup);
            currentTime += deltaTime;
            this.normalizedTime = currentTime / duration;
            if (currentTime >= duration) currentTime = duration;

            switch (currentJobType)
            {
                case JObType.Opacity:    InterpolateOpacity();    break;
                default:                 ThrowInvalidJobType();   break;
            }
            
            base.RaiseOnTick(); //raise the onUpdate event
            base.CheckJobInterpolationComplete();
        }
        
        #region Setup
        public override void SetupRewind()
        {
            switch (currentJobType) 
            {
                case JObType.Opacity:     jobAction = () => SetupTweenOpacity(_startFloat);     break;
                default: ThrowInvalidJobType();                                                 break;
            }
        }
        
        private void SetupTweenOpacity(float opacityAmount)
        {
            _startFloat = _canvasGroup.alpha;
            _targetFloat = opacityAmount - _startFloat;
        }
        #endregion
        
        #region Interpolation
             
        private void InterpolateOpacity()
        {
            var alpha = InterpolateFloat(_startFloat, _targetFloat, currentTime, duration);
            _canvasGroup.alpha = alpha;
        }
        #endregion
    }
}