using UnityEngine;
using System.Collections;

public class SettingUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = false;
	}

	public void SoundBack ()
	{
	}
	
	public void SoundButton ()
	{
	}
	
	public void Guide ()
	{
	}
	
	public void Board ()
	{
		Application.OpenURL (Config.BOARD_URL);
	}

	public void FacebookLogout ()
	{
		if (FB.IsLoggedIn) {
			FB.Logout ();
		}
	}
}
