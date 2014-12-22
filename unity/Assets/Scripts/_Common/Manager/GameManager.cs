using UnityEngine;
using System.Collections;

public abstract class GameManager : MonoBehaviour
{
	// variable
	private float shakeAmount = 0.2f;
	private float shakeTime = 0.2f;

	protected void CameraShake ()
	{
		iTween.ShakePosition (Camera.main.transform.gameObject, iTween.Hash ("x", shakeAmount, "time", shakeTime));
	}

	void OnApplicationPause(bool pauseStatus) {
//		Debug.Log ("GameManager OnApplicationPause : " + pauseStatus);
		if (!pauseStatus) {
			Notification.CancelAll ();
		}
	}
}
