using UnityEngine;
using System.Collections;

public class OverUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}

	public void Retry ()
	{
		SSSceneManager.Instance.Reset ();
	}
	
	public void Next ()
	{
		SSSceneManager.Instance.GoHome ();
	}
}
