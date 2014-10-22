using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
	private const float WAIT_SECOND = 1f;
	private UILabel uiLabel;
	private string text;
	private string defaultText = "Loading";
	private string lastText = "Loading....";
	
	void Start ()
	{
		uiLabel = GameObject.Find (Config.LABEL).GetComponent<UILabel> ();

		text = defaultText;
		StartCoroutine (SetUILabel ());
		if (LoadingData.currentLevel == Config.LOGIN) {
			Login ();
		} else {
			StartCoroutine (DisplayLoadingScreen (LoadingData.currentLevel));
		}
	}

	private IEnumerator SetUILabel ()
	{
		yield return new WaitForSeconds (WAIT_SECOND);
		
		if (text == lastText) {
			text = defaultText;
		}
		
		text += ".";
		uiLabel.text = text;
		
		StartCoroutine (SetUILabel ());
	}
	
	private IEnumerator DisplayLoadingScreen (string level)
	{
		yield return new WaitForSeconds (WAIT_SECOND);
		AsyncOperation asyncOperation = Application.LoadLevelAsync (level);
		while (!asyncOperation.isDone) {
			// Create prograss bar ?
			int loadProgress = (int)(asyncOperation.progress * 100);
//			Debug.Log (loadProgress);
			
			yield return null;
		}
	}

	private void Login ()
	{
		MasterDataLogin ();
		HttpLogin ();
	}

	private void MasterDataLogin ()
	{
		DataQuery dataQuery = DataQuery.Instance ();
		dataQuery.MasterData ();
	}

	private void HttpLogin ()
	{
		HttpComponent httpManager = gameObject.GetComponent<HttpComponent> ();
		httpManager.Login (Config.REG_GCM_APNS_FACEBOOK_WAIT, true);
		httpManager.OnDone = () => {
			LoginCallback ();
		};
	}

	private void LoginCallback () {
		Debug.Log ("LoginCallback");

		StartCoroutine (DisplayLoadingScreen (LoadingData.currentLevel));
	}
}
