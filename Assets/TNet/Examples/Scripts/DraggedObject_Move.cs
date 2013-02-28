//------------------------------------------
//            Tasharen Network
// Copyright © 2012 Tasharen Entertainment
//------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

/// <summary>
/// This script shows how it's possible to associate objects with players.
/// You can see it used on draggable cubes in Example 3.
/// </summary>

public class DraggedObject_Move : TNBehaviour
{
	Transform mTrans;
	Player mOwner = null;
	Vector3 mTarget;
	Quaternion mRotTarget;
	public float Speed = 5f;
	public int MaxHealth = 100;
	private int mCurrentHealth;	
	
	
	public System.Collections.Generic.List<WeaponClass> Weapons = new System.Collections.Generic.List<WeaponClass>();
	
	public UISlider healthBar;
	public GameObject loseScreen;
	
	private WeaponClass mCurrentWeapon;
		
	public Player Owner
	{
		get
		{
			return mOwner;
		}
		set
		{
			mOwner = value;
		}			
	}
	
	public int Health
	{
		get
		{
			return mCurrentHealth;
		}
	}

	void Awake ()
	{
		mTrans = transform;
		mTarget = mTrans.position;
		mRotTarget = mTrans.rotation;
		
		mCurrentHealth = MaxHealth;
		
		SetupWeapons();
		
	}
	
	private void SetupWeapons()
	{
		if (Weapons.Count == 0)
		{
			Debug.Log(gameObject.name + " has no weapons", this);
		}
		
		for(int i = 0; i < Weapons.Count; i++)
		{
			GameObject weaponTemp = (GameObject)GameObject.Instantiate(Weapons[i].gameObject, transform.position, Quaternion.identity);
			weaponTemp.transform.parent = transform;
			Weapons[i] = weaponTemp.GetComponent<WeaponClass>();	
		}
		
		mCurrentWeapon = Weapons[0];
	}

	void Update ()
	{
		mTrans.position = Vector3.Lerp(mTrans.position, mTarget, Time.deltaTime * 20f);
		mTrans.rotation = Quaternion.Lerp(mTrans.rotation, mRotTarget, Time.deltaTime * 20f);
				
		if(mOwner != null)
		{
			if (TNManager.playerID == mOwner.id)
			{			
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if(Physics.Raycast(ray,out hit,100))
				{
					Vector3 dif = mTarget - hit.point;
					dif.y = 0;
					
					mRotTarget = Quaternion.LookRotation(dif,Vector3.up);
					
				}
				
				if(Input.GetKey(KeyCode.A))
				{
					mTarget += (new Vector3(-1 , 0 , 0) * Speed ) * Time.deltaTime;					
				}
				
				if(Input.GetKey(KeyCode.D))
				{
					mTarget += (new Vector3(1, 0, 0) * Speed) * Time.deltaTime;					
				}
				
				if(Input.GetKey(KeyCode.W))
				{
					mTarget += (new Vector3(0, 0 , 1) * Speed) * Time.deltaTime;					
				}
				
				if(Input.GetKey(KeyCode.S))
				{ 
					mTarget += (new Vector3(0, 0 , -1) * Speed) * Time.deltaTime;					
				}
				
				if (Input.GetMouseButtonDown(0))
				{					
					mCurrentWeapon.CurrentFiringPoint = transform;
					mCurrentWeapon.StartFiring();
				}
				
				if (Input.GetMouseButtonUp(0))
				{
					mCurrentWeapon.StopFiring();
				}				
				
				if (renderer.enabled == false && Input.GetKeyDown(KeyCode.Space))
				{
					renderer.enabled = true;
					mCurrentHealth = MaxHealth;
					NGUITools.SetActive(loseScreen,false);
				}
				
				healthBar.sliderValue = (float)mCurrentHealth / (float)MaxHealth;
				
				//mTarget = transform.position;
				
				tno.SendQuickly(3, Target.OthersSaved, mTarget);
				tno.SendQuickly(4, Target.OthersSaved, mRotTarget);
			}
		}	
		
	}
	/// <summary>
	/// Remember the last player who claimed control of this object.
	/// </summary>

	[RFC(2)]
	public void ClaimObject (int playerID, Vector3 pos)
	{
		mOwner = TNManager.GetPlayer(playerID);
		mTrans.position = pos;
		mTarget = pos;

		// Move the object to the Ignore Raycast layer while it's being dragged
		gameObject.layer = LayerMask.NameToLayer((mOwner != null) ? "Ignore Raycast" : "Default");		
	}
	
	/// <summary>
	/// Updates the health. Pass negative values to "damage" the entity
	/// </summary>
	/// <param name='updateValue'>
	/// Update value.
	/// </param>
	public void UpdateHealth(int updateValue)
	{
		mCurrentHealth = Mathf.Clamp(mCurrentHealth + updateValue, 0, MaxHealth);
		
		if (mCurrentHealth <= 0)
		{
			KillEntity();
		}
	}
	
	public void KillEntity()
	{
		NGUITools.SetActive(loseScreen,true);
		renderer.enabled = false;
	}	
	
	void OnGUI()
	{
		Vector3 labelPos = MainCamera.Instance.camera.WorldToScreenPoint(transform.position);
		labelPos.y = Screen.height - labelPos.y;
		labelPos.y -= 72f;
		GUI.Box(new Rect(labelPos.x - 64, labelPos.y, 128, 24), mCurrentHealth.ToString());
	}

	/// <summary>
	/// Save the target position.
	/// </summary>

	[RFC(3)] void MoveObject (Vector3 pos) { mTarget = pos; }
	[RFC(4)] void RotateObject (Quaternion rot) { mRotTarget = rot; }
	[RFC(5)] void UpdateHealthOverNetwork(int health) { mCurrentHealth = health; }
	
}