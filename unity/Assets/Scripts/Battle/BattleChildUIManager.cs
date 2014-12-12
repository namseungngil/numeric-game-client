using UnityEngine;
using System.Collections;

public class BattleChildUIManager : UIManager
{
	private BattleUIManager battleUIManager;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}
	
	public override void Start ()
	{
		SSSceneManager.Instance.LoadMenu(Config.BATTLE);
	}

	public override void OnKeyBack ()
	{
		base.OnKeyBack ();
		
		if (!popupFlag) {
			if (battleUIManager == null) {
				battleUIManager = GameObject.Find (Config.BATTLE).GetComponent<BattleUIManager> ();
			}

			battleUIManager.Stop ();
		}
	}
}
