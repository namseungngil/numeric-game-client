using UnityEngine;
using System.Collections;

public class InputEffectComponent : MonoBehaviour
{
	// const
	private const float DRAG_EFFECT_RESET_TIME = 0.1f;
	// gameobject
	public GameObject inputEffectGameObject;
	public GameObject dragEffectGameObject;
	// component
	private Camera effectCamera;
	// float
	private float dragEffectTime;
	private bool dragFlag;

	void Start ()
	{
		effectCamera = gameObject.GetComponentInChildren<Camera> ();
		dragEffectTime = DRAG_EFFECT_RESET_TIME;
		dragFlag = false;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			dragFlag = true;

			if (inputEffectGameObject != null) {
				Create (inputEffectGameObject);
			}
		}

		if (dragFlag) {
			dragEffectTime -= Time.deltaTime;
			if (dragEffectTime < 0) {
				dragEffectTime = DRAG_EFFECT_RESET_TIME;

				if (dragEffectGameObject != null) {
					Create (dragEffectGameObject);
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			dragFlag = false;
			dragEffectTime = DRAG_EFFECT_RESET_TIME;
		}
	}

	private void Create (GameObject gO)
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		float z = effectCamera.farClipPlane / 2;
		Vector3 xyz = new Vector3 (x, y, z);

		GameObject temp = Instantiate (gO, effectCamera.ScreenToWorldPoint (xyz), Quaternion.identity) as GameObject;
		temp.transform.parent = gameObject.transform;
	}

//	private Vector3 GetNGUIPostion ()
//	{
//		Vector3 uipos = NGUITools.FindCameraForLayer (gameObject.layer).camera.ScreenToWorldPoint (Input.mousePosition);
//		float x = uipos.x;
//		float y = uipos.y;
//		float z = uipos.z;
//
//		return new Vector3 (x, y, z);
//	}
}
