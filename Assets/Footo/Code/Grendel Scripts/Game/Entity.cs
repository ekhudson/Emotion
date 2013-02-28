using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Jan 15, 2012
	/// 
	/// Filename: Entity.cs
	/// 
	/// Summary: Extends Baseobject. Provides information that is typically
	/// shared across a variety of ingame entities, such as health, movement,
	/// and life/death events. Basically, anything that may influence final
	/// entity logic, but is not specific logic itself... if that makes sense
	/// 
	/// </summary>
	
public class Entity : BaseObject {
	
	//PUBLIC VARIABLES
	public bool Managed = true; //is this Entity managed? Entities cannot update themselves, so checking this off requires a custom way to update the entity thereafter
	public bool DestroyedOnDeath = true; //is this Entity destroyed when it dies?
	public FactionManager.Factions Faction; //this Entity's Faction
	public int Health = 100; //the Entity's starting health	
	
	//PROTECTED VARIABLES
	protected int _currentHealth; //the entity's current health	
	protected bool _toBeKilled = false; //this is marked true when the entity is ready to be cleared out by the Manager	
	protected SearchRadius _searchRadius; //this entities SearchRadius
	protected List<Entity> _nearbyAllies; //allies within the SearchRadius
	protected List<Entity> _nearbyEnemies; //enemies within the SearchRadius;
	protected UISprite mSprite;
	
	//PRIVATE VARIABLES
	private Entity _testEntity; //temp Entity used for sorting SearchRadius list
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected override void Awake ()
	{		
		base.Awake();		
		mSprite = GetComponent<UISprite>();
	}

    public UISprite Sprite
    {
        get
        {
            return mSprite;
        }
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
	protected override void Start () 
	{			
		base.Start();
		
		if (Managed)
		{ 			
			EntityManager.AddToDictionary(_instanceID, this);
			EntityManager.AddToUpdateList(this);
		}
		
		_currentHealth = Health;
		_searchRadius = GetComponentInChildren<SearchRadius>();
		
		OnSpawn(); 
	}	
	
	virtual public int TakeDamage(int amount)
	{		
		int dmg = ModifyHealth(-amount);
		
		if (_currentHealth <= 0)
		{			
			KillEntity();
		}
		
		return dmg;
	}	

	virtual public void KillEntity()
	{
		OnDeath();
				
		if (Managed){EntityManager.RemoveFromDictionary(_instanceID); } //remove this Entity from the Manager
		if (DestroyedOnDeath) { Destroy(_gameObject); } //destroy the gameObject, if specified		
	}
	
	virtual public void CalledUpdate()
	{
		//this update function is called by the Manager
		if (_searchRadius)
		{
			SortSearchRadiusList();
		}
		
		//mSprite.transform.localPosition = transform.position;
	}
		
	/// <summary>
	/// ModifyHealth
	/// 
	/// Accepts a single int to modify current health
	/// and returns the result. NOTE: modification is by
	/// addition, so lowering health requires a negative
	/// number
	/// </summary>
	/// <param name="amount">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>		
	virtual public int ModifyHealth(int amount)
	{		
		return _currentHealth += amount;		
	}
	
	void SortSearchRadiusList()
	{
		foreach(Collider collider in _searchRadius.ObjectList)
		{
			_testEntity = EntityManager.EntityDictionary[ collider.GetInstanceID() ];
			if(_testEntity.Faction == this.Faction)
			{
				_nearbyAllies.Add(_testEntity);
			}
			else
			{
				_nearbyEnemies.Add(_testEntity);
			}
		}
	}
	
	/// <summary>
	/// Raises the death event.
	/// </summary>
	virtual public void OnDeath()
	{
		
	}
	
	/// <summary>
	/// Raises the spawn event.
	/// </summary>
	virtual public void OnSpawn()
	{
		
	}
	
	/// <summary>
	/// Raises the damage event.
	/// </summary>
	/// <param name='amount'>
	/// Amount.
	/// </param>
	virtual public void OnDamage(int amount)
	{
		
	}	
	
	//ACCESSORS
	public bool ToBeKilled
	{
		get{return _toBeKilled;}
		set{_toBeKilled = value;}
	}
}
