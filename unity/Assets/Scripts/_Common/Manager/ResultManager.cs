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
		
		stage = GameObject.Find (STAGE).GetComponent<UILabel> ();
		score = GameObject.Find (SCORE).GetComponent<UILabel> ();
		maxScore = GameObject.Find (MAX_SCORE).GetComponent<UILabel> ();
		
		stage.text = SceneData.stageLevel; 
		score.text = SceneData.score;
		maxScore.text = Game.Score (int.Parse (SceneData.stageLevel)) [2].ToString ();
		
		star1 = GameObject.Find ("S" + Config.STAR1).GetComponent<UISprite> ();
		star2 = GameObject.Find ("S" + Config.STAR2).GetComponent<UISprite> ();
		star3 = GameObject.Find ("S" + Config.STAR3).GetComponent<UISprite> ();
	}
}
