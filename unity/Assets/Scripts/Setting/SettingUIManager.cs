using UnityEngine;
using System.Collections;

public class SettingUIManager : UIManager
{
	// const
	private const string SCENEMANAGER = "SceneManager";
	private const string SOUND = "Sound";
	private const string LOGIN_BACK = "LoginBack";
	// gameobject
	private GameObject logout;
	// component
	private Register register;
	private SceneManager sceneManager;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;

		IsCache = false;
	}

	public override void Start ()
	{
		sceneManager = GameObject.Find (SCENEMANAGER).GetComponent<SceneManager> ();

		if (GameObject.Find (Config.LOGIN) != null) {
			GameObject.Find (LOGIN_BACK).SetActive (false);
		}

		if (GameObject.Find (Config.MYPAGE) != null) {

		}

		register = Register.Instance ();
	}

	public void SoundBack ()
	{
		bool flag = register.GetBackSound ();

		if (flag) {
			Camera.main.audio.Stop ();
		} else {
			Camera.main.audio.Play ();
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

	public void LoginBack ()
	{
		SSSceneManager.Instance.GoHome ();
	}

	public void CheatKey ()
	{
		QueryModel queryModel = QueryModel.Instance ();
		queryModel.DummyData ();
	}
}
