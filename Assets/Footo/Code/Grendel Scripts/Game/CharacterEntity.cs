using UnityEngine;
using System.Collections;

//A rigidbody based character controller for Grendel Entities
public class CharacterEntity : Entity
{
    public float SkinWidth = 0.01f;
    public float StepOffset = 0.35f;
    public float MinMoveAmount = 0f;
    private Vector3 mCurrentMove = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }
    
    // Update is called once per frame
    private void FixedUpdate ()
    {
        if(IsGrounded())
        {
            mRigidbody.isKinematic = true;
        }
        else
        {
            mRigidbody.isKinematic = false;
        }

         mTransform.Translate(mCurrentMove, Space.World);
    }

    public void Move(Vector3 amount)
    {
        amount.x = Mathf.Round(amount.x);
        amount.y = Mathf.Round(amount.y);
        amount.z = Mathf.Round(amount.z);

        mCurrentMove = amount;
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(mTransform.position - new Vector3(0, mCollider.bounds.size.y * 0.5f, 0), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, SkinWidth))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        Debug.DrawRay(mTransform.position - new Vector3(0, mCollider.bounds.size.y * 0.5f, 0), Vector3.down, Color.yellow, SkinWidth);
    }
}
