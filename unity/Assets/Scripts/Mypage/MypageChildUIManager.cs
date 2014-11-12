using UnityEngine;
using System.Collections;

public class MypageChildUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = true;
	}
	
	public override void OnEnableFS ()
	{
		SSSceneManager.Instance.LoadMenu(Config.MYPAGE);
	}
}
