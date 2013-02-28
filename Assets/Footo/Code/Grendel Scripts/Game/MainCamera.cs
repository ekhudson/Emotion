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
	
    // Use this for initialization
    void Start () 
    {

    }
    
    // Update is called once per frame
    void Update () 
    {
        if (NetworkManager.Instance == null && Application.isEditor && mLocalPlayer == null)
        {
            GameObject tempPlayer = (GameObject)GameObject.Instantiate(PlayerPrefab, new Vector3(0,2,0), Quaternion.identity);
            mLocalPlayer = tempPlayer.GetComponent<FootoEntity>();
            mLocalPlayer.Owner = new TNet.Player("TempPlayer");
        }

        mousPos = Input.mousePosition;
        mousPos.z = CameraOffset.y;

		mousPos = camera.ScreenToWorldPoint(mousPos);
		localPlayerPos = mLocalPlayer == null ? NetworkManager.Instance.LocalPlayerAvatar.transform.position : mLocalPlayer.transform.position;

		mousPos.y = localPlayerPos.y;
		
		Vector3 difVector = mousPos - localPlayerPos;
		Vector3 camtarget = localPlayerPos + (difVector * maxDist);

		transform.position = Vector3.Lerp(transform.position, camtarget, CameraFollowSpeed) + CameraOffset;

    }
}//end class
