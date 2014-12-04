using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUIManager : ResultManager
{
	public override void Awake ()
	{
		base.Awake ();
	}

	public override void Start ()
	{
		base.Start ();

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

	public void GameStart ()
	{
		if (loveComponent.UseLove ()) {
			SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
			SSSceneManager.Instance.Screen (Game.Scene (Config.BATTLE));
		} else {

		}
	}

	public void Request ()
	{
		Debug.Log (UIButton.current.name);
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
