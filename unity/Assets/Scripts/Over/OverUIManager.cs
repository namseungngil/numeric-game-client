using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverUIManager : ResultManager
{
	// const
	private string BACKGROUND = "Background";
	private string PLAY = "Play";
	private string NEXT = "NEXT";
	private string RETRY = "RETRY";
	// component
	private RankFacebookManager rankFacebookManager;
	private UILabel btnLabel;
	// array
	private Color32[] failColor;

	public override void Start ()
	{
		base.Start ();

		if (FB.IsLoggedIn) {
			rankFacebookManager = gameObject.GetComponent<RankFacebookManager> ();
			rankFacebookManager.Rank ();
		}

		btnLabel = GameObject.Find (PLAY).GetComponentInChildren<UILabel> ();

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

			btnLabel.text = NEXT;

		} else {
			failColor = new Color32[] {
				new Color32 (88, 119, 57, 200),
				new Color32 (88, 119, 57, 255),
				new Color32 (95, 31, 23, 255)
			};

			UISprite b1 = GameObject.Find (BACKGROUND + "1").GetComponent<UISprite> ();
			UISprite b2 = GameObject.Find (BACKGROUND + "2").GetComponent<UISprite> ();
			UISprite b3 = GameObject.Find (BACKGROUND + "3").GetComponent<UISprite> ();
			b1.color = failColor[0];
			b2.color = failColor[0];
			b3.color = failColor[0];

			btnLabel.text = RETRY;
		}
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public void RetryPlay ()
	{
		if (btnLabel.text == RETRY) {
			SSSceneManager.Instance.Close ();
			SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
			SSSceneManager.Instance.Screen (Game.Scene (Config.BATTLE));
		} else {
			OverCancel ();
		}
	}
	
	public void OverCancel ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}
}
