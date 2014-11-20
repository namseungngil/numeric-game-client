using UnityEngine;
using System.Collections;

public class LoadingUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}
}
