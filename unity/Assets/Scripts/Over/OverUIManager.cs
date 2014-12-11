using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverUIManager : ResultManager
{
	// const
	private const string BACKGROUND = "Background";
	private const string PLAY = "Play";
	private const string NEXT = "NEXT";
	private const string RETRY = "RETRY";
	private const string SPRITE = "Sprite";
	private const float STAR_ON_TIMER = 1f;
	// gameobject
	public GameObject starEffect;
	// component
	private EffectCameraManager effectCameraManager;
	private RankFacebookManager rankFacebookManager;
	private UILabel btnLabel;
	// array
	private List<GameObject> button;
	private List<GameObject> star;
	private Color32[] failColor;
	private List<int> list;
	// variable
	private int index;
	private int score;

	public override void Start ()
	{
		base.Start ();

		if (FB.IsLoggedIn) {
			rankFacebookManager = gameObject.GetComponent<RankFacebookManager> ();
			rankFacebookManager.Rank ();
		}

		effectCameraManager = GameObject.Find (Config.EFFECTCAMERA).GetComponent<EffectCameraManager> ();

		button = new List<GameObject> ();
		button.Add (GameObject.Find (PLAY));
		button.Add (GameObject.Find (CANCEL));
		btnLabel = button [0].GetComponentInChildren<UILabel> ();

		star = new List<GameObject> ();
		star.Add (Logic.GetChildObject (star1.gameObject, SPRITE));
		star.Add (Logic.GetChildObject (star2.gameObject, SPRITE));
		star.Add (Logic.GetChildObject (star3.gameObject, SPRITE));

		Logic.SetActive (star, false);

		score = int.Parse (SceneData.score);
		list = Game.Score (int.Parse (SceneData.stageLevel));
		index = 0;
		if (score >= list [index]) {
			Logic.SetActive (button, false);

			StartCoroutine (Clear (0.5f));
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
			b1.color = failColor [0];
			b2.color = failColor [1];
			b3.color = failColor [2];

			btnLabel.text = RETRY;
		}
	}

	private IEnumerator Clear (float time)
	{
		yield return new WaitForSeconds (time);
		effectCameraManager.GUIOnEffect (starEffect, starList[index].gameObject);
		star[index].SetActive (true);
		index++;
		if (index < star.Count) {
			if (score >= list [index]) {
				StartCoroutine (Clear (STAR_ON_TIMER));
			} else {
				Logic.SetActive (button, true);
			}
		} else {
			Logic.SetActive (button, true);
		}
	}

	public void RetryPlay ()
	{
		int temp = int.Parse (SceneData.stageLevel);
		if (btnLabel.text == RETRY) {

		} else {
			temp += 1;
		}

		if (temp < SceneData.lastStage) {
			SceneData.nextStage = temp.ToString ();
		}

		OverCancel ();
	}
	
	public void OverCancel ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}
}
