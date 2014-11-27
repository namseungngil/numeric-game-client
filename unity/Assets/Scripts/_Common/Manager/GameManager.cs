using UnityEngine;
using System.Collections;

public abstract class GameManager : MonoBehaviour
{
	// variable
	private float shakeAmount = 0.2f;
	private float shakeTime = 0.2f;

	protected virtual void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Debug.Log ("Back Button");
				AndroidBackButton ();
			}
		}
	}

	protected abstract void AndroidBackButton ();

	protected void CameraShake ()
	{
		iTween.ShakePosition (Camera.main.transform.gameObject, iTween.Hash ("x", shakeAmount, "time", shakeTime));
	}

	void OnApplicationPause(bool pauseStatus) {
		Debug.Log ("GameManager OnApplicationPause : " + pauseStatus);
		if (!pauseStatus) {
			Notification.CancelAll ();
		}
	}
}
