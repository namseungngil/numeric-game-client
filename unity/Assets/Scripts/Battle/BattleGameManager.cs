using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameStatus
{
	Ready = 1,
	Shuffle,
	Play,
	Attack,
	Miss,
	Stop,
	Over
}

public class BattleGameManager : GameManager
{
	// const
	private const string PLUS = "battleP3_04";
	private const string MINUS = "battleP3_05";
	private const string MULTIPLICATION = "battleP3_06";
	private const string DIVISION = "battleP3_07";
	private const string EQUAL = "battleP3_08";
	private const string PROBLEM1_LABEL_TEXT = "Problem1";
	private const string PROBLEM2_LABEL_TEXT = "Problem2";
	private const string PROBLEM3_LABEL_TEXT = "Problem3";
	private const string SING = "Sign";
	private const string RESUlT = "Result";
	private const string TIME_PLUS = "TimePlus";
	private const string SCORE_TEXT = "Score : ";
	private const string NEXT = "Next";
	private const string PROGRESS_BAR = "Progress Bar";
	private const string GUAGE_STAR0 = "guage_star0";
	private const string TIME = "Time";
	private const string BONUS = "Bonus";
	private const string BONUS_BACKGROUND = "BonusBackground";
	private const string TIME_BACKGROUND = "TimeBackground";
	private const string BATTLE_T00 = "battle_t00";
	private const string BATTLE_T08 = "battle_t08";
	private const string BATTLE_TIME01 = "battle_time01";
	private const string BATTLE_TIME02 = "battle_time02";
	private const string BATTLE_TIME03 = "battle_time03";
	private const string GUIDE = "Guide";
	private const string PROBLEM_BACKGROUND = "ProblemBackground";
	private const string PANEL300 = "Panel:300";
	private const float READY_TIME = 2f;
	private const float START_TIME = 1f;
	private const float ATTACK_TIME = 1f;
	private const float MISS_TIME = 1f;
	private const float OVER_TIME = 2f;
	private const float SHUFFLE_TIME = 1f;
	private const float TIME_MAX = 60f;
	private const float BONUS_TIME = 5f;
	private const int GUIDE_COUNT = 5;
	// gameobject
	public GameObject goodEffect;
	public GameObject missEffect;
	public GameObject clearEffect;
	public GameObject timeOverEffect;
	private GameObject panel100;
	private GameObject panel200;
	private GameObject panel300;
	private GameObject guide;
	// component
	private GoogleMobileAdsComponent ad;
	private EffectCameraManager effectCameraManager;
	private BattleUIManager battleUIManager;
	private HttpComponent httpComponent;
	private GameStatus gameStatus;
	private TweenPosition panel200TweenPosition;
	private Animation panel200Animation;
	private Animation scoreAnimation;
	private Animation bonusAnimation;
	private Animation merryAnimation;
	private UIProgressBar uIProgressbar;
	private UIProgressBar timeUIProgressbar;
	private UISprite star1UISprite;
	private UISprite star2UISprite;
	private UISprite star3UISprite;
	private UISprite timerUISprite;
	private UISprite signUISprite;
	private UISprite resultUISprite;
	private UISprite problemUISprite;
	private UILabel problem1UILabel;
	private UILabel problem2UILabel;
	private UILabel problem3UILabel;
	private UILabel timerUILabel;
	private UILabel scoreUILabel;
	private UILabel panel200UILabel;
	private UILabel bonusUILabel;
	private UILabel timePlusUILabel;
	private UILabel guideParent;
	// array
	private string[] MARK_TEXT;
	private string[] tempMark;
	private string[] cardString;
	private UILabel[] panel100UILabel;
	private Color[] color;
	// variable
	private string labelString;
	private string problemSign;
	private string firstString;
	private string lastString;
	public float timer;
	private int numberMax;
	private int clearCount;
	private int missCount;
	private int hitCount;
	private int comboCount;
	private int result;
	private int score;
	private int score1;
	private int score2;
	private int score3;
	private int guideCount;
	private bool guideFlag = false;
	
	void Start ()
	{
		GameObject tempAd = GameObject.Find (Config.ROOT_MANAGER);
		if (tempAd != null) {
			ad = tempAd.GetComponent<GoogleMobileAdsComponent> ();
			ad.SetAd ();
		}

		effectCameraManager = GameObject.Find (Config.EFFECT_CAMERA).GetComponent<EffectCameraManager> ();
		battleUIManager = GameObject.Find (Config.UIROOT).GetComponent<BattleUIManager> ();
		uIProgressbar = GameObject.Find (PROGRESS_BAR).GetComponent<UIProgressBar> ();
		timeUIProgressbar = GameObject.Find (TIME).GetComponent<UIProgressBar> ();
		star1UISprite = GameObject.Find (Config.STAR1).GetComponent<UISprite> ();
		star2UISprite = GameObject.Find (Config.STAR2).GetComponent<UISprite> ();
		star3UISprite = GameObject.Find (Config.STAR3).GetComponent<UISprite> ();
		timerUISprite = GameObject.Find (TIME_BACKGROUND).GetComponent<UISprite> ();
		signUISprite = GameObject.Find (SING).GetComponent<UISprite> ();
		resultUISprite = GameObject.Find (RESUlT).GetComponent<UISprite> ();
		problemUISprite = GameObject.Find (PROBLEM_BACKGROUND).GetComponent<UISprite> ();
		timerUILabel = GameObject.Find (Config.TIMER).GetComponent<UILabel> ();
		scoreUILabel = GameObject.Find (Config.SCORE).GetComponent<UILabel> ();
		timePlusUILabel = GameObject.Find (TIME_PLUS).GetComponent<UILabel> ();
		bonusAnimation = GameObject.Find (BONUS_BACKGROUND).GetComponent<Animation> ();
		merryAnimation = GameObject.Find (Config.MERRY).GetComponent<Animation> ();

		httpComponent = gameObject.GetComponent<HttpComponent> ();

		panel100 = GameObject.Find (Config.PANEL100);
		panel200 = GameObject.Find (Config.PANEL200);
		panel300 = GameObject.Find (PANEL300);
		guide = GameObject.Find (GUIDE);

		scoreAnimation = scoreUILabel.gameObject.GetComponent<Animation> ();
		panel200Animation = panel200.GetComponentInChildren<Animation> ();
		panel200UILabel = panel200.GetComponentInChildren<UILabel> ();
		panel200TweenPosition = panel200.GetComponentInChildren<TweenPosition> (); 
		bonusUILabel = Logic.GetChildObject (bonusAnimation.gameObject, BONUS).GetComponent<UILabel> ();

		MARK_TEXT = new string[] {"?", "?"};
		cardString = new string[Config.CARD_COUNT];

		signUISprite.spriteName = "";
		resultUISprite.spriteName = "";

		timePlusUILabel.text = "+" + (int)BONUS_TIME;
		timePlusUILabel.gameObject.SetActive (false);

		panel300.SetActive (false);
		guide.SetActive (false);
		guideParent = null;
		guideCount = 0;

		color = new Color[] {
			new Color32 (0, 184, 162, 128), new Color32 (255, 92, 73, 128)
		};

		BattleStart ();
	}

	void Update ()
	{
		if (gameStatus == GameStatus.Play) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer = 0;
				if (!guideFlag) {
					StartCoroutine (Over ());
				}
			}

			SetTimer ();
		}

		SetGuide ();
	}

	private void SetTimer ()
	{
		float temp = timer / (float)TIME_MAX;
		if (temp <= 0.4) {
			timerUISprite.spriteName = BATTLE_T08;
			problemUISprite.color = color [1];
		} else {
			timerUISprite.spriteName = BATTLE_T00;
			problemUISprite.color = color [0];
		}
		timeUIProgressbar.value = temp > 1 ? 1 : temp;
		timerUILabel.text = "" + ((int)timer).ToString ();
	}

	private IEnumerator Over ()
	{
		SetStatus (GameStatus.Over);

		SceneData.score = score.ToString ();

		bool flag = true;
		if (score >= score3) {
			// star3
		} else if (score >= score2) {
			// star2
		} else if (score >= score1) {
			// star1
		} else {
			flag = false;
		}

		if (guideFlag) {
			flag = true;
		}

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);

		string temp = "TIME OVER";
		GameObject tempEffect = timeOverEffect;
		if (flag) {
			temp = "CLEAR";
			tempEffect = clearEffect;
			panel200UILabel.color = new Color32 (41, 171, 226, 255);

			// animation
			merryAnimation.Play (Config.ANIMATION_GOOD);
		} else {
			panel200UILabel.color = new Color32 (212, 20, 90, 255);

			// animation
			merryAnimation.Play (Config.ANIMATION_MISS);
		}

		// effect
		effectCameraManager.GUIOnOverEffect (tempEffect, panel200UILabel.gameObject);
		panel200UILabel.text = temp;
		yield return new WaitForSeconds (OVER_TIME);

		// guide
		if (guideFlag) {
			SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
			if (SceneData.tutorialStartScene == Config.MYPAGE) {
				SSSceneManager.Instance.Screen (Game.Scene (SceneData.tutorialStartScene));
			} else {
				SSSceneManager.Instance.GoHome ();
			}

			yield break;
		}

		if (flag) {
			// ad
			if (ad != null) {
				ad.GetAd ();
			}

			// DB
			QueryModel dataQuery = QueryModel.Instance ();
			string date = QueryModel.Date ();
			string[] tempData = dataQuery.BattleClear (numberMax.ToString (), score.ToString (), ((int)timer).ToString (), hitCount.ToString (), clearCount.ToString (), missCount.ToString (), date);

			// score
			if (FB.IsLoggedIn) {
				if (tempData != null) {
					Dictionary<string, string> dic = new Dictionary<string, string> ();
					for (int i = 0; i < tempData.Length; i++) {
						dic.Add (dataQuery.questUserColumnName [i], tempData [i]);
					}
					httpComponent.OnDone = (object obj) => {
						SSSceneManager.Instance.PopUp (Config.OVER);
					};
					httpComponent.Over (dic);
				}
			} else {
				SSSceneManager.Instance.PopUp (Config.OVER);
			}
		} else {
			SSSceneManager.Instance.PopUp (Config.OVER);
		}
	}

	private IEnumerator Ready ()
	{
		// Game status
		SetStatus (GameStatus.Ready);

		panel200.SetActive (true);
		panel200TweenPosition.ResetToBeginning ();
		panel200TweenPosition.Play (true);

		string temp = "READY";
		if (guideFlag) {
			panel300.SetActive (true);
			panel200UILabel.color = new Color32 (255, 255, 255, 255);
			temp = "TUTORIALS";
		} else {
			panel200UILabel.color = new Color32 (140, 198, 63, 255);
		}
		panel200UILabel.text = temp;
		yield return new WaitForSeconds (READY_TIME);

		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.text = "GO";
		yield return new WaitForSeconds (START_TIME);

		panel200.SetActive (false);
		panel100.SetActive (true);
		StartCoroutine (Shuffle ());
	}

	private IEnumerator Shuffle ()
	{
		// Game Status
		SetStatus (GameStatus.Shuffle);

		firstString = null;
		lastString = null;

		CardLogic (numberMax);
		yield return new WaitForSeconds (SHUFFLE_TIME);
		// animation
		merryAnimation.Play (Config.ANIMATION_NORMAL);

		ProblemLogic ();
	}

	private List<string> CardSuffle (int start, int last)
	{
		List<string> arr = new List<string> ();

		for (int i = start; i < last; i++) {
			int tempString = i + 1;
			arr.Add (tempString.ToString ());
		}

		return RandomArray.RandomizeStrings<string> (arr);
	}

	private List<string> SetCardCount (List<string> list, List<string> get, int count)
	{
		for (int i = 0; i < count; i++) {
			list.Add (get [i]);
		}

		return list;
	}

	private void CardLogic (int number)
	{
		List<string> temp12 = new List<string> ();
		List<string> temp101 = new List<string> ();
		List<string> temp102 = new List<string> ();

		if (number <= 20) {
			temp12 = CardSuffle (0, number);
		}
		if (number > 20 && number <= 110) {
			temp12 = CardSuffle (0, 9);
			temp101 = CardSuffle (9, number);
		} 
		if (number > 110) {
			temp12 = CardSuffle (0, 9);
			temp101 = CardSuffle (9, 99);
			temp102 = CardSuffle (99, number);
		}

		List<string> temp = new List<string> ();
		int tempCount = 9;
		if (temp102.Count > 0) {
			tempCount = 3;
			temp = SetCardCount (temp, temp12, tempCount);
			temp = SetCardCount (temp, temp101, tempCount);
			temp = SetCardCount (temp, temp102, tempCount);
		} else if (temp101.Count > 0) {
			tempCount = Random.Range (4, 6);
			temp = SetCardCount (temp, temp12, tempCount);
			temp = SetCardCount (temp, temp101, Config.CARD_COUNT - tempCount);
		} else {
			temp = SetCardCount (temp, temp12, tempCount);
		}

		temp = RandomArray.RandomizeStrings<string> (temp);

		if (panel100UILabel == null) {
			panel100UILabel = panel100.GetComponentsInChildren<UILabel> ();
		}

		int numberCount = 0;
		foreach (UILabel uiLabel in panel100UILabel) {
			if (uiLabel.name == Config.LABEL) {
				cardString [numberCount] = temp [numberCount];
				uiLabel.text = temp [numberCount].ToString ();
				numberCount ++;
			}

			if (problem1UILabel == null && uiLabel.name == PROBLEM1_LABEL_TEXT) {
				problem1UILabel = uiLabel;
			} else if (problem2UILabel == null && uiLabel.name == PROBLEM2_LABEL_TEXT) {
				problem2UILabel = uiLabel;
			} else if (problem3UILabel == null && uiLabel.name == PROBLEM3_LABEL_TEXT) {
				problem3UILabel = uiLabel;
			}
		}

		problem1UILabel.text = "";
		problem2UILabel.text = "";
		problem3UILabel.text = "";
		signUISprite.spriteName = "";
		resultUISprite.spriteName = "";
		bonusUILabel.text = cardString [Random.Range (0, 9)];
		bonusAnimation.Play (Config.ANIMATION_BUTTON);
	}

	private void ProblemLogic ()
	{
		int numberFirst = Random.Range (0, 5);
		int numberLast = Random.Range (5, 9);
		
		numberFirst = int.Parse (cardString [numberFirst]);
		numberLast = int.Parse (cardString [numberLast]);
		
		// +-×÷＝★♠♥♣
		tempMark = MARK_TEXT;
		int mark = Random.Range (1, 5);
		problemSign = "";
		switch (mark) {
		case 1: // +
			result = numberFirst + numberLast;
			problemSign = PLUS;
			break;
		case 2: // -
			List<int> tempInt = new List<int> ();
			foreach (string i in cardString) {
				tempInt.Add (int.Parse (i));
			}

			tempInt.Sort ();

			List<int> temp2Result = new List<int> ();
			for (int i = tempInt.Count - 1; i >= 0; i--) {
				for (int j = i - 1; j >= 0; j--) {
					int cal2Result = tempInt [i] - tempInt [j];
					temp2Result.Add (cal2Result);
				}
			}

			result = temp2Result [Random.Range (0, temp2Result.Count)];
			problemSign = MINUS;
			break;
		case 3: // ×
			result = numberFirst * numberLast;
			problemSign = MULTIPLICATION;
			break;
		case 4: // ÷
			List<int> temp4Result = new List<int> ();

			foreach (string temp in cardString) {
				for (int i = 0; i < cardString.Length; i++) {
					if (temp != cardString [i]) {
						int calFirst = int.Parse (temp);
						int calLast = int.Parse (cardString [i]);
						if ((calFirst % calLast) == 0) {
							int cal4Result = calFirst / calLast;
							temp4Result.Add (cal4Result);
						}
					}
				}
			}

			if (temp4Result.Count <= 0) {
				ProblemLogic ();
			}

			result = temp4Result [Random.Range (0, temp4Result.Count)];
			problemSign = DIVISION;
			break;
		}

//		foreach (UILabel uiLabel in cardUILabel) {
//			uiLabel.text = "";
//		}

		SetLabel ();

		SetStatus (GameStatus.Play);
	}

	private void SetLabel ()
	{
		string tempFirst = tempMark [0];
		string tempLast = tempMark [1];

		if (firstString != null) {
			tempFirst = firstString;
		}

		if (lastString != null) {
			tempLast = lastString;
		}

		signUISprite.spriteName = problemSign;
		resultUISprite.spriteName = EQUAL;

		problem1UILabel.text = tempFirst;
		problem2UILabel.text = tempLast;
		problem3UILabel.text = result.ToString ();
	}

	private IEnumerator SetNextStatusWait (float time)
	{
		yield return new WaitForSeconds (time);

		StartCoroutine (Shuffle ());
	}

	private void SetGuage ()
	{
		float temp = (float)score / (float)score3;
		uIProgressbar.value = temp > 1 ? 1 : temp;

		if (score >= score1 && star1UISprite.spriteName != GUAGE_STAR0) {
			star1UISprite.spriteName = GUAGE_STAR0;
		}

		if (score >= score2 && star2UISprite.spriteName != GUAGE_STAR0) {
			star2UISprite.spriteName = GUAGE_STAR0;
		}

		if (score >= score3 && star3UISprite.spriteName != GUAGE_STAR0) {
			star3UISprite.spriteName = GUAGE_STAR0;
		}
	}

	private void Effect (GameObject gO)
	{
		for (int i = 0; i < 40; i++) {
			Vector3 screenPos = new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 0);
			Vector3 pos = effectCameraManager.Get ().ScreenToWorldPoint (screenPos);
			pos.z += Config.EFFECT_Z;
			GameObject gObj = Instantiate (gO, pos, Quaternion.identity) as GameObject;
			gObj.transform.parent = effectCameraManager.Get ().transform;
		}
	}

	private IEnumerator SetAttack ()
	{
		clearCount++;
		comboCount++;
		guideCount++;
		SetStatus (GameStatus.Attack);

		int tempFrist = int.Parse (firstString);
		int tempLast = int.Parse (lastString);
		if (tempFrist == numberMax || tempLast == numberMax) {
			hitCount++;
		}

		int bonus = int.Parse (bonusUILabel.text);
		if (tempFrist == bonus || tempLast == bonus) {
			timePlusUILabel.gameObject.SetActive (true);
			timePlusUILabel.gameObject.GetComponent<Animation> ().Play (Config.ANIMATION_BUTTON);
			timer += BONUS_TIME;
			SetTimer ();
		}

		panel200UILabel.color = new Color32 (255, 143, 0, 255);
		string tempString = "GOOD";
		int tempCombo = comboCount - 1;
		if (tempCombo >= 1) {
			panel200UILabel.color = new Color32 (255, 112, 95, 255);
			tempString = "COMBO +";
			tempString += "" + tempCombo;
			tempFrist += tempCombo;
			tempLast += tempCombo;
		}

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.text = tempString;
		// effect
		Effect (goodEffect);

		// animation
		merryAnimation.Play (Config.ANIMATION_GOOD);

		yield return new WaitForSeconds (ATTACK_TIME);
		timePlusUILabel.gameObject.SetActive (false);
		panel200.SetActive (false);

		yield return new WaitForSeconds (ATTACK_TIME / 2);
		score += tempFrist;
		scoreUILabel.text = "" + (score <= 0 ? 0 : score);
		SetGuage ();
		// Animation
		scoreAnimation.Play (Config.ANIMATION_BUTTON);

		yield return new WaitForSeconds (ATTACK_TIME / 2);
		score += tempLast;
		scoreUILabel.text = "" + (score <= 0 ? 0 : score);
		SetGuage ();
		// Animation
		scoreAnimation.Play (Config.ANIMATION_BUTTON);

		if (guideFlag) {
			if (guideCount >= GUIDE_COUNT) {
				StartCoroutine (Over ());
				yield break;
			}
		}

		StartCoroutine (SetNextStatusWait (ATTACK_TIME / 2));
	}

	private IEnumerator SetMiss ()
	{
		missCount++;
		comboCount = 0;
		SetStatus (GameStatus.Miss);

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.color = new Color32 (158, 0, 167, 255);
		panel200UILabel.text = "MISS";
		// effect
		Effect (missEffect);

		// animation
		merryAnimation.Play (Config.ANIMATION_MISS);

		yield return new WaitForSeconds (MISS_TIME);
		panel200.SetActive (false);

		StartCoroutine (SetNextStatusWait (MISS_TIME / 2));
	}

	private int MainLogic (int first, int last)
	{
		int tempResult = -1;
		switch (problemSign) {
		case PLUS : // +
			tempResult = first + last;
			break;
		case MINUS : // -
			tempResult = first - last;
			break;
		case MULTIPLICATION : // *
			tempResult = first * last;
			break;
		case DIVISION : // ÷
			tempResult = first / last;
			if ((first % last) != 0) {
				tempResult = -1;
			}
			break;
		}

		return tempResult;
	}

	private void SetStatus (GameStatus gS)
	{
		if (gameStatus == GameStatus.Stop) {
			return;
		}

		gameStatus = gS;
	}

	private float GaugeStarPercent (float f, float minX)
	{
		float temp = (((f / score3) * 130) * 2) + minX;
		return temp;
	}

	private string tempGuide = "";
	private void SetGuide ()
	{
		if (!guideFlag) {
			return;
		}

		if (gameStatus == GameStatus.Play) {
			guide.SetActive (true);


			if (firstString == null) {
				for (int i = 0; i < cardString.Length; i++) {
					for (int j = 0; j < cardString.Length; j++) {
						if (cardString[j] == cardString[i]) {
							continue;
						}
	
						int tempInt = MainLogic (int.Parse (cardString[i]), int.Parse (cardString[j]));
						if (tempInt == result) {
							tempGuide = cardString[i];
							break;
						}
					}
				}
			} else {
				for (int k = 0; k < cardString.Length; k++) {
					if (tempGuide == cardString [k]) {
						continue;
					}
					int tempLast = int.Parse (cardString [k]);
					
					int tempInt2 = MainLogic (firstInt, tempLast);
					if (tempInt2 == result) {
						tempGuide = tempLast.ToString ();
						break;
					}
				}
			}
//			Debug.Log ("SetGuide : " + tempGuide);

			if (guideParent != null && guideParent.text == tempGuide) {
				return;
			}

			foreach (UILabel uL in panel100UILabel) {
				if (uL.name == Config.LABEL) {
					if (uL.text == tempGuide) {
						guideParent = uL;

					}
				}
			}
//			Debug.Log ("guideParent : " + guideParent.text);

			if (guideParent != null) {
				Vector3 guideVector3 = guideParent.gameObject.GetComponentInParent<UISprite> ().transform.localPosition;
				guideVector3.y += 120f;
				guide.transform.localPosition = guideVector3;
			}


		} else {
			tempGuide = "";
			guide.transform.localPosition = new Vector3 (10000, 10000, 0);
			guide.SetActive (false);
			guideParent = null;
		}
	}

	private int firstInt = 0;
	private int lastInt = 0;

	public void SetNumber (int length)
	{
		if (gameStatus != GameStatus.Play || firstString == cardString [length]) {
			return;
		}

		if (firstString == null) {
			firstString = cardString [length];
			firstInt = int.Parse (firstString);

			bool tempFlag = false;
			for (int i = 0; i < cardString.Length; i++) {
				if (length == i) {
					continue;
				}
				int tempLast = int.Parse (cardString [i]);

				int tempInt = MainLogic (firstInt, tempLast);
				if (tempInt == result) {
					tempFlag = true;
				}
			}

			if (!tempFlag) {
				StartCoroutine (SetMiss ());
			}
		} else {
			lastString = cardString [length];
			lastInt = int.Parse (lastString);
		}

		// set label
		SetLabel ();

		if (firstString == null || lastString == null) {
			return;
		}

		int tempResult = MainLogic (firstInt, lastInt);
		if (tempResult == result) {
			StartCoroutine (SetAttack ());
		} else {
			StartCoroutine (SetMiss ());
		}
	}

	public bool GetCheckStatus (GameStatus gS)
	{
		return gameStatus == gS;
	}

	public GameStatus GetGameStatus ()
	{
		return gameStatus;
	}

	public float GetTimer ()
	{
		return timer;
	}

	public void BattleStart ()
	{
		clearCount = 0;
		missCount = 0;
		hitCount = 0;
		score = 0;
		score1 = 0;
		score2 = 0;
		score3 = 0;

		// Set time
		timer = TIME_MAX;

		// Set numer max
		numberMax = int.Parse (SceneData.stageLevel);

		List<int> list = Game.Score (numberMax);
		// Set score1
		score1 = list [0];
		score2 = list [1];
		score3 = list [2];

		float minX = -130f;
		float maxX = 130f;

		Vector3 starVector1 = star2UISprite.transform.localPosition;
		starVector1.x = GaugeStarPercent (score1, minX);
		star1UISprite.transform.localPosition = starVector1;

		Vector3 starVector2 = star2UISprite.transform.localPosition;
		starVector2.x = GaugeStarPercent (score2, minX);
		star2UISprite.transform.localPosition = starVector2;

		Vector3 star3Vector3 = star3UISprite.transform.localPosition;
		star3Vector3.x = maxX;
		star3UISprite.transform.localPosition = star3Vector3;


		scoreUILabel.text = "" + score;
		timerUILabel.text = ((int)timer).ToString ("");

		panel100.SetActive (false);
		panel200.SetActive (false);

		gameStatus = GameStatus.Ready;
		StartCoroutine (Ready ());
	}

	public bool Stop ()
	{
		if (gameStatus == GameStatus.Over) {
			return false;
		}

		gameStatus = GameStatus.Stop;
		return true;
	}

	public void StopClear ()
	{
		gameStatus = GameStatus.Play;
		StartCoroutine (Shuffle ());
	}

	public void SetGuide (bool flag)
	{
		guideFlag = flag;
	}
}
