using UnityEngine;
using System.Collections;

public class LoadingPopup : MonoBehaviour
{
	private static LoadingPopup instance;

	private GameObject panel1000;

	public static LoadingPopup Instance ()
	{
		if (instance == null) {
			instance = new LoadingPopup ();
		}

		return instance;
	}

	private IEnumerator Popup ()
	{
		AsyncOperation asyncOperation = Application.LoadLevelAdditiveAsync (Config.LOADINGPOPUP);

		yield return asyncOperation;

		panel1000 = NGUITools.AddChild (GameObject.Find (Config.UIROOT), GameObject.Find (Config.PANEL1000));
		GameObject temp = GameObject.Find ("UI Root");
		if (temp != null) {
			Destroy (temp);
		}
	}

	public void Create (MonoBehaviour mB)
	{
		if (panel1000 == null) {
			mB.StartCoroutine (Popup ());
		}
	}

	public void Close ()
	{
		if (panel1000 != null) {
			Destroy (panel1000);
		}
	}
}
