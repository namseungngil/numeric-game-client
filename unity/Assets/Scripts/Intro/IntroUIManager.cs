﻿using UnityEngine;
using System.Collections;

public class IntroUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;

		IsCache = false;
	}
}
