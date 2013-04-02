using UnityEngine;
using System.Collections;
using TNet;

public class CurrentPlayerList : MonoBehaviour 
{
	
	public UILabel playerList;
	
	void OnNetworkPlayerJoin(Player p)
	{
		Debug.Log ("PLAYER HAS ENTERED");
		UpdateList();
	}
	
	void OnNetworkPlayerLeave(Player p)
	{
		Debug.Log ("PLAYER HAS LEFT");
		
		UpdateList();
	}
	
	void OnClick()
	{
		UpdateList();
	}
	
	void UpdateList()
	{
		if(playerList.text != null)
		{
			playerList.text = "";
	
			foreach( Player player in NetworkManager.Instance.CurrentPlayers)
			{
				if (player == null)
				{
					continue;
				}
				
				playerList.text += player.name + "\n";
			}
		}
	}
}
