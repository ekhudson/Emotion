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

    public int StartPosition = 0;
    [HideInInspector]public List<MoveableObjectPosition> Positions = new List<MoveableObjectPosition>();

    private float mPingPongDelay = 0f;
    private bool mLooping = false;

    private bool mInterruptable = true;

    private int mTargetPosition = -1;
    private int mPreviousPosition = -1;
    private int mCurrentPosition;

    private Vector3 mMoveDirection = Vector3.zero;
    private Vector3 mOriginalPosition = Vector3.zero;
    private Vector3 mOriginalSize = Vector3.zero; //used for drawing the future position
    private Quaternion mOriginalRotation = Quaternion.identity; //used for drawing hte future positions
    private float mCurrentMoveTime = 0f;

    private const float kMinimumStopDistance = 0.01f;

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

    public int CurrentPositionIndex
    {
        get
        {
            return mCurrentPosition;
        }
    }

	// Use this for initialization
	private void Start ()
    {
        mOriginalPosition = transform.position;
        mOriginalSize = renderer.bounds.size;
        mOriginalRotation = transform.rotation;
        mCurrentPosition = StartPosition;
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

                if ( ((mOriginalPosition + Positions[mTargetPosition].Position) - transform.position).magnitude < kMinimumStopDistance)
                {
                    mCurrentMoveTime = 0;
                    mCurrentPosition = mTargetPosition;
                    mTargetPosition = -1;
                    State = MoveableObjectStates.IDLE;
                    return;
                }

                mCurrentMoveTime += Time.deltaTime;

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

        mCurrentMoveTime = 0f;
        mInterruptable = interruptable;
        mPreviousPosition = mCurrentPosition;
        mCurrentPosition = -1;
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
            mOriginalRotation = transform.rotation;
        }

        foreach(MoveableObjectPosition position in Positions)
        {
            Gizmos.DrawWireCube(mOriginalPosition + position.Position, position.Rotation * (mOriginalRotation * mOriginalSize));
        }
    }
}
