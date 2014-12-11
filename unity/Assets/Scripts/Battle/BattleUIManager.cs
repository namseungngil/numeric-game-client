using UnityEngine;
using System.Collections;

public class BattleUIManager : UIManager
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
		battleGameManager = gameObject.GetComponent<BattleGameManager> ();
	}

	public override void OnKeyBack ()
	{
		Stop ();
	}

	public void BattleOnClick ()
	{
//		Debug.Log ("BattleOnClick");
		if (!battleGameManager.GetCheckStatus (GameStatus.Play)) {
			return;
		}

		GameObject gO = UIButton.current.gameObject;
		UILabel tempUILabel = gO.GetComponentInChildren<UILabel> ();
		if (tempUILabel.text == "") {
			return;
		}

		string temp = gO.name.Replace (Config.BATTLE, "");
		battleGameManager.SetNumber (int.Parse(temp) - 1);

		tempUILabel.text = "";
	}

	public void Stop ()
	{
		if (battleGameManager.Stop ()) {
			SSSceneManager.Instance.PopUp (Config.STOP);
		}
	}
}
