using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUIManager : ResultManager
{
	// component
	private EffectCameraManager effectCameraManager;

	public override void Awake ()
	{
		base.Awake ();
	}

	public override void Start ()
	{
		base.Start ();
		effectCameraManager = GameObject.Find (Config.EFFECT_CAMERA).GetComponent<EffectCameraManager> ();

		int tempScore = int.Parse (SceneData.score);
		List<int> list = Game.Score (int.Parse (SceneData.stageLevel));
		if (tempScore >= list [0]) {
			star1.spriteName = MYPSTART_STAR0;
			if (tempScore >= list [1]) {
				star2.spriteName = MYPSTART_STAR0;
				if (tempScore >= list [2]) {
					star3.spriteName = MYPSTART_STAR0;
				}
			}
		}
	}

	protected void PopupOnActive (SSController s)
	{
		effectCameraManager.SetActive (false);
	}
	
	protected void PopupOnDeActive (SSController s)
	{
		effectCameraManager.SetActive (true);
	}

	public void GameStart ()
	{
		if (loveComponent.UseLove ()) {
			SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
			SSSceneManager.Instance.Screen (Game.Scene (Config.BATTLE));
		} else {
			Cancel ();
			SSSceneManager.Instance.PopUp (Config.NOT, null, PopupOnActive, PopupOnDeActive);
		}
	}

	public void Request ()
	{
//		Debug.Log (UIButton.current.name);
		if (UIButton.current.name == FacebookManager.BUTTON) {
			return;
		}

		if (startFacebookManager != null) {
			string[] temp = new string[] {
				UIButton.current.name
			};
			startFacebookManager.onChallengeClicked (temp);
		}
	}
}
