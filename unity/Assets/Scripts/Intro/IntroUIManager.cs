using UnityEngine;
using System.Collections;

public class IntroUIManager : SSController
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = false;
	}
}
