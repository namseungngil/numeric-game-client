using UnityEngine;
using System.Collections;

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
		SSSceneManager.Instance.LoadMenu(Config.BATTLE);
	}
}
