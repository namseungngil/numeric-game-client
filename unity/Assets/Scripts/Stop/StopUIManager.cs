using UnityEngine;
using System.Collections;

public class StopUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public void Out ()
	{
		SSSceneManager.Instance.GoHome ();
	}
}
