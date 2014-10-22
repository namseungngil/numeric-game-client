using UnityEngine;
using System.Collections;

public class BattleUIManager : UIManager
{
	BattleGameManager battleGameManager;
	
	new void Start ()
	{
		battleGameManager = GameObject.Find (Config.GAME_MANAGER).GetComponent<BattleGameManager> ();
	}

//	void Update ()
//	{
//		float temp = battleGameManager.GetTimer ();
//		if (temp > 0) {
//			timer.text = "" + (int)temp;
//		}
//	}

	private void Common (int length)
	{
		battleGameManager.SetNumber (length);
	}

	public void BattleOnClick (GameObject gO)
	{
		if (!battleGameManager.GetCheckStatus (GameStatus.Play)) {
			return;
		}

		UILabel tempUILabel = gO.GetComponentInChildren<UILabel> ();
		if (tempUILabel.text == "") {
			return;
		}

		Common(int.Parse(gO.name) - 1);
		gO.GetComponent<Animation> ().Play (Config.ANIMATION_BUTTON);
		tempUILabel.text = "";
		// Color32 myNewColor = new Color32(128,128,128,255);
		// Color myOtherColor = new Color(0.5f,0.5f,0.5f, 1f); //RGBA in 0-1f
	}

	public void Retry ()
	{
		battleGameManager.BattleStart ();
	}

	public void Next ()
	{
		LoadingData.currentLevel = Config.MYPAGE;
		Application.LoadLevel (Config.LOADING);
	}
}
