using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
    private FootoEntity mEntity;

    private List<GameObject> mCandidateList = new List<GameObject>();
    private Vector3 mTargetLocation;

    private Vector3 mCurrentMoveDirection;

    private GameObject mCurrentTarget;

    private SearchRadius mSearchRadius;

    public enum MonsterBehaviourStates
    {
        IDLE,
        HIDING,
        HUNTING,
        CHASING,
        ATTACKING,
        FLEEING,
    }

    private MonsterBehaviourStates mBehaviourState;

    public List<GameObject> CandidateList
    {
        get
        {
            return mCandidateList;
        }
    }

    public FootoEntity OwnerEntity
    {
        get
        {
            return mEntity;
        }
    }

    private void Start()
    {
        mEntity = GetComponent<FootoEntity>();
        mSearchRadius = GetComponentInChildren<SearchRadius>();
    }

    private void Update()
    {
        switch(mBehaviourState)
        {
            case MonsterBehaviourStates.ATTACKING:

            break;

            case MonsterBehaviourStates.CHASING:

            break;

            case MonsterBehaviourStates.FLEEING:

            break;

            case MonsterBehaviourStates.HIDING:

            break;

            case MonsterBehaviourStates.HUNTING:

            break;

            case MonsterBehaviourStates.IDLE:

            break;
        }
    }

    public void SetState(MonsterBehaviourStates newState)
    {
        if (newState == mBehaviourState)
        {
            return;
        }

        switch(newState)
        {
            case MonsterBehaviourStates.ATTACKING:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            case MonsterBehaviourStates.CHASING:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            case MonsterBehaviourStates.FLEEING:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            case MonsterBehaviourStates.HIDING:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            case MonsterBehaviourStates.HUNTING:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            case MonsterBehaviourStates.IDLE:

                switch(mBehaviourState)
                {
                    case MonsterBehaviourStates.ATTACKING:

                    break;

                    case MonsterBehaviourStates.CHASING:

                    break;

                    case MonsterBehaviourStates.FLEEING:

                    break;

                    case MonsterBehaviourStates.HIDING:

                    break;

                    case MonsterBehaviourStates.HUNTING:

                    break;

                    case MonsterBehaviourStates.IDLE:
        
                    break;

                    default:

                    break;
                }

            break;

            default:

            break;
        }

        mBehaviourState = newState;
    }

    private void LookForEntitiesByTag(string tag)
    {
        foreach(Collider other in mSearchRadius.ObjectList)
        {
            if (other.tag == tag)
            {
                mCurrentTarget = other.gameObject;
                mCurrentMoveDirection = (mCurrentTarget.transform.position - transform.position).normalized;
                mBehaviourState = MonsterBehaviourStates.CHASING;
            }
        }
    }

    private void ValidateTargetVisible()
    {
        if (!mSearchRadius.ObjectList.Contains(mCurrentTarget.collider))
        {
            mCurrentTarget = null;
            mBehaviourState = MonsterBehaviourStates.HUNTING;
        }
    }

    public void UpdateCandidateList(List<GameObject> candidateList)
    {
        mCandidateList = candidateList;
    }

    public void UpdateCurrentTarget(GameObject target)
    {
        mCurrentTarget = target;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        SearchRadius searchRadius = (SearchRadius)GetComponentInChildren<SearchRadius>();

        Gizmos.DrawWireSphere(transform.position, searchRadius.SearchCollider.radius);

        Gizmos.color = Color.white;

        if (mCandidateList != null)
        {
            foreach(GameObject obj in mCandidateList)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }

        Gizmos.color = Color.white;
    }

}
