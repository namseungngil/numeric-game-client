using UnityEngine;
using System.Collections;
using DATA;

public class MypageUIManager : UIManager
{
	// const
	private const string UP = "Up";
	private const string Down = "Down";
	//gameobject
	private GameObject upGameObject;
	private GameObject downGameObject;
	// component
	private MypageGameManager mypageGameManager;

	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		upGameObject = GameObject.Find (UP);
		downGameObject = GameObject.Find (Down);
		mypageGameManager = gameObject.GetComponent<MypageGameManager> ();

		if (!mypageGameManager.NextQuestStatus ()) {
			upGameObject.SetActive (false);
		}

		if (!mypageGameManager.BackQuestStatus ()) {
			downGameObject.SetActive (false);
		}

	}

	public void Love ()
	{
		if (FB.IsLoggedIn) {
			SSSceneManager.Instance.PopUp (Config.LOVE);
		}
	}

	public void Setting ()
	{
		SSSceneManager.Instance.PopUp (Config.SETTING);	
	}

	public void GameStart ()
	{
		string temp = UIButton.current.name.ToString ();
		if (temp == MypageGameManager.DISABLE_QUEST) {
			return;
		}
		SenceData.stageLevel = temp;
		SSSceneManager.Instance.PopUp (Config.START);
	}
}
