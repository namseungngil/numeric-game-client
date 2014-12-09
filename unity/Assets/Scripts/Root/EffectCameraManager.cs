using UnityEngine;
using System.Collections;

public class EffectCameraManager : MonoBehaviour
{
	// component
	private Camera effectCamera;
	
	void Start ()
	{
		effectCamera = Logic.GetChildObject (gameObject, "Camera").GetComponent<Camera> ();
	}

	public Camera Get ()
	{
		return effectCamera;
	}

	public void GUIOnEffect (GameObject effect, GameObject ngui)
	{
		GameObject temp = Instantiate (effect, new Vector3 (1000, 1000, 0), Quaternion.identity) as GameObject;

		Camera guiCam = NGUITools.FindCameraForLayer (ngui.layer);
		
		Vector3 position = effectCamera.ViewportToWorldPoint (guiCam.WorldToViewportPoint (ngui.transform.position));
		position.z += Config.EFFECT_Z;

		temp.transform.position = position;
	}
}
