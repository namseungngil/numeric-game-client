using UnityEngine;
using System.Collections;
using DATA;

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
		Register register = Register.Instance ();
		int temp = register.GetStage ();

		if (temp >= 9 && temp <= 11) {
			temp -= 9;
			Color myColor = DataArray.battleColor [temp];
			Camera.main.backgroundColor = myColor;
		}

		SSSceneManager.Instance.LoadMenu(Config.BATTLE);
	}

	public override void OnKeyBack ()
	{
		base.OnKeyBack ();
		
		if (!popupFlag) {
			if (battleUIManager == null) {
				battleUIManager = GameObject.Find (Config.BATTLE).GetComponent<BattleUIManager> ();
			}

			if (battleUIManager.GetBackStatus ()) {
				battleUIManager.Stop ();
			}
		}
	}
}
