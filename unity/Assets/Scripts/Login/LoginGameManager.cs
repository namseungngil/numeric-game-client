using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginGameManager : GameManager
{
	// compoent
	private UIManager uIManager;

	private void LoginCallback (FBResult result)
	{
		if (result.Error != null) {
			// "Error Response:\n" + result.Error;
			Debug.Log ("Error Response:\n" + result.Error);
			uIManager.Cancel ();
			// debugText = result.Error;
		} else if (!FB.IsLoggedIn) {
			// "Login cancelled by Player";
			Debug.Log ("Login cancelled by Player");
			uIManager.Cancel ();
		} else {
			// "Login was successful!";
			Debug.Log ("Login was successful!");
			uIManager.Cancel ();
			SSSceneManager.Instance.Screen (Config.INTRO);
		}
	}

	public void PopupCallback (SSController s)
	{
		uIManager = s.gameObject.GetComponent<UIManager> ();
		FB.Login ("email, publish_actions", LoginCallback);
	}

	public void FacebookLogin ()
	{
		if (FB.IsLoggedIn) {
			return;
		}

		SSSceneManager.Instance.PopUp (Config.LOADING, null, PopupCallback);
	}
}
