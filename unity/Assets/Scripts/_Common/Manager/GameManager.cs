using UnityEngine;
using System.Collections;

public abstract class GameManager : MonoBehaviour
{
	// variable
	private float cameraShake = 0;
	private float shakeAmount = 0.7f;
	private float decreaseFactor = 1.0f;

	protected void Update ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey (KeyCode.Escape)) {
				Debug.Log ("Back Button");
				AndroidBackButton ();
			}
		}

		if (cameraShake > 0) {
			Camera.main.transform.localPosition = Random.insideUnitSphere * shakeAmount;
			cameraShake -= Time.deltaTime * decreaseFactor;

			if (cameraShake < 0) {
				cameraShake = 0;
			}
		}
	
	}

	protected abstract void AndroidBackButton ();

	protected void CameraShake (float value = 1.0f)
	{
		cameraShake = value;
	}

}
