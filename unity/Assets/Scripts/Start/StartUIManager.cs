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
		loveComponent = GameObject.Find (Config.MYPAGE).GetComponent<LoveComponent> ();
	}

	public void GameStart ()
	{
		loveComponent.UseLove ();
		SSSceneManager.Instance.Screen (Config.BATTLE);
	}
}
