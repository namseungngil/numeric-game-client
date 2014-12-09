﻿using UnityEngine;
using System.Collections;

public class UIManager : SSController
{
	// const
	protected const string CANCEL = "Cancel";

	protected virtual void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Debug.Log ("Back Button");
				AndroidBackButton ();
			}
		}
	}
	
	void AndroidBackButton ()
	{
		Debug.Log ("AndroidBackButton : " + gameObject.name);
	}
	
	public void Cancel ()
	{
		SSSceneManager.Instance.Close ();
	}
}
