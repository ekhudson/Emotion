using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableObject : MonoBehaviour
{
    public Vector3 OffPosition;
    public Vector3 OnPosition;
    public bool StartOff = true;

    public float StartDelay;
    public float MoveSpeed = 1f;
    public bool PingPong = false;
    public float PingPongDelay = 0f;

    public List<EditorObject> AcceptEventsFromObjects = new List<EditorObject>();
    public EventTransceiver.Events[] ActivateOnEvents;
    public EventTransceiver.Events[] DeactivateOnEvents;

    private Vector3 mMoveDirection = Vector3.zero;
    private Vector3 mOriginalPosition = Vector3.zero;

    public enum MovableObjectStates
    {
        ON,
        MOVING_TO_OFF,
        MOVING_TO_ON,
        OFF,
    }

    public MovableObjectStates State;

	// Use this for initialization
	private void Start ()
    {
        mOriginalPosition = transform.position;

        if (StartOff)
        {
            State = MovableObjectStates.OFF;
            transform.position = mOriginalPosition + OffPosition;
        }
        else
        {
            State = MovableObjectStates.ON;
            transform.position = mOriginalPosition + OnPosition;
        }

        foreach(EventTransceiver.Events evt in ActivateOnEvents)
        {
            EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(evt).GetType(), Activate);
        }

        foreach(EventTransceiver.Events evt in DeactivateOnEvents)
        {
            EventManager.Instance.AddHandler(EventTransceiver.LookupEvent(evt).GetType(), Deactivate);
        }
	}

    private void Update()
    {
        switch(State)
        {
            case MovableObjectStates.ON:

            break;

            case MovableObjectStates.MOVING_TO_OFF:

                transform.position = Vector3.Lerp(transform.position, mOriginalPosition + OffPosition, MoveSpeed * Time.deltaTime);

                if (transform.position == transform.position + OffPosition)
                {
                    State = MovableObjectStates.MOVING_TO_OFF;
                }

            break;

            case MovableObjectStates.MOVING_TO_ON:

                transform.position = Vector3.Lerp(transform.position, mOriginalPosition + OnPosition, MoveSpeed * Time.deltaTime);

                if (transform.position == transform.position + OnPosition)
                {
                    State = MovableObjectStates.MOVING_TO_ON;
                }

            break;

            case MovableObjectStates.OFF:

            break;
        }
    }

    private void Activate(object sender, EventBase evt)
    {

        if (!AcceptEventsFromObjects.Contains(evt.Sender as EditorObject))
        {
            return;
        }

        if (StartOff)
        {

            if (State == MovableObjectStates.OFF || State == MovableObjectStates.MOVING_TO_OFF)
            {
                State = MovableObjectStates.MOVING_TO_ON;
                mMoveDirection = (OnPosition - OffPosition).normalized;
            }
        }
        else
        {
            if (State == MovableObjectStates.ON || State == MovableObjectStates.MOVING_TO_ON)
            {
                State = MovableObjectStates.MOVING_TO_OFF;
                mMoveDirection = (OffPosition - OnPosition).normalized;
            }
        }
    }

    private void Deactivate(object sender, EventBase evt)
    {
        if (!AcceptEventsFromObjects.Contains(evt.Sender as EditorObject))
        {
            return;
        }

        if (StartOff)
        {

            if (State == MovableObjectStates.ON || State == MovableObjectStates.MOVING_TO_ON)
            {
                State = MovableObjectStates.MOVING_TO_OFF;
                mMoveDirection = (OffPosition - OnPosition).normalized;
            }
        }
        else
        {
            if (State == MovableObjectStates.OFF || State == MovableObjectStates.MOVING_TO_OFF)
            {
                State = MovableObjectStates.MOVING_TO_ON;
                mMoveDirection = (OnPosition - OffPosition).normalized;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 offPos;
        Vector3 onPos;
        Vector3 pos = Application.isPlaying == true ? mOriginalPosition : transform.position;

        if (StartOff)
        {
            if (OffPosition == null)
            {
                offPos = pos + OffPosition;
                onPos = pos + new Vector3(0,renderer.bounds.size.y,0);
            }
        }
        else
        {
            if (OnPosition == null)
            {
                onPos = pos;
                offPos = pos - new Vector3(0,renderer.bounds.size.y,0);
            }
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(pos + OffPosition, renderer.bounds.size);

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(pos + OnPosition, renderer.bounds.size);
    }
}
