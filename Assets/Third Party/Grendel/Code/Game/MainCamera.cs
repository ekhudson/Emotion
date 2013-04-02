using UnityEngine;
using System.Collections;

public class MainCamera : Singleton<MainCamera>
{
    
    public Vector3 CameraOffset = new Vector3(0, 10, 0);
    public float CameraFollowSpeed = 5f;
    public GameObject PlayerPrefab;

	public float CameraFollowDist = 2f;
	
	Vector3 mousPos;
    Vector3 localPlayerPos;
	public float maxDist = 7f;

    private FootoEntity mLocalPlayer;

    protected override void Awake()
    {
        if (mInstance != null)
        {
            mInstance.transform.position = transform.position;
            mInstance.transform.rotation = transform.rotation;
        }

        base.Awake();
    }

    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
    {
        if (Application.loadedLevelName == "MainMenu")
        {
            return;
        }

        if (NetworkManager.Instance == null && Application.isEditor && mLocalPlayer == null)
        {
            GameObject tempPlayer = (GameObject)GameObject.Instantiate(PlayerPrefab, new Vector3(0,2,0), Quaternion.identity);
            mLocalPlayer = tempPlayer.GetComponent<FootoEntity>();
            mLocalPlayer.Owner = new TNet.Player("TempPlayer");

            if (mLocalPlayer == null)
            {
                return;
            }
        }

        mousPos = Input.mousePosition;
        mousPos.z = CameraOffset.y;

		mousPos = camera.ScreenToWorldPoint(mousPos);

        try
        {
		    localPlayerPos = mLocalPlayer == null ? NetworkManager.Instance.LocalPlayerAvatar.transform.position : mLocalPlayer.transform.position;
        }
        catch
        {
            return;
        }

		mousPos.y = localPlayerPos.y;
		
		Vector3 difVector = localPlayerPos - mousPos;
		Vector3 camtarget = localPlayerPos + (difVector * maxDist);

		transform.position = Vector3.Lerp(transform.position, camtarget, CameraFollowSpeed) + CameraOffset;
        transform.LookAt(localPlayerPos);

    }
}//end class
