using UnityEngine;
using System.Collections;

public class NotUIManager : UIManager
{
	// const
	private EffectCameraManager effectCameraManager;

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

		effectCameraManager = GameObject.Find (Config.EFFECT_CAMERA).GetComponent<EffectCameraManager> ();
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

	public void Love ()
	{
		Cancel ();
		SSSceneManager.Instance.PopUp (Config.LOVE, null, PopupOnActive, PopupOnDeactive);
	}
}
