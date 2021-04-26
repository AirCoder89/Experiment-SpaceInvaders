using AirCoder.TJ.Core.Jobs;
using AirCoder.TJ.Core.Sequences;
using UnityEngine;

namespace AirCoder.TJ.Core
{
    public static class JobFactory
    {
        public static ITweenJob CreateJob<T>()
        {
            ITweenJob job = null;
            
            if (typeof(RectTransform) == typeof(T))                  job = new RectTransformJob();
            else if (typeof(UnityEngine.Graphics) == typeof(T))      job = new GraphicsJob();
            else if (typeof(UnityEngine.UI.Graphic) == typeof(T))    job = new GraphicsJob();
            else if (typeof(Material) == typeof(T))                  job = new MaterialJob();
            else if (typeof(Transform) == typeof(T))                 job = new TransformJob();
            else if (typeof(CanvasGroup) == typeof(T))               job = new CanvasGroupJob();
            
            job?.Initialize(typeof(T));
            return job;
        }

        public static ITweenSequence CreateSequence(TJSystem system, SequenceType seqType)
        {
           return new SequenceJob(system, seqType);
        }
    }
}