using UnityEngine;
using System.Collections;

public class StartUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public void GameStart ()
	{
		SSSceneManager.Instance.Screen (Config.BATTLE);
	}
}
