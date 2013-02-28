//------------------------------------------
//            Tasharen Network
// Copyright © 2012 Tasharen Entertainment
//------------------------------------------

using UnityEngine;
using TNet;
using System.IO;
using System.Collections;

/// <summary>
/// This script provides a main menu for all examples.
/// The menu is created in Unity's built-in Immediate Mode GUI system.
/// The menu makes use of the following TNet functions:
/// - TNManager.Connect
/// - TNManager.JoinChannel
/// - TNManager.LeaveChannel
/// - TNManager.Disconnect
/// - TNServerInstance.Start
/// - TNServerInstance.Stop
/// </summary>

[ExecuteInEditMode]
public class ExampleMenu : MonoBehaviour
{
	const float buttonWidth = 150f;
	const float buttonHeight = 40f;

	public int serverTcpPort = 5127;
	public int discoveryPort = 5129;
	public string mainMenu = "Example Menu";
	public string[] examples;
	public GUIStyle button;
	public GUIStyle text;
	public GUIStyle input;
	
	public UIInput uii_serverName;
	public UIInput uii_addressName;
	public UILabel uil_StartServer;
	public UISprite uis_StartServer;
	public UILabel uil_message;
	
	public GameObject go_ServerListbtn;
	public GameObject go_ServerListParent;
	
	public GameObject go_MainMenu;
	
	public GameObject go_OptionsMenu;
	
	public GameObject go_Disconnectbtn;
	
	GameObject ref_SvrListMyServerBtn;
	
	public System.Collections.Generic.List<GameObject> NGUIserverList;
	public System.Collections.Generic.List<GameObject> NGUIchannelBtnList;
	public GameObject go_channelParent;
	public GameObject go_ChannelBtn;
	
	string mAddress = "127.0.0.1";
	string mServerName = string.Empty;
	string mMessage = "";
	float mAlpha = 0f;
	
	//bool b_once = false;

	/// <summary>
	/// Start listening for incoming UDP packets right away.
	/// </summary>

	void Start ()
	{
		if (Application.isPlaying)
		{
			// We don't want mobile devices to dim their screen and go to sleep while the app is running
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			// Make it possible to use UDP using a random port
			TNManager.StartUDP(Random.Range(10000, 50000));
			
			mServerName = string.Format("{0}'s Server", System.Environment.UserName);
			
			uii_serverName.text = mServerName;
		}
	}

	/// <summary>
	/// Adjust the server list's alpha based on whether it should be shown or not.
	/// </summary>

	void Update ()
	{
		
		
		
		if (Application.isPlaying)
		{
			float target = (TNDiscoveryClient.knownServers.size == 0) ? 0f : 1f;
			mAlpha = Tools.SpringLerp(mAlpha, target, 8f, Time.deltaTime);
			
			//Debug.Log(TNDiscoveryClient.knownServers.size);
			//if (TNServerInstance.gameServer != null)
				//Debug.Log(TNServerInstance.gameServer.name);
			
			if (!TNManager.isConnected)
			{
			
				if(TNDiscoveryClient.knownServers.size != 0 && go_ServerListParent.transform.childCount <= TNDiscoveryClient.knownServers.size )
				{
					CreateServerList();
                    NGUITools.SetActive(go_ServerListParent,true);
				}
				else if(TNDiscoveryClient.knownServers.size == 0)
				{
					foreach(GameObject serverlistbtn in NGUIserverList)
					{
						NGUITools.Destroy(serverlistbtn);
					}
					NGUIserverList.Clear();
				}
			}
			else
			{
//				if (!Application.isPlaying || Application.loadedLevelName == mainMenu && !b_once)
//				{	
//					b_once = true;
//					
//					
//				}
				if (TNManager.isInChannel)
				{
					
					
					if(go_channelParent.activeSelf)
						NGUITools.SetActive(go_channelParent, false);
				}
			}
		}
	}

	/// <summary>
	/// Show the GUI for the examples.
	/// </summary>

//	void OnGUI ()
//	{
//		if (!TNManager.isConnected)
//		{
//			DrawConnectMenu();
//		}
//		else
//		{
//			if (!Application.isPlaying || Application.loadedLevelName == mainMenu)
//			{
//				DrawSelectionMenu();
//			}
//			else if (TNManager.isInChannel)
//			{
//				DrawExampleMenu();
//			}
//			DrawDisconnectButton();
//			DrawDebugInfo();
//		}
//	}
	
	void SetServerName()
	{
		mServerName = uii_serverName.text;
		
		if(ref_SvrListMyServerBtn != null)
		{
			ServerListBtnInfo temp = ref_SvrListMyServerBtn.GetComponent<ServerListBtnInfo>();
			
			temp.serverListLabel.text = mServerName + " : " + temp.s_ServerAddress;
		}
	}
	
	void SetServerAddress()
	{
		mAddress = uii_addressName.text;
	}
	
	void ConnectToServer()
	{
		StartConnect(mAddress);
		mMessage = "Connecting...";
		setMessage(mMessage);
	}
	
	void StartLocalServer()
	{		
		if (TNServerInstance.isActive)
		{
			uil_StartServer.text = "Start Local Server";
			uis_StartServer.color = Color.green;
		
			// Stop the server, saving all the data
			TNServerInstance.Stop();
			mMessage = "Server stopped";
			setMessage(mMessage);
		}
		else
		{
		
#if UNITY_WEBPLAYER
		mMessage = "Can't host from the Web Player due to Unity's security restrictions";
#else
		// Start a local server, loading the saved data if possible
		// The UDP port of the server doesn't matter much as it's optional,
		// and the clients get notified of it via Packet.ResponseSetUDP.
		
		
		TNServerInstance.Start(serverTcpPort, Random.Range(10000, 40000));
		TNServerInstance.discoveryPort = discoveryPort;
		mMessage = "Server started";
		setMessage(mMessage);
			
		if (TNServerInstance.gameServer != null)
		{
			TNServerInstance.gameServer.name = mServerName;
			
		}
			
			
			
		uil_StartServer.text = "Stop Local Server";
		uis_StartServer.color = Color.red;
		
#endif
		}
	}
	
	void setMessage(string message)
	{
		uil_message.text = message;
	}
	
	void channelSelection()
	{
		//Debug.Log(examples.Length + " :: " + NGUIchannelBtnList.Count);
		
		if(examples.Length != NGUIchannelBtnList.Count)
		{
			int count = examples.Length;
			
			for (int i = 0; i < count; ++i)
			{
				string sceneName = examples[i];
				
				GameObject tempChannelObject = NGUITools.AddChild(go_channelParent,go_ChannelBtn);
				NGUIchannelBtnList.Add(tempChannelObject);
				
				ChannelBtn tempCB = tempChannelObject.GetComponent<ChannelBtn>();
				
				tempCB.em_CB_Ref = this;
				tempCB.uil_channelName.text = sceneName;
				tempCB.i = i;
				
			}
			
			UIGrid UIG_ref = go_channelParent.GetComponent<UIGrid>();
			UIG_ref.Reposition();
		}
	}
	
	public void SetSelectionMenu()
	{		
		if(go_channelParent.activeSelf)
			NGUITools.SetActive(go_channelParent, false);
		else
			NGUITools.SetActive(go_channelParent, true);
	}
	
	void channelMenu()
	{
		TNManager.LeaveChannel();
		
		NGUITools.SetActive(go_channelParent,true);
	}
	
	/// <summary>
	/// This menu is shown if the client has not yet connected to the server.
	/// </summary>

	void DrawConnectMenu ()
	{
		Rect rect = new Rect(Screen.width * 0.5f - 200f * 0.5f - mAlpha * 120f,
			Screen.height * 0.5f - 100f, 200f, 220f);

		// Show a half-transparent box around the upcoming UI
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.Box(Tools.PadRect(rect, 8f), "");
		GUI.color = Color.white;

		GUILayout.BeginArea(rect);
		{
			GUILayout.Label("Server Name", text);
			mServerName = GUILayout.TextField(mServerName, input, GUILayout.Width(200f));
			
			if (TNServerInstance.gameServer != null)
			{
				TNServerInstance.gameServer.name = mServerName;
			}
			
			GUILayout.Label("Server Address", text);
			mAddress = GUILayout.TextField(mAddress, input, GUILayout.Width(200f));

			if (GUILayout.Button("Connect", button))
			{
				// We want to connect to the specified destination when the button is clicked on.
				// "OnNetworkConnect" function will be called sometime later with the result.
				TNManager.Connect(mAddress);
				mMessage = "Connecting...";
			}

			if (TNServerInstance.isActive)
			{
				GUI.backgroundColor = Color.red;

				if (GUILayout.Button("Stop the Server", button))
				{
					// Stop the server, saving all the data
					TNServerInstance.Stop("server.dat");
					mMessage = "Server stopped";
				}
			}
			else
			{
				GUI.backgroundColor = Color.green;
				
				if (GUILayout.Button("Start a Local Server", button))
				{
#if UNITY_WEBPLAYER
					mMessage = "Can't host from the Web Player due to Unity's security restrictions";
#else
					// Start a local server, loading the saved data if possible
					// The UDP port of the server doesn't matter much as it's optional,
					// and the clients get notified of it via Packet.ResponseSetUDP.
					
					
					TNServerInstance.Start(serverTcpPort, Random.Range(10000, 40000), "server.dat");					 
					TNServerInstance.discoveryPort = discoveryPort;
					mMessage = "Server started";
					
										
#endif
				}
			}
			GUI.backgroundColor = Color.white;

			if (!string.IsNullOrEmpty(mMessage)) GUILayout.Label(mMessage, text);
		}
		GUILayout.EndArea();

		if (mAlpha > 0.01f)
		{
			rect.x = rect.x + (Screen.width - rect.xMin - rect.xMax) * mAlpha;
			DrawServerList(rect);
		}
	}

	/// <summary>
	/// This function is called when a connection is either established or it fails to connect.
	/// Connecting to a server doesn't mean that the connected players are now immediately able
	/// to see each other, as they have not yet joined a channel. Only players that have joined
	/// some channel are able to see and interact with other players in the same channel.
	/// You can call TNManager.JoinChannel here if you like, but in this example we let the player choose.
	/// </summary>

	void OnNetworkConnect (bool success, string message) 
	{
		
		if(success)
		{
			NGUITools.SetActive(go_MainMenu,false);
			NGUITools.SetActive(go_OptionsMenu,true);
			//NGUITools.SetActive(go_Disconnectbtn,false);
					
			NGUITools.SetActive(go_channelParent,true);
			channelSelection();
		}
		
		mMessage = message;
		setMessage(mMessage);
	}

	/// <summary>
	/// This menu is shown when a connection has been established and the player has not yet joined any channel.
	/// </summary>

	void DrawSelectionMenu ()
	{
		int count = examples.Length;

		Rect rect = new Rect(
			Screen.width * 0.5f - buttonWidth * 0.5f,
			Screen.height * 0.5f - buttonHeight * 0.5f * count,
			buttonWidth, buttonHeight);

		for (int i = 0; i < count; ++i)
		{
			string sceneName = examples[i];

			if (GUI.Button(rect, sceneName, button))
			{
				// When a button is clicked, join the specified channel.
				// Whoever creates the channel also sets the scene that will be loaded by everyone who joins.
				// In this case, we are specifying the name of the scene we've just clicked on.
				TNManager.JoinChannel(i + 1, sceneName);
			}
			rect.y += buttonHeight;
		}
		rect.y += 20f;
	}

	/// <summary>
	/// This menu is shown if the player has joined a channel.
	/// </summary>

	void DrawExampleMenu ()
	{
		Rect rect = new Rect(0f, Screen.height - buttonHeight, buttonWidth, buttonHeight);

		if (GUI.Button(rect, "Main Menu", button))
		{
			// Leaving the channel will cause the "OnNetworkLeaveChannel" to be sent out.
			TNManager.LeaveChannel();
		}
	}

	/// <summary>
	/// We want to return to the menu when we leave the channel.
	/// This message is also sent out when we get disconnected.
	/// </summary>

	void OnNetworkLeaveChannel ()
	{
		Application.LoadLevel(mainMenu);
	}

	/// <summary>
	/// The disconnect button is only shown if we are currently connected.
	/// </summary>

	void DrawDisconnectButton ()
	{
		Rect rect = new Rect(Screen.width - buttonWidth, Screen.height - buttonHeight, buttonWidth, buttonHeight);

		if (GUI.Button(rect, "Disconnect", button))
		{
			// Disconnecting while in some channel will cause "OnNetworkLeaveChannel" to be sent out first,
			// followed by "OnNetworkDisconnect". Disconnecting while not in a channel will only trigger
			// "OnNetworkDisconnect".
			TNManager.Disconnect();
		}
	}

	/// <summary>
	/// Print some additional information such as ping and which type of connection this is.
	/// </summary>

	void DrawDebugInfo ()
	{
		GUILayout.Label("Ping: " + TNManager.ping + " (" + (TNManager.canUseUDP ? "TCP+UDP" : "TCP") + ")", text);
	}

	/// <summary>
	/// Draw the list of known LAN servers.
	/// </summary>

	void DrawServerList (Rect rect)
	{
		GUI.color = new Color(1f, 1f, 1f, mAlpha * mAlpha * 0.5f);
		GUI.Box(Tools.PadRect(rect, 8f), "");
		GUI.color = new Color(1f, 1f, 1f, mAlpha * mAlpha);

		GUILayout.BeginArea(rect);
		{
			GUILayout.Label("LAN Server List", text);

			// List of discovered servers
			List<ServerList.Entry> list = TNDiscoveryClient.knownServers;

			// Server list example script automatically collects servers that have recently announced themselves
			for (int i = 0; i < list.size; ++i)
			{
				ServerList.Entry ent = list[i];
							
				if (GUILayout.Button(string.Format("{0}({1})", ent.name, ent.ip.ToString()), button))
				{
					TNManager.Connect(ent.ip.ToString());
					mMessage = "Connecting...";
				}
			}
		}
		GUILayout.EndArea();
		GUI.color = Color.white;
	}
	
	void CreateServerList()
	{
		if( go_ServerListParent.transform.childCount < TNDiscoveryClient.knownServers.size )
		{
			foreach(GameObject serverlistbtn in NGUIserverList)
			{
				NGUITools.Destroy(serverlistbtn);
			}
			NGUIserverList.Clear();
			
			// List of discovered servers
			List<ServerList.Entry> list = TNDiscoveryClient.knownServers;
			
			// Server list example script automatically collects servers that have recently announced themselves
			for (int i = 0; i < list.size; ++i)
			{
				ServerList.Entry ent = list[i];
				
				GameObject serverListObject = NGUITools.AddChild(go_ServerListParent,go_ServerListbtn);
				
				if(serverListObject != null)
					NGUIserverList.Add(serverListObject);
				
				ServerListBtnInfo tempBtnInfo = serverListObject.GetComponent<ServerListBtnInfo>();
				
				
				// set the name and address strings
				tempBtnInfo.s_ServerName = ent.name;				
				tempBtnInfo.s_ServerAddress = ent.ip.ToString();
				
				// Display the address and name;
				tempBtnInfo.serverListLabel.text = ent.name + " : " + ent.ip.ToString();
				
				tempBtnInfo.em_Ref = this;
				
				if(ent.name == mServerName)
				{
					ref_SvrListMyServerBtn = serverListObject;
				}
				
				//mMessage = "Connecting...";
				//setMessage(mMessage);
			}
			
			go_ServerListParent.GetComponent<UIGrid>().Reposition();
		}
	}
	
	public void StartConnect(string s)
	{
		TNManager.Connect(s);
		
		mMessage = "Connecting...";
		setMessage(mMessage);
	}
	
	public void StartDisconnect()
	{
		TNManager.Disconnect();
		
		NGUITools.SetActive(go_MainMenu,true);
		NGUITools.SetActive(go_Disconnectbtn,false);
		
		NGUITools.SetActive(go_OptionsMenu, false);
		
		foreach(GameObject channelbutton in NGUIchannelBtnList)
		{
			NGUITools.Destroy(channelbutton);
		}
		
		NGUIchannelBtnList.Clear();
		
		//NGUITools.SetActive(go_channelParent, false);
		
		//b_once = false;
	}
}
