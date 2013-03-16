using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    private FootoEntity mEntity;

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

    private void Start()
    {
        mEntity = GetComponent<FootoEntity>();
        mSearchRadius = GetComponentInChildren<SearchRadius>();
    }

    private void Update()
    {
//        switch(mBehaviourState)
//        {
//            case MonsterBehaviourStates.IDLE:
//
//                LookForEntitiesByTag("Player");
//
//            break;
//
//            case MonsterBehaviourStates.HUNTING:
//
//                mEntity.MoveEntity(mCurrentMoveDirection);
//                mCurrentMoveDirection = (mTargetLocation - transform.position).normalized;
//
//            break;
//
//            case MonsterBehaviourStates.CHASING:
//
//                mEntity.MoveEntity(mCurrentMoveDirection);
//                mTargetLocation = mCurrentTarget.transform.position;
//                mCurrentMoveDirection = (mTargetLocation - transform.position).normalized;
//
//            break;
//        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        SearchRadius searchRadius = (SearchRadius)GetComponentInChildren<SearchRadius>();

        Gizmos.DrawWireSphere(transform.position, searchRadius.SearchCollider.radius);

        Gizmos.color = Color.white;

    }

}
