using UnityEngine;
using System.Collections;

public abstract class GameManager : MonoBehaviour
{
	protected void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Debug.Log ("Back Button");
				AndroidBackButton ();
			}
		}
	
	}

	protected abstract void AndroidBackButton ();
}
