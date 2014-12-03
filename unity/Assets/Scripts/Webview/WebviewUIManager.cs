using UnityEngine;
using System.Collections;

public class WebviewUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = true;
	}
}
