using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

[ActionCategory("AIEntity")]
[Tooltip("Selects a target from the AI's list of candidates, based on critera provided")]
public class AISelectTarget: FsmStateAction
{

    public AIController Controller;

    private List<GameObject> mCandidateList = new List<GameObject>();

    [UIHint(UIHint.Variable)]
    [Tooltip("Store the direction to the target")]
    public FsmVector3 StoreTargetDirection;

    public override void Reset()
    {

    }

    public override void OnEnter()
    {
        mCandidateList = Controller.CandidateList;

        GameObject testTarget = null;

        foreach(GameObject obj in mCandidateList)
        {
            testTarget = obj;
        }

        if (testTarget != null)
        {
            Controller.UpdateCurrentTarget(testTarget);
            StoreTargetDirection = (testTarget.transform.position - Controller.transform.position).normalized * Controller.OwnerEntity.MoveSpeed;
        }

        Finish();
    }
}
