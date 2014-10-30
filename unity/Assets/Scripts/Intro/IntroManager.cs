using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
	// const
	private const float WAIT_SECOND = 1f;
	// component
	private UILabel uiLabel;
	// variable
	private string text;
	private string defaultText = "Loading";
	private string lastText = "Loading....";
	private float time;
	
	void Start ()
	{
		time = 0;

		uiLabel = GameObject.Find (Config.LABEL).GetComponent<UILabel> ();

		text = defaultText;
		Login ();

	}

	void Update ()
	{
		time += Time.deltaTime;
		if (time > WAIT_SECOND) {
			time = 0;
			if (text == lastText) {
				text = defaultText;
			}
			
			text += ".";
			uiLabel.text = text;
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
		QueryModel dataQuery = QueryModel.Instance ();
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

		SSSceneManager.Instance.Screen (Config.LOGIN);

//		StartCoroutine (DisplayLoadingScreen (LoadingData.currentLevel));
	}
}
