using UnityEngine;
using System.Collections;
using TNet;

public class NetworkCamera : MonoBehaviour 
{
	public GameObject TargetToLookAt;
	private TNObject mTNO;	
	private Player mOwner;	

	public void ClaimCamera(Player player)
	{
		if (TNManager.isThisMyObject)
		{
			ClaimObject(player.id, transform.position);
			mTNO = gameObject.AddComponent<TNObject>();
			TargetToLookAt = transform.parent.gameObject;		
			Camera.SetupCurrent(GetComponent<Camera>());
		}
	}
	
	[RFC(2)]
	void ClaimObject (int playerID, Vector3 pos)
	{
		mOwner = TNManager.GetPlayer(playerID);
		transform.position = pos;		

		// Move the object to the Ignore Raycast layer while it's being dragged
		gameObject.layer = LayerMask.NameToLayer((mOwner != null) ? "Ignore Raycast" : "Default");
	}
	
	void Update()
	{
		mTNO.SendQuickly(3, Target.AllSaved, transform.position);
		transform.LookAt(TargetToLookAt.transform.position);
	}
			
	[RFC(3)] void MoveObject (Vector3 pos) { transform.position = pos; }	
}
