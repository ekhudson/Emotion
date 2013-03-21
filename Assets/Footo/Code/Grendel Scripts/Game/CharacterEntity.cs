using UnityEngine;
using System.Collections;

//A rigidbody based character controller for Grendel Entities
public class CharacterEntity : Entity
{
    public bool NonPlayerCharacter = false; //NPC's DO NOT register for user input events
    public float StepOffset = 0.35f;
    public float MinMoveAmount = 0f;
    private Vector3 mCurrentMove = Vector3.zero;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }
    
    // Update is called once per frame
    void FixedUpdate ()
    {
        mRigidbody.AddForce(mCurrentMove, ForceMode.Impulse);
    }
}
