using UnityEngine;
using System.Collections;

public class StartUIManager : UIManager
{
	// component
	private LoveComponent loveComponent;

	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{

		GameObject gObj = GameObject.Find (Config.ROOT_MANAGER);
		loveComponent = gObj.GetComponent<LoveComponent> ();
	}

	public void GameStart ()
	{
		loveComponent.UseLove ();

		SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
		SSSceneManager.Instance.Screen (Config.BATTLE);
	}
}
