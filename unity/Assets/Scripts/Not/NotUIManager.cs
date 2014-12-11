using UnityEngine;
using System.Collections;

public class NotUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		if (!FB.IsLoggedIn) {
			GameObject.Find (Config.FACEBOOK).SetActive (false);
		}
	}

	public void Love ()
	{
		Cancel ();
		SSSceneManager.Instance.PopUp (Config.LOVE);
	}
}
