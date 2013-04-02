using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

[ActionCategory("AIEntity")]
[Tooltip("Finds any Objects in the Entity's Search Radius with the tag, outputs an array")]
public class AIFindObjectsByTag : FsmStateAction
{

    public AIController Controller;
    public SearchRadius SearchVolume;
    [UIHint(UIHint.Variable)]
    [Tooltip("Tag to search for")]
    public string Tag;
    [Tooltip("Use this AI's Field of View")]
    public bool UseFieldOfView;
    [Tooltip("Check line of sight to objects")]
    public bool CheckLineOfSight;
    private List<GameObject> mCandidateList = new List<GameObject>();

    private List<Collider> ObjectList = new List<Collider>();

    public override void Reset()
    {

    }

    public override void OnEnter()
    {
        mCandidateList.Clear();

        RaycastHit hit;
        Vector3 targetDirection;

        foreach(Collider other in SearchVolume.ObjectList)
        {
            if (other == null || other.tag != Tag)
            {
                continue;
            }

            targetDirection = other.transform.position - Controller.transform.position;

            //Field of View Check
            if (UseFieldOfView)
            {
                if (Vector3.Angle(targetDirection, Controller.transform.forward) > Controller.OwnerEntity.FieldOfView * 0.5f)
                {
                    continue;
                }
            }

            //Line of Sight Check
            if (CheckLineOfSight)
            {
                if (Physics.Raycast(new Ray(Controller.transform.position, targetDirection), out hit, SearchVolume.SearchCollider.radius))
                {
                    if (hit.transform.tag != Tag)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            mCandidateList.Add(other.gameObject);
        }

        if (mCandidateList.Count > 0)
        {
            Controller.UpdateCandidateList(mCandidateList);
            Fsm.Event("OBJECTSFOUND");
            return;
        }

        Finish();
    }
}
