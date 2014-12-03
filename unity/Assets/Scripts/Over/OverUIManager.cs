using UnityEngine;
using System.Collections;

public class OverUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public void Retry ()
	{
		SSSceneManager.Instance.Close ();
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.BATTLE));
	}
	
	public void Next ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}
}
