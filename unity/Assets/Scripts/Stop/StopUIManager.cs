using UnityEngine;
using System.Collections;

public class StopUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = true;
	}

	public void Out ()
	{
		SSSceneManager.Instance.GoHome ();
//		SSSceneManager.Instance.Screen (Config.MYPAGE);
	}
}
