using UnityEngine;
using System.Collections;

public class SettingUIManager : UIManager
{
	// const
	private const string SCENEMANAGER = "SceneManager";
	private const string SOUND = "Sound";
	private const string LOGIN_BACK = "Back";
	private const string SOUND_BACK = "SoundBack";
	private const string SOUND_BUTTON = "SoundButton";
	private const string MYPSET_OFF = "myPset_off";
	private const string MYPSET_ON = "myPset_on";
	// gameobject
	private GameObject logout;
	// component
	private Register register;
	private UISprite soundBack;
	private UISprite soundButton;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;

		IsCache = false;
	}

	public override void Start ()
	{
		register = Register.Instance ();

		if (GameObject.Find (Config.LOGIN) != null) {
			GameObject.Find (LOGIN_BACK).SetActive (false);
		}

		soundBack = GameObject.Find (SOUND_BACK).GetComponent<UISprite> ();
		soundButton = GameObject.Find (SOUND_BUTTON).GetComponent<UISprite> ();

		if (!register.GetBackSound ()) {
			soundBack.spriteName = MYPSET_ON;
		}
		if (!register.GetButtonSound ()) {
			soundButton.spriteName = MYPSET_ON;
		}
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public void SoundBack ()
	{
		bool flag = register.GetBackSound ();

		if (flag) {
			Camera.main.audio.Stop ();
			soundBack.spriteName = MYPSET_ON;
		} else {
			Camera.main.audio.Play ();
			soundBack.spriteName = MYPSET_OFF;
		}

		register.SetBackSound (!flag);
	}
	
	public void SoundButton ()
	{
		bool flag = register.GetButtonSound ();

		if (flag) {
			soundBack.spriteName = MYPSET_ON;
		} else {
			soundBack.spriteName = MYPSET_OFF;
		}

		register.SetButtonSound (!flag);
	}

	public void LoginBack ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
		SSSceneManager.Instance.GoHome ();
	}

	public void CheatKey ()
	{
		QueryModel queryModel = QueryModel.Instance ();
		queryModel.DummyData ();
	}
}
