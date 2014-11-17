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
	private const float READY_TIME = 2f;
	private const float START_TIME = 1f;
	private const float ATTACK_TIME = 1f;
	private const float MISS_TIME = 1f;
	private const float OVER_TIME = 2f;
	private const float SHUFFLE_TIME = 1f;
	private const string READY_TEXT = "Ready";
	private const string START_TEXT = "Start";
	private const string TIME_OVER_TEXT = "Time Over";
	private const string CLEAR_TEXT = "Clear";
	private const string PROBLEM_LABEL_TEXT = "Problem";
	private const string SCORE_TEXT = "Score : ";
	private const string NEXT = "Next";
	private const float TIME_MAX = 90f;
	// gameobject
	private GameObject panel100;
	private GameObject panel200;
	// component
	private GameStatus gameStatus;
	private UILabel[] cardUILabel = new UILabel[Config.CARD_COUNT];
	private UILabel problemUILabel;
	private UILabel timerUILabel;
	private UILabel hpUILabel;
	private Animation panel200Animation;
	private TweenPosition panel200TweenPosition;
	private UILabel panel200UILabel;
	private BattleUIManager battleUIManager;
	private HttpComponent httpComponent;
	// array
	private string[] MARK_TEXT = new string[] {"?", "?"};
	private string[] tempMark;
	private string[] cardString = new string[Config.CARD_COUNT];
	// variable
	private string labelString;
	private string problemSign;
	private int numberMax;
	private float timer;
	private int clearCount;
	private int missCount;
	private int hitCount;
	private int comboCount;
	private string firstString = null;
	private string lastString = null;
	private int result;
	private int score;
	private int score1;
	private int score2;
	private int score3;
	
	void Start ()
	{
		battleUIManager = GameObject.Find (Config.UIROOT).GetComponent<BattleUIManager> ();
		timerUILabel = GameObject.Find (Config.TIMER).GetComponent<UILabel> ();
		hpUILabel = GameObject.Find (Config.HP).GetComponent<UILabel> ();
		httpComponent = gameObject.GetComponent<HttpComponent> ();

		panel100 = GameObject.Find (Config.PANEL100);
		panel200 = GameObject.Find (Config.PANEL200);

		panel200Animation = panel200.GetComponentInChildren<Animation> ();
		panel200UILabel = panel200.GetComponentInChildren<UILabel> ();
		panel200TweenPosition = panel200.GetComponentInChildren<TweenPosition> ();

		BattleStart ();
	}

	protected override void Update ()
	{
		base.Update ();
		if (gameStatus == GameStatus.Play) {
			timerUILabel.text = "" + ((int)timer).ToString("N2");
			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer = 0;
				StartCoroutine (Over ());
			}
		}
	}

	protected override void AndroidBackButton ()
	{
		battleUIManager.Stop ();
	}

	private IEnumerator Over ()
	{
		gameStatus = GameStatus.Over;

		bool flag = true;
		if (score >= score3) {
			// star3
		} else if (score >= score2) {
			// star2
		} else if (score >= score1) {
			// star3
		} else {
			flag = false;
		}

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);

		string temp = TIME_OVER_TEXT;
		if (flag) {
			temp = CLEAR_TEXT;
		}
		panel200UILabel.text = temp;
		yield return new WaitForSeconds (OVER_TIME);

		if (flag) {
			// DB
			QueryModel dataQuery = QueryModel.Instance ();
			string date = dataQuery.Date ();
			dataQuery.BattleClear (numberMax.ToString (), score.ToString (), ((int)timer).ToString (), hitCount.ToString (), clearCount.ToString (), missCount.ToString (), date);

			// Http
			List<string> list = new List<string> ();
			string[] tempBatleClear = new string[] {
				numberMax.ToString (), score.ToString (), ((int)timer).ToString (), hitCount.ToString (), clearCount.ToString (), missCount.ToString (), date
			};
			for (int i = 0; i < tempBatleClear.Length; i++) {
				list.Add (tempBatleClear[i]);
			}

//			httpComponent.Result (list);
//			httpComponent.OnDone = () => {
//			};

			// score
			if (FB.IsLoggedIn)
			{
				var query = new Dictionary<string, string>();
				query[QueryModel.SCORE] = score.ToString();
				query["type"] = numberMax.ToString ();
				FB.API(FacebookManager.ME_SCORE_QUERY, Facebook.HttpMethod.POST, delegate(FBResult r) {
					Debug.Log("Result: " + r.Text);
				}, query);
			}
		}

		SSSceneManager.Instance.PopUp (Config.OVER);
	}

	private IEnumerator Ready ()
	{
		// Game status
		gameStatus = GameStatus.Ready;

		panel200.SetActive (true);
		panel200TweenPosition.ResetToBeginning ();
		panel200TweenPosition.Play (true);
		panel200UILabel.text = READY_TEXT;
		yield return new WaitForSeconds (READY_TIME);

		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.text = START_TEXT;
		yield return new WaitForSeconds (START_TIME);

		panel200.SetActive (false);
		panel100.SetActive (true);
		StartCoroutine (Shuffle ());
	}

	private IEnumerator Shuffle ()
	{
		// Game Status
		gameStatus = GameStatus.Shuffle;

		firstString = null;
		lastString = null;

		CardLogic (numberMax);
		yield return new WaitForSeconds (SHUFFLE_TIME);
		ProblemLogic ();
	}

	private void CardLogic (int number)
	{
		string[] temp = new string[number];
		for (int i = 0; i < number; i++) {
			int tempString = i + 1;
			temp[i] = tempString.ToString ();
		}
		temp = RandomArray.RandomizeStrings (temp);

		UILabel[] panel100UILabel = panel100.GetComponentsInChildren<UILabel> ();
		int numberCount = 0;

		foreach (UILabel uiLabel in panel100UILabel) {
			if (uiLabel.name == Config.LABEL) {
//				Debug.Log (temp[numberCount]);
				cardUILabel[numberCount] = uiLabel;
				cardString[numberCount] = temp[numberCount];
				uiLabel.text = temp[numberCount];
				numberCount ++;
			} else if (uiLabel.name == PROBLEM_LABEL_TEXT) {
				problemUILabel = uiLabel;
			}
		}

		problemUILabel.text = "";
	}

	private void ProblemLogic ()
	{
		int numberFirst = Random.Range (0, 5);
		int numberLast = Random.Range (5, 9);
		
		numberFirst = int.Parse (cardString[numberFirst]);
		numberLast = int.Parse (cardString[numberLast]);
		
		// +-×÷＝★♠♥♣
		tempMark = MARK_TEXT;
		tempMark = RandomArray.RandomizeStrings (tempMark);
		int mark = Random.Range (1, 5);
		problemSign = "";
		switch (mark) {
		case 1 : // +
			result = numberFirst + numberLast;
			problemSign = "+";
			break;
		case 2 : // -
			List<int> tempInt = new List<int> ();
			foreach (string i in cardString) {
				tempInt.Add (int.Parse (i));
			}

			tempInt.Sort ();

			List<int> temp2Result = new List<int> ();
			for (int i = tempInt.Count - 1; i >= 0; i--) {
				Debug.Log (tempInt[i]);
				for (int j = i - 1; j >= 0; j--) {
					int cal2Result = tempInt[i] - tempInt[j];
					temp2Result.Add (cal2Result);
				}
			}

			result = temp2Result[Random.Range (0, temp2Result.Count)];
			problemSign = "-";
			break;
		case 3 : // ×
			result = numberFirst * numberLast;
			problemSign = "*";
			break;
		case 4 : // ÷
			List<int> temp4Result = new List<int> ();

			foreach (string temp in cardString) {
				for (int i = 0; i < cardString.Length; i++) {
					if (temp != cardString[i]) {
						int calFirst = int.Parse (temp);
						int calLast = int.Parse (cardString[i]);
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

			result = temp4Result[Random.Range (0, temp4Result.Count)];
			problemSign = "/";
			break;
		}

//		foreach (UILabel uiLabel in cardUILabel) {
//			uiLabel.text = "";
//		}

		SetLabel ();

		gameStatus = GameStatus.Play;
	}

	private void SetLabel ()
	{
		string tempFirst = tempMark[0];
		string tempLast = tempMark[1];

		if (firstString != null) {
			tempFirst = firstString;
		}

		if (lastString != null) {
			tempLast = lastString;
		}

		labelString = tempFirst + " " + problemSign + " " + tempLast + " = " + result;
		problemUILabel.text = labelString;
	}

	private IEnumerator SetNextStatusWait (float time)
	{
		yield return new WaitForSeconds (time);

		StartCoroutine (Shuffle ());
	}

	private IEnumerator SetAttack ()
	{
		clearCount++;
		comboCount++;
		gameStatus = GameStatus.Attack;

		int tempFrist = int.Parse (firstString);
		int tempLast = int.Parse (lastString);
		if (tempFrist == numberMax || tempLast == numberMax) {
			hitCount++;
		}

		string tempString = "Attack";
		int tempCombo = comboCount - 1;
		if (tempCombo >= 1) {
			tempString = "Combo +";
			tempString += "" + tempCombo;
			tempFrist += tempCombo;
			tempLast += tempCombo;
		}

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.text = tempString;
		yield return new WaitForSeconds (ATTACK_TIME);
		panel200.SetActive (false);

		yield return new WaitForSeconds (ATTACK_TIME / 2);
		score += tempFrist;
		hpUILabel.text = "" + (score <= 0 ? 0 : score);
		// Animation

		yield return new WaitForSeconds (ATTACK_TIME / 2);
		score += tempLast;
		hpUILabel.text = "" + (score <= 0 ? 0 : score);
		// Animation

		StartCoroutine (SetNextStatusWait (ATTACK_TIME / 2));
	}

	private IEnumerator SetMiss ()
	{
		missCount++;
		comboCount = 0;
		gameStatus = GameStatus.Miss;

		panel200.SetActive (true);
		panel200Animation.Play (Config.ANIMATION_BUTTON);
		panel200UILabel.text = "Miss";
		yield return new WaitForSeconds (MISS_TIME);
		panel200.SetActive (false);

		StartCoroutine (SetNextStatusWait (MISS_TIME / 2));
	}

	private int MainLogic (int first, int last)
	{
		int tempResult = -1;
		switch (problemSign) {
		case "+" : // +
			tempResult = first + last;
			break;
		case "-" : // -
			tempResult = first - last;
			break;
		case "*" : // *
			tempResult = first * last;
			break;
		case "/" : // ÷
			tempResult = first / last;
			if ((first % last) != 0) {
				tempResult = -1;
			}
			break;
		}

		return tempResult;
	}

	private int firstInt = 0;
	private int lastInt = 0;
	public void SetNumber (int length)
	{
		Debug.Log (gameStatus);
		if (gameStatus != GameStatus.Play || firstString == cardString[length]) {
			return;
		}

		if (firstString == null) {
			firstString = cardString[length];
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
			lastString = cardString[length];
			lastInt = int.Parse (lastString);
		}

		// set label
		SetLabel ();

		if (firstString == null || lastString == null) {
			return;
		}

		int tempResult = MainLogic (firstInt, lastInt);
		Debug.Log ("result : " + tempResult);
		if (tempResult == result) {
			StartCoroutine (SetAttack ());
		} else {
			StartCoroutine (SetMiss ());
		}
	}

	public bool GetCheckStatus (GameStatus gS) {
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
		score1 = 0;
		score2 = 0;
		score3 = 0;

		// Set time
		timer = TIME_MAX;

		// Set numer max
		numberMax = int.Parse (SenceData.stageLevel);

		List<int> list = Game.Score (numberMax);
		// Set score1
		score1 = list [0];
		score2 = list [1];
		score3 = list [2];

		hpUILabel.text = "" + score;

		panel100.SetActive (false);
		panel200.SetActive (false);
		
		StartCoroutine (Ready ());
	}

	public void Stop ()
	{
		gameStatus = GameStatus.Stop;
	}

	public void StopClear ()
	{
		gameStatus = GameStatus.Play;
		StartCoroutine (Shuffle ());
	}
}
