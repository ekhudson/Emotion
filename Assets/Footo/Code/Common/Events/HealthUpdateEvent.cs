using UnityEngine;
using System.Collections;

public class HealthUpdateEvent : EventBase
{
	[SerializeField]protected HealthUpdateType.HealthUpdateTypes mUpdateType;
	protected int mUpdateAmount;
	protected int mNewHealthAmount;
	
	public HealthUpdateType.HealthUpdateTypes UpdateType
	{
		get { return mUpdateType; }
	} 
	
	public int UpdateAmount
	{
		get { return mUpdateAmount; }
	}	

    public HealthUpdateEvent(object sender, HealthUpdateType.HealthUpdateTypes updateType, int updateAmount, int newHealthAmount, Vector3 position) : base(position, sender)
    {   		
		mUpdateType = updateType;
		mUpdateAmount = updateAmount;
		mNewHealthAmount = newHealthAmount;
    }	
	
	
}
