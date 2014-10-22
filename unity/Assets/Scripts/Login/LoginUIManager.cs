using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginUIManager : UIManager
{
	private string path;

	new void Start () {
		base.Start ();
		NumericPlayerPrefs numericPlayerPrefs = NumericPlayerPrefs.Instance ();
		numericPlayerPrefs.GetLove ();
	}

	public void Login ()
	{
		if (uiStatus != UIStatus.Default) {
			return;
		}

		LoadingData.currentLevel = Config.MYPAGE;
		Application.LoadLevel (Config.LOADING);
	}

	public void Facebook ()
	{
		if (uiStatus != UIStatus.Default) {
			return;
		}

		LoginFacebookManager loginFacebookManager = GameObject.Find (Config.GAME_MANAGER).GetComponent<LoginFacebookManager> ();
		loginFacebookManager.Login ();
	}

	public void FacebookLogout ()
	{
		LoginFacebookManager loginFacebookManager = GameObject.Find (Config.GAME_MANAGER).GetComponent<LoginFacebookManager> ();
		loginFacebookManager.Logout ();
	}
}
