﻿using UnityEngine;
using System.Collections;

public class StopUIManager : UIManager
{
	private BattleGameManager battleGameManager;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		battleGameManager = GameObject.Find (Config.BATTLE).GetComponent<BattleGameManager> ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public void Continue ()
	{
		if (battleGameManager != null) {
			battleGameManager.StopClear ();
		}
		Cancel ();
	}

	public void Out ()
	{
		SSSceneManager.Instance.DestroyScenesFrom (Config.BATTLE);
		SSSceneManager.Instance.Screen (Game.Scene (Config.MYPAGE));
	}

	public void Restart ()
	{
		if (battleGameManager != null) {
			battleGameManager.BattleStart ();
		}
		Cancel ();
	}
}
