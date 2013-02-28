using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TNet;

public class BoxManager : MonoBehaviour {
    
    public System.Collections.Generic.List<BoxData> BoxList = new System.Collections.Generic.List<BoxData>();
    private static BoxManager mInstance;
    private bool mHostAssigned = false;
    public GameObject PlayerPrefab;
    
    [System.Serializable]
    public class BoxData
    {
        public FootoEntity Box;
        [SerializeField]public Player Owner = null;        
        
        public BoxData(FootoEntity box, Player owner)
        {
            Box = box;            
            Owner = owner;
        }        
    }
    
    public static BoxManager Instance
    {
        get
        {
            return mInstance;
        }
    }
    
    void Start()
    {
        mInstance = this;
    }
    
    public GameObject GetBoxOfPlayer(Player p)
    {
        if (TNManager.player == p && !mHostAssigned)
        {
            //AssignBoxToPlayer(TNManager.player);
        }
        
        foreach(BoxData boxData in BoxList)
        {
            if (boxData.Box.Owner == p)
            {
                return boxData.Box.gameObject;
            }
        }        
        
        return null;
    }

    public void CreateAvatarForPlayer(Player p)
    {
        if (p == null)
        {
            Debug.Log ("Attemped to create avatar for null player");
            return;
        }
        else
        {
            TNManager.client.onCreate += this.OnCreatObject;
            TNManager.Create(PlayerPrefab, new Vector3(0,2,0), Quaternion.identity);
        }
    }

    public void OnCreatObject(int creator, int index, uint objectID, BinaryReader reader)
    {
        Debug.Log(string.Format("Creating avatar for {0}", TNManager.GetPlayer(creator).name));

        TNObject obj = TNObject.Find(objectID);
        FootoEntity avatar = obj.GetComponent<FootoEntity>();

        if (avatar == null)
        {
            return;
        }

        if (creator == TNManager.playerID)
        {
            avatar.Owner = TNManager.player;
             //Call the claim function directly in order to make it feel more responsive
            avatar.ClaimObject(TNManager.playerID, avatar.transform.position);
            avatar.GetComponent<TNObject>().Send(2, Target.OthersSaved, TNManager.playerID, avatar.transform.position);
            NetworkManager.Instance.LocalPlayerAvatar = avatar.gameObject;
            TNManager.client.onCreate -= this.OnCreatObject;
        }
    }
}
