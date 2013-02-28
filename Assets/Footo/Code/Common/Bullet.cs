using UnityEngine;
using TNet;

public class Bullet : TNBehaviour {
	
//	Transform BmTrans;
//	Vector3 BmTarget;
//	public float BSpeed = 5f;
//	
//	void Awake()
//	{
//		BmTrans = transform;
//		BmTarget = BmTrans.position;
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{	
//		
//		BmTrans.position = BmTarget;
//		
//		
//		BmTarget = (BmTrans.position) - ((BmTrans.forward * BSpeed) * Time.deltaTime);
//		tno.SendQuickly(5, Target.OthersSaved, BmTarget);
//		
//		if (!Camera.main.rect.Contains(Camera.main.WorldToViewportPoint(BmTrans.position)))
//		{
//			TNManager.Destroy(gameObject);
//		}
//		
//		
//	}
//	
//	 void OnCollisionEnter(Collision collision) 
//	{		
//		DraggedObject_Move DO_M = collision.gameObject.GetComponent<DraggedObject_Move>();
//		
//		DO_M.UpdateHealth
//		
//       // TNManager.Destroy(gameObject);
//    }
//	
//	[RFC(5)] void MoveBulletObject (Vector3 Bpos) { BmTrans.position = Bpos; }
}
