using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DATA;

public class MypageChildUIManager : UIManager
{
	// const
	private const string EFFECT1 = "Effect1";
	private const string EFFECT2 = "Effect2";
	private const string EFFECT3 = "Effect3";
	// gameobject
	List<GameObject> effectList;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;

		IsCache = false;
	}

	public override void Start ()
	{
		Register register = Register.Instance ();
		int temp = register.GetStage ();

		Color myColor = DataArray.color [temp];

		effectList = new List<GameObject> ();
		effectList.Add (GameObject.Find (EFFECT1));
		effectList.Add (GameObject.Find (EFFECT2));
		effectList.Add (GameObject.Find (EFFECT3));

		for (int i = 0; i < effectList.Count; i++) {
			if (i != (temp % 3)) {
				effectList [i].SetActive (false);
			}
		}

		Camera.main.backgroundColor = myColor;
		SSSceneManager.Instance.LoadMenu (Config.MYPAGE);
	}

	public override void OnKeyBack ()
	{
		base.OnKeyBack ();
		
		if (!popupFlag) {
			SSSceneManager.Instance.DestroyScenesFrom (Config.MYPAGE);
			SSSceneManager.Instance.GoHome ();
		}
	}
}
