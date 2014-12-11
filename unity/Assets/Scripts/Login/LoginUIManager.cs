using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginUIManager : UIManager
{
	// gameobject
	private GameObject facebookUI;
	// component
	private LoginGameManager loginGameManager;
	private UILabel uILabel;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}
	public override void Start ()
	{
		facebookUI = GameObject.Find (Config.FACEBOOK);
		uILabel = facebookUI.GetComponentInChildren<UILabel> ();
		loginGameManager = gameObject.GetComponent<LoginGameManager> ();
	}

	public override void OnKeyBack ()
	{
		Application.Quit ();
	}

	void Update ()
	{
		if (FB.IsLoggedIn) {
//			facebookUI.SetActive (false);
			uILabel.text = "LOGOUT";
		} else {
//			facebookUI.SetActive (true);
			uILabel.text = "LOGIN";
		}
	}

	public void GameStart ()
	{
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}
	
	public void FacebookLogin ()
	{
		if (FB.IsLoggedIn) {
			FB.Logout ();
		} else {
			loginGameManager.FacebookLogin ();
		}
	}

	public void Setting ()
	{
		SSSceneManager.Instance.PopUp (Config.SETTING);
	}
}
