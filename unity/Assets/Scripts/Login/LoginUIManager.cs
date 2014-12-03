using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginUIManager : UIManager
{
	// gameobject
	private GameObject facebookUI;
	// component
	private LoginGameManager loginGameManager;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}
	public override void Start ()
	{
		facebookUI = GameObject.Find (Config.FACEBOOK);
		loginGameManager = gameObject.GetComponent<LoginGameManager> ();
	}

	protected override void Update ()
	{
		if (FB.IsLoggedIn) {
			facebookUI.SetActive (false);
		} else {
			facebookUI.SetActive (true);
		}
	}

	public void GameStart ()
	{
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
//		SSSceneManager.Instance.GoHome ();
	}
	
	public void FacebookLogin ()
	{
		loginGameManager.FacebookLogin ();
	}

	public void Setting ()
	{
		SSSceneManager.Instance.PopUp (Config.SETTING);
	}
}
