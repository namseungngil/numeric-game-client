using UnityEngine;
using System.Collections;

public class MypageUIManager : UIManager
{
	// gameobject
	private GameObject panel300;
	private GameObject panel400;
	private GameObject panel500;
	// gameobject
	private GameObject gameManager;
	// component
	private MypageFacebookManager mypageFacebookManager;
	// string
	private string level;
	// bool
	private bool flag;

	new void Start ()
	{
		base.Start ();

		gameManager = GameObject.Find (Config.GAME_MANAGER);

		panel300 = GameObject.Find (Config.PANEL300);
		panel400 = GameObject.Find (Config.PANEL400);
		panel500 = GameObject.Find (Config.PANEL500);

		panel300.SetActive (false);
		panel400.SetActive (false);
		panel500.SetActive (false);

		flag = false;
	}

	public void Love ()
	{
		CommonNotification (panel300);
	}

	public void LoveCancel ()
	{
		CommonCancel (panel300);
	}

	public void FacebookFriendList ()
	{
		CommonCancel (panel300, panel400);
		FacebookFriends ();
	}

	public void FacebookFriendListCancel ()
	{
		CommonCancel (panel400);
		FacebookRequest ();
	}

	public void FacebookFriends ()
	{
		mypageFacebookManager = gameManager.GetComponent<MypageFacebookManager> ();
		mypageFacebookManager.Friends ();
	}

	public void FacebookRequest ()
	{
		mypageFacebookManager.onChallengeClicked ();
	}

	public void Battle (GameObject gO)
	{
		CommonNotification (panel500);
		level = gO.GetComponentInChildren<UILabel> ().text;
	}

	public void BattleCancel ()
	{
		CommonCancel (panel500);
	}

	public void BattleStart ()
	{
		if (!flag) {
			LoveComponent loveComponent = gameManager.GetComponent<LoveComponent> ();
			flag = loveComponent.UseLove ();
			if (flag) {
				LoadingData.currentLevel = Config.BATTLE;
				LoadingData.stageLevel = level;
				Application.LoadLevel (Config.LOADING);
			}
		}
	}
}
