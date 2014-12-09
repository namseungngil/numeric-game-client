using UnityEngine;
using System.Collections;

public class NotUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.SAME;
		//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public void Love () {

	}
}
