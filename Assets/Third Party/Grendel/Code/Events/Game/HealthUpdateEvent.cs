using UnityEngine;
using System.Collections;

public class HealthUpdateEvent : EventBase
{
	[SerializeField]protected HealthUpdateType.HealthUpdateTypes mUpdateType;
	protected int mUpdateAmount;
	protected int mNewHealthAmount;
    protected Entity mSubject;
	
	public HealthUpdateType.HealthUpdateTypes UpdateType
	{
		get { return mUpdateType; }
	} 
	
	public int UpdateAmount
	{
		get { return mUpdateAmount; }
	}

    public Entity Subject
    {
        get { return mSubject; }
    }

    public HealthUpdateEvent(object sender, Entity subject, HealthUpdateType.HealthUpdateTypes updateType, int updateAmount, int newHealthAmount, Vector3 position) : base(position, sender)
    {   		
        mSubject = subject;
        mUpdateType = updateType;
		mUpdateAmount = updateAmount;
		mNewHealthAmount = newHealthAmount;
    }	
	
	
}
