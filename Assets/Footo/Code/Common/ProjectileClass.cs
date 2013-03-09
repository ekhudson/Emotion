using UnityEngine;
using System.Collections;
using TNet;

public class ProjectileClass : MonoBehaviour 
{    
    public AnimationCurve ProjectileSpeedOverLife= new AnimationCurve();
    public int ProjectileDamage = 1;
    public float Lifetime = 60;
    
    private Transform mTrans;
    private Vector3 mTarget;
    private float mCurrentLifetime;
    private TNObject mTNObject;
    private Rigidbody mRigidBody;
    public ParticleSystem DefaultHitEffect;

    private void Awake()
    {
        mTrans = transform;
        mTarget = mTrans.position;
        mTNObject = GetComponent<TNObject>();
        mRigidBody = rigidbody;
    }
    
    // Update is called once per frame
    private void Update () 
    {    
        mCurrentLifetime += Time.deltaTime;

        mRigidBody.velocity = mTrans.forward * ProjectileSpeedOverLife.Evaluate(mCurrentLifetime / Lifetime);
        
        //mTrans.position = mTarget;
        
        //mTarget = (mTrans.position) + ((mTrans.forward * ProjectileSpeedOverLife.Evaluate(mCurrentLifetime / Lifetime)) * Time.deltaTime);
        //mTNObject.SendQuickly(5, Target.OthersSaved, mTarget);
        
        if (mCurrentLifetime >= Lifetime)
        {
            TNManager.Destroy(gameObject);
        }        
    }
    
    private void OnCollisionEnter(Collision collision) 
    {        
        FootoEntity DO_M = collision.gameObject.GetComponent<FootoEntity>();

        if (DO_M == null)
        {
            //do nothing
        }
        else
        {
            DO_M.UpdateHealth(this, -ProjectileDamage, HealthUpdateType.HealthUpdateTypes.BallisticDamage);
        }

        DefaultHitEffect.transform.parent = null;
        DefaultHitEffect.transform.position = collision.contacts[0].point;
        DefaultHitEffect.transform.rotation = Quaternion.FromToRotation(DefaultHitEffect.transform.up, collision.contacts[0].normal);
        DefaultHitEffect.Play(true);
        Destroy(DefaultHitEffect.gameObject, DefaultHitEffect.duration);
        
        TNManager.Destroy(gameObject);
    }
    
   // [RFC(5)] void MoveBulletObject (Vector3 projectilePosition) { mTrans.position = projectilePosition; }
}
