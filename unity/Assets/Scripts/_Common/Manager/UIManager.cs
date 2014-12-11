using UnityEngine;
using System.Collections;

public class UIManager : SSController
{
	// const
	protected const string CANCEL = "Cancel";

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
	
	public void Cancel ()
	{
		SSSceneManager.Instance.Close ();
	}
}
