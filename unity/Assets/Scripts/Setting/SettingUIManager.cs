using UnityEngine;
using System.Collections;

public class SettingUIManager : UIManager
{
	// const
	private const string LOGOUT = "Logout";
	private const string SOUND = "Sound";
	// gameobject
	private GameObject logout;
	// component
	private Register register;
	private SoundControl soundControl;

	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = true;
	}

	public override void Start ()
	{
		logout = GameObject.Find (LOGOUT);
		if (FB.IsLoggedIn) {
			logout.SetActive (true);
		} else {
			logout.SetActive (false);
		}

		register = Register.Instance ();
		GameObject temp = GameObject.Find (SOUND);
		if (temp != null) {
			soundControl = temp.GetComponent<SoundControl> ();
		}
	}

	public void SoundBack ()
	{
		bool flag = register.GetBackSound ();

		if (flag) {

		} else {

		}

		if (soundControl != null) {
			soundControl.Set (!flag);
		}

		register.SetBackSound (!flag);
	}
	
	public void SoundButton ()
	{
		bool flag = register.GetButtonSound ();

		if (flag) {

		} else {

		}

		register.SetButtonSound (!flag);
	}
	
	public void Guide ()
	{
	}

	public void Logout ()
	{
		FB.Logout ();
		logout.SetActive (false);
	}

	public void CheatKey ()
	{
		QueryModel queryModel = QueryModel.Instance ();
		queryModel.DummyData ();
	}
}
