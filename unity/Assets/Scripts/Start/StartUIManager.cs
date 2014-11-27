using UnityEngine;
using System.Collections;

public class StartUIManager : UIManager
{
	// component
	private LoveComponent loveComponent;
	private StartFacebookManager startFacebookManager;

	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		GameObject gObj = GameObject.Find (Config.ROOT_MANAGER);
		if (gObj != null) {
			loveComponent = gObj.GetComponent<LoveComponent> ();
		}

		startFacebookManager = GameObject.Find (GRIDLIST).GetComponent<StartFacebookManager> ();
	}

	public void GameStart ()
	{
		if (loveComponent.UseLove ()) {
			SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
			SSSceneManager.Instance.Screen (Game.Scene (Config.BATTLE));
		} else {

		}
	}

	public void Request ()
	{
		if (UIButton.current.name == FacebookManager.BUTTON) {
			return;
		}

		if (startFacebookManager != null) {
			string[] temp = new string[] {
				UIButton.current.name
			};
			startFacebookManager.onChallengeClicked (temp);
		}
	}
}
