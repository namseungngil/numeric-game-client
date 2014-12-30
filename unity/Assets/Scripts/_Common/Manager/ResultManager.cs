using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultManager : UIManager
{
	// const
	protected const string STAGE = "Stage";
	protected const string SCORE = "Score";
	protected const string MAX_SCORE = "MaxScore";
	protected const string MYPSTART_STAR0 = "myPstart_star0";
	// component
	protected LoveComponent loveComponent;
	protected RankFacebookManager startFacebookManager;
	protected UILabel stage;
	protected UILabel score;
	protected UILabel maxScore;
	protected UISprite star1;
	protected UISprite star2;
	protected UISprite star3;
	// array
	protected List<GameObject> starList;

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
		
		startFacebookManager = gameObject.GetComponent<RankFacebookManager> ();
		
		stage = GameObject.Find ("S" + STAGE).GetComponent<UILabel> ();
		score = GameObject.Find ("S" + SCORE).GetComponent<UILabel> ();
		maxScore = GameObject.Find (MAX_SCORE).GetComponent<UILabel> ();
		
		stage.text = SceneData.stageLevel; 
		score.text = SceneData.score;
		maxScore.text = Game.Score (int.Parse (SceneData.stageLevel)) [2].ToString ();

		starList = new List<GameObject> ();
		starList.Add (GameObject.Find ("S" + Config.STAR1));
		starList.Add (GameObject.Find ("S" + Config.STAR2));
		starList.Add (GameObject.Find ("S" + Config.STAR3));

		star1 = starList [0].GetComponent<UISprite> ();
		star2 = starList [1].GetComponent<UISprite> ();
		star3 = starList [2].GetComponent<UISprite> ();
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
