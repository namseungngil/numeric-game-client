using UnityEngine;
using System.Collections;

public class UIManager : SSController
{
	// const
	protected const string CANCEL = "Cancel";
	// static
	public static bool popupFlag;

	public override void Start ()
	{
		base.Start ();

		popupFlag = false;
	}

//	protected virtual void Update ()
//	{
//		if (Application.platform == RuntimePlatform.Android) {
//			if (Input.GetKey (KeyCode.Escape)) {
//				Debug.Log ("Back Button");
//				AndroidBackButton ();
//			}
//		}
//	}
//	
//	protected virtual void AndroidBackButton ()
//	{
//		Debug.Log ("AndroidBackButton");
//	}

	protected virtual void PopupOnActive (SSController sSC)
	{
		popupFlag = true;
	}
	
	protected virtual void PopupOnDeactive (SSController sSC)
	{
		popupFlag = false;
	}
	
	public void Cancel ()
	{
		SSSceneManager.Instance.Close ();
	}
}
