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
		Notification.CancelAll ();

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

	private void Login ()
	{
		HttpLogin ();
	}

	private void HttpLogin ()
	{
		HttpComponent httpManager = gameObject.GetComponent<HttpComponent> ();
		httpManager.Login (Config.REG_GCM_APNS_FACEBOOK_WAIT, true);
		httpManager.OnDone = (object obj) => {
			LoginCallback ();
		};
	}

	private void LoginCallback () {
		Debug.Log ("LoginCallback");

//		SSSceneManager.Instance.Screen (Config.LOGIN);
		SSSceneManager.Instance.GoHome ();
	}
}
