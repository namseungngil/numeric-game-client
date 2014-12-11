using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectCameraManager : MonoBehaviour
{
	// class
	public class EffectCamera
	{
		public GameObject effect;
		public float fx = 0;
		public float fy = 0;

		public EffectCamera (GameObject gO)
		{
			effect = gO;
		}
	};
	// component
	private Camera effectCamera;
	// array
	private List<ParticleSystem> pSList;

	void Start ()
	{
		effectCamera = Logic.GetChildObject (gameObject, "Camera").GetComponent<Camera> ();
	}

	public Camera Get ()
	{
		return effectCamera;
	}

	public void GUIOnEffect (GameObject effect, GameObject ngui, float fX = 0, float fY = 0)
	{
		GameObject temp = Instantiate (effect, new Vector3 (1000, 1000, 0), Quaternion.identity) as GameObject;

		Camera guiCam = NGUITools.FindCameraForLayer (ngui.layer);
		
		Vector3 position = effectCamera.ViewportToWorldPoint (guiCam.WorldToViewportPoint (ngui.transform.position));
		position.x += fX;
		position.y += fY;
		position.z += Config.EFFECT_Z;

		temp.transform.position = position;
		temp.transform.parent = effectCamera.transform;
	}

	public void GUIOnEffect (GameObject[] effect, GameObject ngui)
	{
		foreach (GameObject gObj in effect) {
			GUIOnEffect (gObj, ngui);
		}
	}

	public IEnumerator GUIOnEffect (List<EffectCamera> lE, GameObject ngui)
	{
		GUIOnEffect (lE [0].effect, ngui, lE [0].fx, lE [0].fy);
		lE.RemoveAt (0);

		yield return new WaitForSeconds (0.2f);

		if (lE != null && lE.Count > 0) {
			StartCoroutine (GUIOnEffect (lE, ngui));
		}
	}

	public void GUIOnOverEffect (GameObject effect, GameObject ngui)
	{
		List<EffectCamera> effectList = new List<EffectCamera> ();
		for (int i = 0; i < 5; i++) {
			EffectCameraManager.EffectCamera tempeC = new EffectCameraManager.EffectCamera (effect);
			if (i == 0) {
				effectList.Add (tempeC);
				continue;
			}

			if (i < 3) {
				tempeC.fx = 1.5f;
			} else {
				tempeC.fx = -1.5f;
			}

			if (i % 2 == 1) {
				tempeC.fy = 1.5f;
			} else {
				tempeC.fy = -1.5f;
			}

			effectList.Add (tempeC);
		}

		StartCoroutine (GUIOnEffect (effectList, ngui));
	}

	public void Destory ()
	{
		foreach (ParticleSystem pS in effectCamera.GetComponentsInChildren<ParticleSystem> ()) {
			GameObject.Destroy (pS.gameObject);
		}

		if (pSList != null && pSList.Count > 0) {
			foreach (ParticleSystem ps in pSList) {
				GameObject.Destroy (ps.gameObject);
			}
			
			pSList.Clear ();
			pSList = null;
		}
	}

	public void SetActive (bool flag)
	{
		if (pSList == null || pSList.Count <= 0) {
			pSList = new List<ParticleSystem> ();
			foreach (ParticleSystem pS in effectCamera.GetComponentsInChildren<ParticleSystem> ()) {
				pSList.Add (pS);
			}
		}

		if (pSList != null && pSList.Count > 0) {
			foreach (ParticleSystem pS in pSList) {
				pS.gameObject.SetActive (flag);
			}
		}
	}
}
