using UnityEngine;
using System.Collections;

public class LoginFacebookManager : FacebookManager
{
	private GameObject facebookUI;

	new void Start ()
	{
		base.Start ();
		facebookUI = GameObject.Find (Config.FACEBOOK);
	}

	void Update ()
	{
		if (FB.IsLoggedIn) {
//			debugText = FB.UserId;
			facebookUI.SetActive (false);
		} else {
			facebookUI.SetActive (true);
		}
	}

//	void OnGUI ()
//	{
//		GUI.TextArea (new Rect (10, 10, 400, 100), debugText);
//	}

	private void LoginCallback (FBResult result)
	{
		if (result.Error != null) {
			// "Error Response:\n" + result.Error;
			Debug.Log ("Error Response:\n" + result.Error);
//			debugText = result.Error;
		} else if (!FB.IsLoggedIn) {
			// "Login cancelled by Player";
			Debug.Log ("Login cancelled by Player");
		} else {
			// "Login was successful!";
			Debug.Log ("Login was successful!");

			gameObject.GetComponent<HttpComponent> ().Login (0);
		}
	}
	
	public void Login ()
	{
		if (!FB.IsLoggedIn) {
			FB.Login ("email, publish_actions", LoginCallback);
		}
	}

	public void Logout ()
	{
		if (FB.IsLoggedIn) {
			FB.Logout ();
		}
	}
}
