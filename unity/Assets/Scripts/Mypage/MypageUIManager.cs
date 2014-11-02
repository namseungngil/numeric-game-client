using UnityEngine;
using System.Collections;

public class MypageUIManager : UIManager
{
	MypageGameManager mypageGameManager;
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
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
		Debug.Log (UIButton.current.name);
		SenceData.stageLevel = UIButton.current.name.ToString ();
		Debug.Log (SenceData.currentLevel);
		SSSceneManager.Instance.PopUp (Config.START);
	}
}
