using UnityEngine;
using System.Collections;
using System;

public class TestScript : MonoBehaviour
{
	FXMakerAnimation fxMakerAnimation;
	NcCurveAnimation ncCurveAnimation;
	LoveComponent loveManager;

	void Start ()
	{
		loveManager = GameObject.Find (Config.GAME_MANAGER).GetComponent<LoveComponent> ();

		ncCurveAnimation = GameObject.Find ("FadeInOut").GetComponent<NcCurveAnimation> ();
		fxMakerAnimation = FXMakerAnimation.Instance ();
		fxMakerAnimation.FadeIn (ncCurveAnimation);
	}

	public void Button1 ()
	{

//	
////		nowTime += 10;
//		string[] str = new string[] {
//			date.AddSeconds(10).ToString ("yyyyMMddHHmmss"), "Title Test aram", "Text test"
//		};
//		Debug.Log (str[0]);
//		Notification.Register (str);

		Debug.Log ("Button1");

//		DateTime date = DateTime.Now;
//		string[] tempString = new string[] {
//			date.AddSeconds(10).ToString ("yyyyMMddHHmmss"), Config.GAME_TITME, "Test notification" 
//		};
//		Notification.Register (tempString);

		loveManager.UseLove ();

		fxMakerAnimation.FadeIn (ncCurveAnimation);
	}

	public void Button2 ()
	{
		Debug.Log ("Button2");
		Notification.Unregister ();

		fxMakerAnimation.FadeOut (ncCurveAnimation);
	}
}
