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
    public PlayMakerArrayListProxy CandidateList;
    public string Tag;

    private List<Collider> ObjectList = new List<Collider>();


    public override void Reset()
    {

    }

    public override void OnEnter()
    {
        CandidateList._arrayList.Clear();

        foreach(Collider other in SearchVolume.ObjectList)
        {
            if (other.tag != Tag)
            {
                continue;
            }

            CandidateList.Add(other, "GameObject");
        }

        Finish();
    }

}
