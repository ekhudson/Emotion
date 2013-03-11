using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableObject : MonoBehaviour
{
    [System.Serializable]
    public class MoveableObjectPosition
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
    public float MoveSpeed = 1f;

    [HideInInspector]public List<MoveableObjectPosition> Positions = new List<MoveableObjectPosition>();

    private float mPingPongDelay = 0f;
    private bool mLooping = false;

    private bool mInterruptable = true;

    private int mTargetPosition;
    private int mPreviousPosition;

    private Vector3 mMoveDirection = Vector3.zero;
    private Vector3 mOriginalPosition = Vector3.zero;
    private Vector3 mOriginalSize = Vector3.zero; //used for drawing the future position

    private const float kMinimumStopDistance = 0.1f;

    public enum MoveableObjectStates
    {
        IDLE,
        MOVING,
        PINGPONG,
    }

    public MoveableObjectStates State;

    public bool IsInterruptable
    {
        get
        {
            return mInterruptable;
        }
    }

    public Vector3 OriginalPosition
    {
        get
        {
            return mOriginalPosition;
        }
    }

	// Use this for initialization
	private void Start ()
    {
        mOriginalPosition = transform.position;
        mOriginalSize = renderer.bounds.size;
	}

    private void Update()
    {
        switch(State)
        {
            case MoveableObjectStates.IDLE:

            break;

            case MoveableObjectStates.MOVING:

                transform.position = Vector3.Lerp(transform.position, mOriginalPosition + Positions[mTargetPosition].Position, MoveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Positions[mTargetPosition].Rotation, MoveSpeed * Time.deltaTime);

                if (transform.position == (mOriginalPosition + Positions[mTargetPosition].Position))
                {
                    State = MoveableObjectStates.IDLE;
                }

            break;

            case MoveableObjectStates.PINGPONG:

            break;
        }
    }

    public void MoveToPosition(int position)
    {
        MoveToPosition(position, true);
    }

    public void MoveToPosition(int position, bool interruptable)
    {
        if (State == MoveableObjectStates.MOVING && !mInterruptable)
        {
            return;
        }

        mInterruptable = interruptable;
        mTargetPosition = position;
        State = MoveableObjectStates.MOVING;
    }

    private void OnDrawGizmosSelected()
    {
        if (Positions == null || Positions.Count <= 0)
        {
            return;
        }

        if (Application.isEditor && !Application.isPlaying)
        {
            mOriginalPosition = transform.position;
            mOriginalSize = renderer.bounds.size;
        }

        foreach(MoveableObjectPosition position in Positions)
        {
            Gizmos.DrawWireCube(mOriginalPosition + position.Position, position.Rotation * mOriginalSize);
        }
    }
}
