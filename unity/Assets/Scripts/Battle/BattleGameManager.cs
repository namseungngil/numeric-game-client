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
	// Const
	private const float READY_TIME = 2f;
	private const float START_TIME = 1f;
//	private const float ATTACK_ALL_TIME = 3f;
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
	private const int CARD_COUNT = 9;
	// Object
	private GameStatus gameStatus;
	private GameObject panel100;
	private GameObject panel200;
	private GameObject panel300;
	private UILabel[] cardUILabel = new UILabel[CARD_COUNT];
	private UILabel problemUILabel;
	private UILabel timerUILabel;
	private UILabel hpUILabel;
	private Animation panel200Animation;
	private TweenPosition panel200TweenPosition;
	private UILabel panel200UILabel;
	private BattleUIManager battleUIManager;
	private HttpComponent httpComponent;
	// Config
	private string labelString;
	private string[] MARK_TEXT = new string[] {"?", "?"};
	private string[] tempMark;
	private string[] cardString = new string[CARD_COUNT];
	private string problemSign;
	private int numberMax;
	private float maxTime = 60f;
	private float timer;
	// User
	private int clearCount;
	private int missCount;
	private int hitCount;
	private int comboCount;
	private string firstString = null;
	private string lastString = null;
	private int result;
	// Boss
	private int hp;

	// +-×÷＝★♠♥♣
	// ■◆▲▼
	void Start ()
	{
		battleUIManager = GameObject.Find (Config.UIROOT).GetComponent<BattleUIManager> ();
		timerUILabel = GameObject.Find (Config.TIMER).GetComponent<UILabel> ();
		hpUILabel = GameObject.Find (Config.HP).GetComponent<UILabel> ();
		httpComponent = gameObject.GetComponent<HttpComponent> ();

		panel100 = GameObject.Find (Config.PANEL100);
		panel200 = GameObject.Find (Config.PANEL200);
		panel300 = GameObject.Find (Config.PANEL300);

		panel200Animation = panel200.GetComponentInChildren<Animation> ();
		panel200UILabel = panel200.GetComponentInChildren<UILabel> ();
		panel200TweenPosition = panel200.GetComponentInChildren<TweenPosition> ();

		BattleStart ();
	}

	new void Update ()
	{
		base.Update ();
		if (gameStatus == GameStatus.Play) {
			timerUILabel.text = "" + timer.ToString("N2");
			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer = 0;
				StartCoroutine (Over (false));
			}
		}
	}

	protected override void AndroidBackButton ()
	{
	}

	private IEnumerator Over (bool flag)
	{
		gameStatus = GameStatus.Over;

		panel200.SetActive (true);

		panel200Animation.Play (Config.ANIMATION_BUTTON);

		string temp = TIME_OVER_TEXT;
		if (flag) {
			temp = CLEAR_TEXT;
		}
		panel200UILabel.text = temp;

		yield return new WaitForSeconds (OVER_TIME);

		panel200.SetActive (false);
		panel300.SetActive (true);

		GameObject.Find (Config.LEVEL).GetComponent<UILabel> ().text = Config.LEVEL_TEXT + LoadingData.stageLevel;
		GameObject.Find (Config.SCORE).GetComponent<UILabel> ().text = "" + clearCount;
		if (flag) {
			GameObject.Find (Config.STAR1).GetComponent<UISprite> ().spriteName = Config.STAR_FULL;
			if (timer >= Config.CLEAR_2_TIME) {
				GameObject.Find (Config.STAR2).GetComponent<UISprite> ().spriteName = Config.STAR_FULL;
			} 
			if (missCount == 0) {
				GameObject.Find (Config.STAR3).GetComponent<UISprite> ().spriteName = Config.STAR_FULL;
			}
		}

		if (flag) {
			// DB
			QueryModel dataQuery = QueryModel.Instance ();
			string date = dataQuery.Date ();
			dataQuery.BattleClear ("" + numberMax, "" + (int)timer, "" + hitCount, "" + clearCount, "" + missCount, date);

			// Http
			List<string> list = new List<string> ();
//			for (int i = 0; i < dataQuery.questUserColumnName.Length; i++) {
//				list.Add ();
//			}


		}

		battleUIManager.PlayAnimation (panel300, true);
//		PlayAnimation (panel300, true);
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
//			result = numberFirst - numberLast;
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
		hp -= tempFrist;
		hpUILabel.text = "" + (hp <= 0 ? 0 : hp);
		// Animation

		yield return new WaitForSeconds (ATTACK_TIME / 2);
		hp -= tempLast;
		hpUILabel.text = "" + (hp <= 0 ? 0 : hp);
		// Animation

		if (hp <= 0) {
			StartCoroutine (Over (true));
		} else {
			StartCoroutine (SetNextStatusWait (ATTACK_TIME / 2));
		}
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
	

	public void SetNumber (int length)
	{
		Debug.Log (gameStatus);
		if (gameStatus != GameStatus.Play || firstString == cardString[length]) {
			return;
		}

		if (firstString == null) {
			firstString = cardString[length];
		} else {
			lastString = cardString[length];
		}

		SetLabel ();

		if (firstString == null || lastString == null) {
			return;
		}

		int tempResult = 0;
		switch (problemSign) {
		case "+" : // +
			tempResult = int.Parse (firstString) + int.Parse (lastString);
			break;
		case "-" : // -
			tempResult = int.Parse (firstString) - int.Parse (lastString);
			break;
		case "*" : // *
			tempResult = int.Parse (firstString) * int.Parse (lastString);
			break;
		case "/" : // ÷
			tempResult = int.Parse (firstString) / int.Parse (lastString);
			break;
		}

		if (tempResult == result) {
			StartCoroutine (SetAttack ());
		} else {
			StartCoroutine (SetMiss ());
		}

		Debug.Log (gameStatus);
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

		// Set time
		timer = maxTime;

		// Set numer max
		numberMax = int.Parse (LoadingData.stageLevel);
		
		// Set hp
//		for (int i = 1; i <= numberMax; i++) {
//			hp += i;
//		}
		hp = numberMax * CARD_COUNT;
		hpUILabel.text = "" + hp;

		panel100.SetActive (false);
		panel200.SetActive (false);
		panel300.SetActive (false);
		
		StartCoroutine (Ready ());
	}
}
