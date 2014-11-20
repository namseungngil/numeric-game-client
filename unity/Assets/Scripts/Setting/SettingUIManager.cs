using UnityEngine;
using System.Collections;

public class SettingUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = true;
	}

	public void SoundBack ()
	{
		QueryModel queryModel = QueryModel.Instance ();
		queryModel.DummyData ();
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
