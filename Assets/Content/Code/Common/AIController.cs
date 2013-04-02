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
