using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUIManager : UIManager
{
	// const
	private const string STAGE = "Stage";
	private const string SCORE = "Score";
	private const string MAX_SCORE = "MaxScore";
	private const string MYPSTART_STAR0 = "myPstart_star0";
	// component
	private LoveComponent loveComponent;
	private StartFacebookManager startFacebookManager;
	private UILabel stage;
	private UILabel score;
	private UILabel maxScore;
	private UISprite star1;
	private UISprite star2;
	private UISprite star3;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		GameObject gObj = GameObject.Find (Config.ROOT_MANAGER);
		if (gObj != null) {
			loveComponent = gObj.GetComponent<LoveComponent> ();
		}

		startFacebookManager = gameObject.GetComponent<StartFacebookManager> ();

		stage = GameObject.Find (STAGE).GetComponent<UILabel> ();
		score = GameObject.Find (SCORE).GetComponent<UILabel> ();
		maxScore = GameObject.Find (MAX_SCORE).GetComponent<UILabel> ();

		stage.text = SceneData.stageLevel; 
		score.text = SceneData.score;
		maxScore.text = Game.Score (int.Parse (SceneData.stageLevel)) [2].ToString ();

		star1 = GameObject.Find ("S" + Config.STAR1).GetComponent<UISprite> ();
		star2 = GameObject.Find ("S" + Config.STAR2).GetComponent<UISprite> ();
		star3 = GameObject.Find ("S" + Config.STAR3).GetComponent<UISprite> ();

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
