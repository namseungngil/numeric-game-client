﻿using UnityEngine;
using System.Collections;

public class LoadingUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void OnKeyBack ()
	{
	}
}
