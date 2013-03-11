using UnityEngine;
using System.Collections;

public class MouseAimer : MonoBehaviour
{
    private Ray mMouseRay;
    private RaycastHit mMouseRaycastHit;
    private Transform mTrans;
    public ParticleSystem AimdotParticle;

    public Vector3 HitPoint
    {
        get
        {
            return mMouseRaycastHit.point;
        }
    }

	// Use this for initialization
	void Start ()
    {
        mTrans = transform;
        AimdotParticle = (ParticleSystem)GameObject.Instantiate(AimdotParticle);
	}
	
	// Update is called once per frame
	void Update ()
    {

        mMouseRay = MainCamera.Instance.camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(mMouseRay,out mMouseRaycastHit,1000);

        AimdotParticle.transform.position = mMouseRaycastHit.point;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(mTrans.position, mMouseRaycastHit.point);
    }
}
