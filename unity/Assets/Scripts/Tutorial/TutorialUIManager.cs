using UnityEngine;
using System.Collections;
using DATA;

public class TutorialUIManager : UIManager
{
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

	private void LoadMenuOnActive (SSController sSC)
	{
		sSC.gameObject.GetComponent<BattleGameManager> ().SetGuide (true);
	}
}
