using AirCoder.TJ.Core.Ease;
using AirCoder.TJ.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace AirCoder.TJ.Core.Jobs
{
    public class GraphicsJob: TweenJobBase
    {
        private Graphic _graphic;
        private float _startFloat;
        private float _targetFloat;
        private Color _targetColor;
        private Color _startColor;
        private Image _image;

        public override void ReleaseReferences()
        {
            base.ReleaseReferences();
            _image = null;
            _graphic = null;
            //TODO
        }

        public override ITweenJob TweenTo<T>(T targetInstance, JObType job, params object[] parameters)
        {
            currentJobType = job;
            _graphic = targetInstance as Graphic;
            if(selectedEase == null) selectedEase = Easing.GetEase(EaseType.Linear);
            
            switch (currentJobType) //setup
            {
                case JObType.Color:       jobAction = () => SetupTweenColor((Color) parameters[0]);       break;
                case JObType.Opacity:     jobAction = () => SetupTweenOpacity((float) parameters[0]);     break;
                case JObType.FillAmount:  jobAction = () => SetupTweenFillAmount((float) parameters[0]);  break;
                default: ThrowInvalidJobType();                                                           break;
            }
            duration = (float) parameters[1];
            return this;
        }

        public override void Tick(float deltaTime)
        {
            if(!IsPlaying) return;
            if(_graphic == null) ThrowMissingReferenceException(_graphic);
            currentTime += deltaTime;
            this.normalizedTime = currentTime / duration;
            if (currentTime >= duration) currentTime = duration;

            switch (currentJobType)
            {
                case JObType.Color:      InterpolateColor();      break;
                case JObType.Opacity:    InterpolateOpacity();    break;
                case JObType.FillAmount: InterpolateFillAmount(); break;
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
                    case JObType.Color:       jobAction = () => SetupTweenColor(_startColor);       break;
                    case JObType.Opacity:     jobAction = () => SetupTweenOpacity(_startFloat);     break;
                    case JObType.FillAmount:  jobAction = () => SetupTweenFillAmount(_startFloat);  break;
                    default: ThrowInvalidJobType();                                                 break;
                }
            }
            private void SetupTweenColor(Color targetColor)
            {
                _startColor = _graphic.color;
                _targetColor = targetColor - _startColor;
            }
    
            private void SetupTweenOpacity(float opacityAmount)
            {
                _startFloat = _graphic.color.a;
                _targetFloat = opacityAmount - _startFloat;
            }
    
            private void SetupTweenFillAmount(float targetAmount)
            {
                _image = (Image)_graphic;
                _startFloat = _image.fillAmount;
                _targetFloat = targetAmount - _startFloat;
            }
        #endregion

        #region Interpolation
            private void InterpolateColor()
            {
                var color = TweenColor(_startColor, _targetColor, currentTime, duration);
                _graphic.color = color;
            }
            
            private void InterpolateFillAmount()
            {
                var fill = InterpolateFloat(_startFloat, _targetFloat, currentTime, duration);
                _image.fillAmount = fill;
            }
                
            private void InterpolateOpacity()
            {
                var alpha = InterpolateFloat(_startFloat, _targetFloat, currentTime, duration);
                _graphic.SetAlpha(alpha);
            }
        #endregion
       
    }
}