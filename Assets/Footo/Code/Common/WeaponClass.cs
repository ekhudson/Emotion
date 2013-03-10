using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

public class WeaponClass : ItemClass 
{
	public ProjectileClass Projectile;
	public Player Owner;
	public Transform CurrentFiringPoint; //ultimately this will just be the current point in an array of points
	public float CooldownInSeconds;
	public int BurstAmount = 1; // 1 == single shot
	public float BurstDelayBetweenShots = 0f; //Delay between shots in the burst, in seconds
	public float BurstDelayBetweenShotsDelta = 0f; //random number between -this and +this applied per shot in the burst
	public float BurstSpreadAmount = 0f; //random number between -this and +this applied to angle of shot per shot in the burst
	public int ClipSize = -1; //-1 == infinite;
	public float ReloadTimeInSeconds;
	public bool IsAutomatic; //does this weapon have automatic fire?
	public AnimationCurve AccuracyOverTime;	
	public ParticleSystem FireEffect01;
	public Transform[] FirePositions;
    public AdjustableAudioClip[] FireSounds;
    public AdjustableAudioClip[] ReloadSounds;
		
	private bool mIsFiring = false;
	private AudioSource mAudioSource;
	private int mCurrentFirePosition = 0;
    private float mCurrentReloadTime = 0;
	
	public enum WEAPON_STATES
	{
		IDLE,
		FIRING,
		COOLING,
		RELOADING,
	}	
	
	public WEAPON_STATES WeaponState;
	private float mCurrentCooldown;
	private float mCurrentClipSize;
	private float mCurrentFiringTime;
	private float mAccuracyModifier = 45;

    public float CurrentClipSize
    {
        get
        {
            return mCurrentClipSize;
        }
    }

    public float CurrentReoadTime
    {
        get
        {
            return mCurrentReloadTime;
        }
    }

	public void Awake()
	{
		mCurrentClipSize = ClipSize;
	}
	
	public void Start()
	{
		mAudioSource = gameObject.GetComponent<AudioSource>();
        mAudioSource.volume = 1;
	}
	
	public void StartFiring()
	{
		mIsFiring = true;
	}
	
	public void StopFiring()
	{
		mIsFiring = false;
	}
	
	public void Update()
	{
        if (WeaponState == WEAPON_STATES.RELOADING)
        {
            mCurrentReloadTime += Time.deltaTime;
            mIsFiring = false;
            return;
        }

        if (mIsFiring)
		{
			if (WeaponState == WEAPON_STATES.IDLE)
			{
				StartCoroutine("FireWeapon");
			}
			
			if (ClipSize > 0 && mCurrentClipSize <= 0)
			{
				StartCoroutine("Reload");
			}
			
			if (!IsAutomatic)
			{
				mIsFiring = false;
			}
			
			mCurrentFiringTime = Mathf.Clamp(mCurrentFiringTime + Time.deltaTime, 0, AccuracyOverTime.length);
		}
		else if (mCurrentFiringTime > 0)
		{
			mCurrentFiringTime = Mathf.Clamp(mCurrentFiringTime - Time.deltaTime, 0, mCurrentFiringTime);			
		}
	}	
	
	public void CreateProjectile()
	{		
		Quaternion rotation = transform.rotation;
		float range = (( ( 1 - AccuracyOverTime.Evaluate(mCurrentFiringTime) / 100)) * mAccuracyModifier) + Random.Range(-BurstSpreadAmount,BurstSpreadAmount);		
		
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y + Random.Range(-range, range), rotation.eulerAngles.z);
		TNManager.Create(Projectile.gameObject, FirePositions[mCurrentFirePosition].position, rotation, transform.forward * Projectile.ProjectileSpeedOverLife.Evaluate(0), Vector3.zero);
		FireEffect01.Emit(FireEffect01.particleCount);
		
		mCurrentFirePosition = Mathf.Clamp(mCurrentFirePosition++, 0, FirePositions.Length - 1);
		
		if (mCurrentFirePosition == FirePositions.Length - 1)
		{
			mCurrentFirePosition = 0;
		}
	}

    public void ReloadWeapon()
    {
       StartCoroutine("Reload");
    }

    void PlaySound(AdjustableAudioClip[] clipArray)
    {

        AdjustableAudioClip adjustableClip = clipArray[Random.Range(0, clipArray.Length)];

        if (clipArray.Length > 0)
        {
            GameObject tempAudioSource = new GameObject();
            tempAudioSource.AddComponent<AudioSource>();
            tempAudioSource.audio.playOnAwake = false;
            tempAudioSource.name = "[SFX] - " + adjustableClip.Clip.name;
            tempAudioSource.transform.parent = MainCamera.Instance.transform;
            tempAudioSource.transform.localPosition = Vector3.zero;
            tempAudioSource.audio.clip = adjustableClip.Clip;
            tempAudioSource.audio.pitch = adjustableClip.Pitch;
            tempAudioSource.audio.volume = 0.1f;
            tempAudioSource.audio.Play();

            Destroy(tempAudioSource, adjustableClip.Clip.length);
        }
    }
	
	IEnumerator Cooldown()
	{
		WeaponState = WEAPON_STATES.COOLING;
		yield return new WaitForSeconds(CooldownInSeconds);
		WeaponState = WEAPON_STATES.IDLE;
	}
	
	IEnumerator Reload()
	{
		WeaponState = WEAPON_STATES.RELOADING;
        mCurrentReloadTime = 0;
        PlaySound(ReloadSounds);

        yield return new WaitForSeconds(ReloadTimeInSeconds);
		mCurrentClipSize = ClipSize;
		WeaponState = WEAPON_STATES.IDLE;
	}
	
	IEnumerator FireWeapon()
	{		
		if (FirePositions.Length <= 0)
		{
			Debug.LogWarning(string.Format("Weapon {0} on Entity {1} requires at least one fire position to work", gameObject.name, transform.parent.parent.name), this);
			return false;
		}
		
		WeaponState = WEAPON_STATES.FIRING;
		
		for(int i = 0; i < BurstAmount; i++)
		{				
			CreateProjectile();
			FireEffect01.Emit(1);
            PlaySound(FireSounds);

            if (ClipSize > 0)
            {
			    mCurrentClipSize--;
            }
			
			if (ClipSize > 0 && mCurrentClipSize <= 0)
			{					
				StartCoroutine("Reload");
				return false;
			}
			
			yield return new WaitForSeconds(BurstDelayBetweenShots + (Random.Range(-BurstDelayBetweenShotsDelta, BurstDelayBetweenShotsDelta)));
		}
		
		StartCoroutine("Cooldown");
		
	}
	
}
