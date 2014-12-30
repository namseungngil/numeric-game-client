using UnityEngine;
using System.Collections;
using DATA;

public class TutorialUIManager : UIManager
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
		Color myColor = DataArray.color [0];
		Camera.main.backgroundColor = myColor;

		Register register = Register.Instance ();
		register.SetTutorial ();

		SceneData.stageLevel = Config.CARD_COUNT.ToString ();

		SSSceneManager.Instance.LoadMenu(Config.BATTLE, null, LoadMenuOnActive);
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

	private void LoadMenuOnActive (SSController sSC)
	{
		sSC.gameObject.GetComponent<BattleGameManager> ().SetGuide (true);
	}
}
