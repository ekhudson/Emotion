using UnityEngine;
using System.Collections;
using TNet;

public class ServerListBtnInfo : MonoBehaviour 
{
	public UILabel serverListLabel;
	
	public string s_ServerName;
	public string s_ServerAddress;
	
	public ExampleMenu em_Ref;
	
	
	void OnClick()
	{
		em_Ref.StartConnect(s_ServerAddress);
	}
}
