using AirCoder.Core;
using AirCoder.Core.Events;
using AirCoder.Core.Views;
using Application = AirCoder.Core.Application;

    public class  : GameSystem
    {

        public (GameController inController, Application inApp, SystemConfig inConfig = null)
            : base(inController, inApp, inConfig)
        {
        }

        public void AddEventListener(eventName inEvent, EventSystem.EventHandler inHandler)
            => _eventsManager.AddEventListener(inEvent, inHandler);

        public virtual void RemoveEventListener(eventName inEvent, EventSystem.EventHandler inHandler)
            => _eventsManager.RemoveEventListener(inEvent, inHandler);

        public virtual void RemoveEventListener(eventName inEvent)
            =>  _eventsManager.RemoveEventListener(inEvent);
    }
