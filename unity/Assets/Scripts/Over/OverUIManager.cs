using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverUIManager : ResultManager
{
	// const
	private const string BACKGROUND = "Background";
	private const string RETRY = "Retry";
	private const string NEXT = "Next";
	private const string SPRITE = "Sprite";
	private const float STAR_ON_TIMER = 1f;
	// gameobject
	public GameObject starEffect;
	private GameObject nextGObj;
	// component
	private EffectCameraManager effectCameraManager;
	private RankFacebookManager rankFacebookManager;
	// array
	private List<GameObject> button;
	private List<GameObject> star;
	private Color32[] failColor;
	private List<int> list;
	// variable
	private int index;
	private int score;
	private int stage;
	private bool flag;

	public override void Start ()
	{
		base.Start ();

		if (FB.IsLoggedIn) {
			rankFacebookManager = gameObject.GetComponent<RankFacebookManager> ();
			rankFacebookManager.Rank ();
		}

		GameObject temp = GameObject.Find (Config.EFFECT_CAMERA);
		if (temp != null) {
			effectCameraManager = temp.GetComponent<EffectCameraManager> ();
		}

		nextGObj = GameObject.Find (NEXT);
		flag = true;

		button = new List<GameObject> ();
		button.Add (GameObject.Find (RETRY));
		button.Add (GameObject.Find (NEXT));
		button.Add (GameObject.Find (CANCEL));

		star = new List<GameObject> ();
		star.Add (Logic.GetChildObject (star1.gameObject, SPRITE));
		star.Add (Logic.GetChildObject (star2.gameObject, SPRITE));
		star.Add (Logic.GetChildObject (star3.gameObject, SPRITE));

		Logic.SetActive (star, false);

		score = int.Parse (SceneData.score);
		stage = int.Parse (SceneData.stageLevel);
		list = Game.Score (stage);
		index = 0;
		if (score >= list [index]) {
			Logic.SetActive (button, false);
			flag = false;

			StartCoroutine (ClearStart ());
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

			nextGObj.SetActive (false);
		}
	}

	public override void OnKeyBack ()
	{
		if (flag) {
			OverCancel ();
		}
	}

	private IEnumerator ClearStart ()
	{
		yield return new WaitForSeconds (0.6f);

		StartCoroutine (Clear (STAR_ON_TIMER));
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
				flag = true;
			}
		} else {
			flag = true;
		}

		if (flag) {
			StartCoroutine (OnButton ());
		}
	}

	private IEnumerator OnButton ()
	{
		yield return new WaitForSeconds (STAR_ON_TIMER);

		Logic.SetActive (button, true);
	}

	public void Next ()
	{
		int temp = stage + 1;

		if (temp <= SceneData.lastStage) {
			SceneData.nextStage = temp.ToString ();
		}

		OverCancel ();
	}

	public void Retry ()
	{
		SceneData.nextStage = stage.ToString ();

		OverCancel ();
	}
	
	public void OverCancel ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}
}
