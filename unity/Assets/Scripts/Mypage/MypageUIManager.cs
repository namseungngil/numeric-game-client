using UnityEngine;
using System.Collections;

public class MypageUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = true;
	}

	public void Love ()
	{
		SSSceneManager.Instance.PopUp (Config.LOVE);
//		CommonNotification (panel300);
	}

	public void Setting ()
	{
		SSSceneManager.Instance.PopUp (Config.SETTING);
	}

	public void GameStart ()
	{
		SenceData.currentLevel = UIButton.current.name;
		SSSceneManager.Instance.PopUp (Config.START);
	}
}
