using UnityEngine;
using System.Collections;
using DATA;

public class MypageChildUIManager : UIManager
{
	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;

		IsCache = false;
	}

	public override void Start ()
	{
		Register register = Register.Instance ();
		int temp = register.GetStage ();


		Color myColor = DataArray.color [temp];

		Camera.main.backgroundColor = myColor;
		SSSceneManager.Instance.LoadMenu(Config.MYPAGE);
	}
}
