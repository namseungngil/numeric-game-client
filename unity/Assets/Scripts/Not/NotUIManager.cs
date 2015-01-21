using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class NotUIManager : UIManager
{
	// const
	private const string INVITE = "Invite";
	// component
	private EffectCameraManager effectCameraManager;
	private LoveComponent loveComponent;

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
			GameObject.Find (INVITE).SetActive (false);
		}

		effectCameraManager = GameObject.Find (Config.EFFECT_CAMERA).GetComponent<EffectCameraManager> ();
		loveComponent = GameObject.Find (Config.ROOT_MANAGER).GetComponent<LoveComponent> ();
	}

	protected override void PopupOnActive (SSController sSC)
	{
		base.PopupOnActive (sSC);

		effectCameraManager.SetActive (false);
	}

	protected override void PopupOnDeactive (SSController sSC)
	{
		base.PopupOnDeactive (sSC);

		effectCameraManager.SetActive (true);
	}

	private void LogCallback (FBResult result)
	{
		if (result != null) {
			Debug.Log (result.Text);
			IDictionary iDictionary = (IDictionary)Json.Deserialize (result.Text);
			IList iList = (IList)iDictionary ["to"];
			loveComponent.Add (iList.Count);
			Cancel ();
		}
	}

	public void Love ()
	{
		Cancel ();
		SSSceneManager.Instance.PopUp (Config.LOVE, null, PopupOnActive, PopupOnDeactive);
	}

	public void Invite ()
	{
		FB.AppRequest (message: "Come play this TwoTouch!", callback: LogCallback);

//		Cancel ();
//		SSSceneManager.Instance.PopUp (Config.LOVE, null, PopupOnActive, PopupOnDeactive);
	}
}
