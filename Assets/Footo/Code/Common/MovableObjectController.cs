using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableObjectController : MonoBehaviour
{
    [System.Serializable]
    public class MovableObjectEvent
    {
        public EventTransceiver.Events OnEvent;
        public MonoBehaviour FromObject;
        public MovableObject TargetObject;
        public List<int> Positions = new List<int>();
        public int IterateAmount;
        public bool StateWrapMode = false;
        public bool Interruptable = true;
        [HideInInspector]public int CurrentPositionIndex = 0;
    }

    public List<MovableObjectEvent> MovableObjectEvents = new List<MovableObjectEvent>();

    private Dictionary<System.Type, List<MovableObjectEvent>> mEventDictionary = new Dictionary<System.Type, List<MovableObjectEvent>>();

    private void Start()
    {
        if (MovableObjectEvents.Count <= 0)
        {
            return;
        }

        foreach(MovableObjectEvent evt in MovableObjectEvents)
        {
            EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(evt.OnEvent).GetType(), EventHandler);

            if (mEventDictionary.ContainsKey(EventTransceiver.LookupEvent(evt.OnEvent).GetType()))
            {
                mEventDictionary[EventTransceiver.LookupEvent(evt.OnEvent).GetType()].Add(evt);
            }
            else
            {
                mEventDictionary.Add(EventTransceiver.LookupEvent(evt.OnEvent).GetType(), new List<MovableObjectEvent>(){evt});
            }
        }
    }

    public void EventHandler(object sender, EventBase evt)
    {
        foreach(MovableObjectEvent movEvt in mEventDictionary[evt.GetType()])
        {
            if (movEvt.FromObject != null && sender != movEvt.FromObject)
            {
                continue;
            }



//            if (movEvt.TargetObject.CurrentPositionIndex == movEvt.FromPosition)
//            {
//                movEvt.TargetObject.MoveToPosition(movEvt.ToPosition, movEvt.Interruptable);
//            }
//            else
//            {
//                movEvt.TargetObject.MoveToPosition(movEvt.FromPosition, movEvt.Interruptable);
//            }
        }
    }

}
