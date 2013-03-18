#define DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

/// <summary>
/// This script shows how it's possible to associate objects with players.
/// You can see it used on draggable cubes in Example 3.
/// </summary>

public class FootoEntity : TNBehaviour
{    
    Transform mTrans;
    Player mOwner = null;
    Vector3 mTarget = Vector3.zero;
    Quaternion mRotTarget;
    public float MoveSpeed = 5f;
    public float RunModifier = 2f;
    public int MaxHealth = 100;
    private int mCurrentHealth;
    public AnimationCurve JumpVelocityCurve;
    public float MinJumpTime = 0.25f;
    public float MaxJumpTime = 1f;
    public bool IsMonster;
    public float FieldOfView;
    
    public System.Collections.Generic.List<ItemAttachPoint> AttachPoints = new System.Collections.Generic.List<ItemAttachPoint>();
    
    public System.Collections.Generic.List<WeaponClass> Weapons = new System.Collections.Generic.List<WeaponClass>();

    public enum PLAYERSTATES
    {
        RUNNING,
        JUMPING,
        FALLING,
    }

    private PLAYERSTATES _state = PLAYERSTATES.RUNNING;
 
    private WeaponClass mCurrentWeapon;
    private CharacterController mController;
    private float mCurrentStateTime = 0f;
    private Vector3 mFallSpeed = Vector3.zero;
     
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
        //mTarget = mTrans.position;
        mRotTarget = mTrans.rotation;
        mController = GetComponent<CharacterController>();
    
        mCurrentHealth = MaxHealth;
    
        SetupWeapons();
    }
 
    private void SetupWeapons()
    {
        if (Weapons.Count == 0)
        {
            Debug.Log(gameObject.name + " has no weapons", this);
            return;
        }
        
        if (Weapons.Count > AttachPoints.Count)
        {
            Debug.Log(string.Format("Entity {0} have more items than attach points", gameObject.name), this);
        }
        
        for(int i = 0; i < Weapons.Count; i++)
        {
            foreach(ItemAttachPoint attachPoint in AttachPoints)
            {
                if (attachPoint.AttachedItem == null)
                {
                    GameObject weaponTemp = (GameObject)GameObject.Instantiate(Weapons[i].gameObject, attachPoint.PhysicalPoint.position, attachPoint.PhysicalPoint.rotation);
                    weaponTemp.transform.parent = attachPoint.PhysicalPoint;
                    Weapons[i] = weaponTemp.GetComponent<WeaponClass>();
                    attachPoint.AttachedItem = Weapons[i];
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    
        mCurrentWeapon = Weapons[0];
    }

    void Update ()
    {

        if(mOwner != null && Camera.main != null)
        {
            if (!IsMonster)
            {
                if (TNManager.playerID == mOwner.id)
                {
        
                    Ray ray = MainCamera.Instance.camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if(Physics.Raycast(ray,out hit,1000))
                    {
                        Vector3 dif = hit.point - mTrans.position;
                        dif.y = 0;
    
                        mRotTarget = Quaternion.LookRotation(dif,Vector3.up);
                    }

                    if(Input.GetKey(KeyCode.A))
                    {
                        mTarget += (new Vector3(-1 , 0 , 0));
                    }
            
                    if(Input.GetKey(KeyCode.D))
                    {
                        mTarget += (new Vector3(1, 0, 0));
                    }
            
                    if(Input.GetKey(KeyCode.W))
                    {
                        mTarget += (new Vector3(0, 0 , 1));
                    }
            
                    if(Input.GetKey(KeyCode.S))
                    { 
                        mTarget += (new Vector3(0, 0 , -1));
                    }
    
                    if(Input.GetKeyDown(KeyCode.R))
                    {
                        mCurrentWeapon.ReloadWeapon();
                    }
            
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (Input.GetKey(KeyCode.LeftShift)){ return; }
                
                        mCurrentWeapon.CurrentFiringPoint = transform;
                        mCurrentWeapon.StartFiring();
                    }
            
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (Input.GetKey(KeyCode.LeftShift)){ return; }
            
                        mCurrentWeapon.StopFiring();
                    }
            
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (Input.GetKey(KeyCode.LeftShift)){ return; }
        
                        if (Weapons.Count >= 2)
                        {
                            Weapons[1].CurrentFiringPoint = transform;
                            Weapons[1].StartFiring();
                        }
                    }
                    
                    if (Input.GetMouseButtonUp(1))
                    {
                        if (Input.GetKey(KeyCode.LeftShift)){ return; }
            
                        if (Weapons.Count >= 2)
                        {
                            Weapons[1].StopFiring();
                        }
                    }
                
                    if (renderer.enabled == false && Input.GetKeyDown(KeyCode.Space))
                    {
                        renderer.enabled = true;
                        mCurrentHealth = MaxHealth;
                    }
        
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        mTarget *= RunModifier;
                        mCurrentWeapon.StopFiring();
        
                        if (Weapons.Count > 1)
                        {
                            Weapons[1].StopFiring();
                        }
                    }
                }

                switch(_state)
                {
                    case PLAYERSTATES.RUNNING:
        
                        if (Input.GetKeyDown(KeyCode.Space) && mController.isGrounded)
                        {
                            SetState(PLAYERSTATES.JUMPING);
                        }
                        else if (!mController.isGrounded)
                        {
                            SetState(PLAYERSTATES.FALLING);
                        }
    
                    break;
    
                    case PLAYERSTATES.JUMPING:
    
                        if (mCurrentStateTime < MinJumpTime)
                        {
                            mFallSpeed.y = JumpVelocityCurve.Evaluate(mCurrentStateTime);
                        }
                        else if (Input.GetKey(KeyCode.Space) && mCurrentStateTime >= MaxJumpTime && !mController.isGrounded)
                        {
                            SetState(PLAYERSTATES.FALLING);
                        }
                        else if (!Input.GetKey(KeyCode.Space) && mCurrentStateTime > MinJumpTime)
                        {
                            SetState(PLAYERSTATES.FALLING);
                        }
                        else if (mController.isGrounded)
                        {
                            SetState(PLAYERSTATES.RUNNING);
                        }
    
                    break;
    
                    case PLAYERSTATES.FALLING:
        
                        if (!mController.isGrounded)
                        {
                            mFallSpeed += Physics.gravity * Time.deltaTime;
    
                        }
                        else if (mController.isGrounded)
                        {
                            SetState(PLAYERSTATES.RUNNING);
                        }

                    break;

                }

                mTarget += mFallSpeed;
    
                mCurrentStateTime += Time.deltaTime;
        
                tno.SendQuickly(3, Target.OthersSaved, mTrans.position);
                tno.SendQuickly(4, Target.OthersSaved, mRotTarget);

            }
        }

        UpdatePosition();

    }

    public void UpdatePosition()
    {
        mController.Move( (mTarget * MoveSpeed) * Time.deltaTime);
        mTrans.rotation = Quaternion.Lerp(mTrans.rotation, mRotTarget, Time.deltaTime * 20f);

        mTarget = Vector3.zero;
    }

    public void SetState(PLAYERSTATES newState)
    {
        PLAYERSTATES stateToUpdateTo = newState;

        switch(newState)
        {
            case PLAYERSTATES.RUNNING:
            
                switch(_state)
                {
                    case PLAYERSTATES.RUNNING:
                    return;
    
                    case PLAYERSTATES.JUMPING:
                    break;
                
                    case PLAYERSTATES.FALLING:
                    break;
    
                    default:

                    _state = PLAYERSTATES.RUNNING;

                    break;
                }

            break;

            case PLAYERSTATES.JUMPING:
            
                switch(_state)
                {
                    case PLAYERSTATES.RUNNING:

                    break;
    
                    case PLAYERSTATES.JUMPING:
                    return;
    
                    case PLAYERSTATES.FALLING:

                    break;
    
                    default:

                    break;
                }




            break;
            
            case PLAYERSTATES.FALLING:

                switch(_state)
                {
                    case PLAYERSTATES.RUNNING:

                    break;

                    case PLAYERSTATES.JUMPING:

                        if (mCurrentStateTime < MinJumpTime)
                        {
                            return;
                        }

                    break;

                    case PLAYERSTATES.FALLING:
                    return;

                    default:

                    break;
                }

            break;
            default:
                
                stateToUpdateTo = PLAYERSTATES.RUNNING;
            
            break;
        }

        if (_state == stateToUpdateTo)
        {
            return;
        }

        switch(stateToUpdateTo)
        {
            case PLAYERSTATES.RUNNING:
                //EventManager.Instance.Post(new PlayerStateChangeRun(this, this._state, PLAYERSTATES.RUNNING));
                break;
            case PLAYERSTATES.JUMPING:
                //EventManager.Instance.Post(new PlayerStateChangeJump(this, this._state, PLAYERSTATES.JUMPING));
                break;
            case PLAYERSTATES.FALLING:
                //EventManager.Instance.Post(new PlayerStateChangeFall(this, this._state, PLAYERSTATES.FALLING));
                break;
            default:
                //Console.Instance.OutputToConsole(this, string.Format("Attempted to change {0} to state {1}, but that state was invalid.", name, stateToUpdateTo), Console.Instance.Style_Error);
                return;
        }
        mCurrentStateTime = 0f;
        _state = stateToUpdateTo;
    }


//DEBUG
    private void DrawWeaponInfo()
    {
        if ((NetworkManager.Instance != null && TNManager.player != mOwner) || mCurrentWeapon == null)
        {
            return;
        }

        float kHeight = 64;
        float kWidth = 128;

        GUILayout.BeginArea(new Rect(0, Screen.height - kHeight, Screen.width, kHeight));

        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

                GUILayout.Label(string.Format("Current Weapons: {0} | {1} / {2}", mCurrentWeapon.name, mCurrentWeapon.CurrentClipSize, mCurrentWeapon.ClipSize));

                if (mCurrentWeapon.WeaponState == WeaponClass.WEAPON_STATES.RELOADING)
                {
                    GUILayout.Box("Reloading", GUILayout.Width(kWidth));


                    Rect rect = GUILayoutUtility.GetLastRect();

                    rect.width = kWidth * (mCurrentWeapon.CurrentReoadTime / mCurrentWeapon.ReloadTimeInSeconds);

                    GUI.color = Color.yellow;
                    GUI.Box(rect, string.Empty);
                    GUI.color = Color.white;
                }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndArea();
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
    public void UpdateHealth(object sender, int updateValue, HealthUpdateType.HealthUpdateTypes type)
    {       
        mCurrentHealth = Mathf.Clamp(mCurrentHealth + updateValue, 0, MaxHealth);
    
        EventManager.Instance.Post(new HealthUpdateEvent(sender, type, updateValue, mCurrentHealth, transform.position));
    
        if (mCurrentHealth <= 0)
        {
            KillEntity();
        }
    }

    public void MoveEntity(Vector3 direction)
    {
        mTarget += direction;
    }
 
    public void KillEntity()
    {

    }

    void OnGUI()
    {
        if (MainCamera.Instance == null)
        {
            return;
        }

        Vector3 labelPos = MainCamera.Instance.camera.WorldToScreenPoint(transform.position);
        labelPos.y = Screen.height - labelPos.y;
        labelPos.y -= 72f;
        GUI.Box(new Rect(labelPos.x - 64, labelPos.y, 128, 24), mCurrentHealth.ToString());

        DrawWeaponInfo();
    }

    /// <summary>
    /// Save the target position.
    /// </summary>
    
    [RFC(3)] void MoveObject (Vector3 pos) { mTrans.position = pos; }
    [RFC(4)] void RotateObject (Quaternion rot) { mRotTarget = rot; }
    [RFC(5)] void UpdateHealthOverNetwork(int health) { mCurrentHealth = health; }
 
}