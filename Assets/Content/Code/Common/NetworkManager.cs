using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TNet;

public class NetworkManager : MonoBehaviour 
{
    [System.Serializable]
    public class PlayerData
    {
        public Player PlayerEntry;
        public Color PlayerColor = Color.white;
        public FootoEntity PlayerAvatar;

        public PlayerData(Player player, Color color, FootoEntity avatar)
        {
            PlayerEntry = player;
            PlayerColor = color;
            PlayerAvatar = avatar;
        }
    }    
    
    [System.Serializable]
    public class ChannelData
    {
        public int channelID;        
        public int playerCount;        
        public bool password;        
        public bool isPersistent;    
        public string level;        
    }
    
    public Player CurrentHost;
    
    public System.Collections.Generic.List<ChannelData> ChannelList = new System.Collections.Generic.List<ChannelData>();
    
    private TNObject mTNObject;
    private static NetworkManager mInstance;
    private GameObject mLocalPlayerAvatar;
    
    public System.Collections.Generic.List<Player> CurrentPlayers
    {
        get
        {
            System.Collections.Generic.List<Player> players = new System.Collections.Generic.List<Player>();
            
            players.Add(CurrentHost);
            
            foreach(Player player in TNManager.players)
            {
                players.Add(player);
            }
            
            return players;
        }
    }
    
    public string ServerName
    {
        get
        {
            if (TNServerInstance.gameServer != null)
            {
                return TNServerInstance.gameServer.name;
            }
            else
            {
                return null;
            }
        }
    }

    void OnNetworkJoinChannel(bool success, string message)
    {
        Debug.Log("Join");
        if (success)
        {
            StartCoroutine("WaitToSpawn");
        }
        else
        {
            Debug.Log(message);
        }
    }

    IEnumerator WaitToSpawn()
    {
        Debug.Log(string.Format("Player {0} has joined, waiting to spawn.", TNManager.player.name));
        yield return new WaitForSeconds(1f);
        BoxManager.Instance.CreateAvatarForPlayer(TNManager.player);
    }
    
    public static NetworkManager Instance
    {
        get
        {
            return mInstance;
        }
    }
    
    public GameObject LocalPlayerAvatar
    {
        get
        {
            if (mLocalPlayerAvatar == null)
            {
                return null;
            }
            
            return mLocalPlayerAvatar;
        }
        set
        {
            mLocalPlayerAvatar = value;
        }
    }
    
    public string HostName
    {
        get
        {            
            return CurrentHost.name + "(Host)";
        }
    }
    
        
    void Awake()
    {
        if (mInstance != null)
        {
            Destroy(gameObject);
            return;
        }        
        
        mInstance = this;
        TNManager.playerName = System.Environment.UserName;        
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TNManager.client.packetHandlers[(byte)Packet.ResponseChannelList] = OnChannelList;
        mTNObject = GetComponent<TNObject>();
    }
    
    void Update()
    {
        if (TNManager.isHosting && CurrentHost != TNManager.player)
        {
            CurrentHost = TNManager.player;
            mTNObject.Send(1, Target.AllSaved, CurrentHost);
        }
    }
    
    void OnGUI()
    {
        if (!Input.GetKey(KeyCode.BackQuote))
        {
            return;
        }

        if (TNManager.isHosting && TNServerInstance.gameServer == null)
        {
            return;
        }
        
        GUILayout.BeginArea(new Rect(0,0, Screen.width, 32), GUI.skin.textArea);
        GUILayout.BeginHorizontal();

        GUILayout.Label(string.Format("Server: {0}", ServerName), GUILayout.Width(128));
        
        if (TNManager.isHosting)
        {
            GUI.color = Color.yellow;                
            
            GUILayout.Box("You are host " + TNManager.player.name, GUILayout.Width(128));
            GUI.color = Color.white;
        }
        else
        {                
            GUI.color = Color.yellow;
            GUILayout.Box("Host: " + CurrentHost.name,GUILayout.Width(128));
            GUI.color = Color.white;
        }        
        
        if (TNManager.players.size > 0)
        {
            foreach(Player player in TNManager.players)
            {                
                if(player == TNManager.player)
                {
                    player.name = System.Environment.UserName;
                }
                
                GUILayout.Box(string.Format("{0}({1})",player.name,player.id), GUILayout.Width(128));            
            }
        }
        else
        {            
            GUILayout.Box("No Players Connected", GUILayout.Width(128));
                        
            TNManager.client.BeginSend(Packet.RequestChannelList);
            TNManager.client.EndSend();
            
            foreach(ChannelData channel in ChannelList)
            {
               GUILayout.Button(string.Format("Channel {0} ({1}) [{2} Player(s)]", channel.channelID.ToString(), channel.level, channel.playerCount.ToString()), GUILayout.Width(256));
//                if(GUILayout.Button(string.Format("Join Channel {0} ({1}) [x{2} Players]", channel.channelID.ToString(), channel.level, channel.playerCount.ToString()), GUILayout.Width(256)))
//                {
//                    if (TNManager.isHosting)
//                    {
//                        BoxManager.Instance.AssignBoxToPlayer(TNManager.player);
//                    }
//                    else
//                    {
//                        TNManager.JoinChannel(channel.channelID, channel.level);
//                    }
//                }
            }
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        
        DrawPlayerLabels();
    }
    
    private void DrawPlayerLabels()
    {
        DrawLabelForPlayer(CurrentHost);
        
        foreach(Player player in TNManager.players)
        {
            DrawLabelForPlayer(player);
        }
    }
    
    public void DrawLabelForPlayer(Player p)
    {    
        GameObject playerAvatar = BoxManager.Instance.GetBoxOfPlayer(p);
        
        if (playerAvatar == null)
        {
            return;    
        }
        
        Vector3 labelPos = MainCamera.Instance.camera.WorldToScreenPoint(playerAvatar.transform.position);
        labelPos.y = Screen.height - labelPos.y;
        labelPos.y -= 40f;
        GUI.Box(new Rect(labelPos.x - 64, labelPos.y, 128, 24), CurrentHost == p ? HostName : p.name);
    }
    
    void OnChannelList (Packet response, BinaryReader reader, IPEndPoint source)
    {
        ChannelList.Clear();
        int count = reader.ReadInt32();
    
        for (int i = 0; i < count; ++i)
        {
            int channelID        = reader.ReadInt32();
            int playerCount        = reader.ReadInt32();
            bool password        = reader.ReadBoolean();
            bool isPersistent    = reader.ReadBoolean();
            string level        = reader.ReadString();    
            
            ChannelData tempData = new ChannelData();
            tempData.channelID = channelID;
            tempData.playerCount = playerCount;
            tempData.password = password;
            tempData.isPersistent = isPersistent;
            tempData.level = level;
            
            ChannelList.Add(tempData);            
            // Do something with this information -- add it to a list perhaps? Whatever you need.
        }
    }
                
    [RFC(1)]
    void UpdateHost(Player player) { CurrentHost = player; }
    

    
}
