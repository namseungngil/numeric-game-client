using UnityEngine;
using System.Collections;
using System;

public class TestScript : MonoBehaviour
{
	FXMakerAnimation fxMakerAnimation;
//	NcCurveAnimation ncCurveAnimation;
	LoveComponent loveManager;
	public GameObject effect;
	Camera effectCamera;
	GameObject button1;

	void Start ()
	{
		effectCamera = GameObject.Find ("EffectCamera").GetComponent<Camera> ();
		loveManager = GameObject.Find (Config.GAME_MANAGER).GetComponent<LoveComponent> ();
		button1 = GameObject.Find ("Button1");

//		ncCurveAnimation = GameObject.Find ("FadeInOut").GetComponent<NcCurveAnimation> ();
//		fxMakerAnimation = FXMakerAnimation.Instance ();
//		fxMakerAnimation.FadeIn (ncCurveAnimation);
		GetNGUIPostion ();
	}
	
	private void GetNGUIPostion ()
	{
		Debug.Log ("GetGet");

//		NGUITools.FindCameraForLayer
		GameObject g = Instantiate (effect, new Vector3 (1000, 1000, 0), Quaternion.identity) as GameObject;

//		Vector3 uipos = effectCamera.WorldToScreenPoint (button1.transform.position);
//		float x = uipos.x;
//		float y = uipos.y;
//		//		float z = uipos.z;
//		
//		Vector3 temp = new Vector3 (x, y, 5);

		Camera guiCam = NGUITools.FindCameraForLayer (button1.gameObject.layer);

		Vector3 pos = effectCamera.ViewportToWorldPoint (guiCam.WorldToViewportPoint (button1.transform.position));
		pos.z += 5;
		g.transform.position = pos;

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

		GetNGUIPostion ();

//		fxMakerAnimation.FadeIn (ncCurveAnimation);
	}

	public void Button2 ()
	{
		Debug.Log ("Button2");
		Notification.Unregister ();

//		fxMakerAnimation.FadeOut (ncCurveAnimation);
	}

	void OnMouseDrag() {
		print("MouseDrag!");
	}
}
