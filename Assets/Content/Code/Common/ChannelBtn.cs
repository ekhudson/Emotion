using UnityEngine;
using System.Collections;

public class ChannelBtn : MonoBehaviour 
{
	public UILabel uil_channelName;
	public int i;
	
	public ExampleMenu em_CB_Ref;
	
	void OnClick()
	{
		TNManager.JoinChannel(i + 1, uil_channelName.text);
		em_CB_Ref.SetSelectionMenu();
	}
}
