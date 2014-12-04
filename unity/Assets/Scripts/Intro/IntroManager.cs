using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroManager : MonoBehaviour
{
	// const
	private const float WAIT_SECOND = 1f;
	// component
	private UILabel uiLabel;
	private HttpComponent httpComponent;
	private QueryModel queryModel;
	// variable
	private string text;
	private string defaultText = "LOADING";
	private string lastText = "LOADING....";
	private float time;
	
	void Start ()
	{
		Notification.CancelAll ();

		time = 0;
		uiLabel = GameObject.Find (Config.LABEL).GetComponent<UILabel> ();
		text = defaultText;

		httpComponent = gameObject.GetComponent<HttpComponent> ();
		queryModel = QueryModel.Instance ();

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
		httpComponent.OnDone = (object obj) => {
			Sync ();
		};

		httpComponent.Login (Config.REG_GCM_APNS_FACEBOOK_WAIT);
	}

	private void Sync ()
	{
		if (!FB.IsLoggedIn) {
			LoginCallback ();
			return;
		}

		DataTable dataTable = queryModel.QuestList ();

		if (dataTable.Rows.Count > 0) {
			httpComponent.OnDone = (object obj) => {
				SyncGet ();
			};

			Dictionary<string, string> dic = new Dictionary<string, string> ();
			for (int i = 0; i < dataTable.Rows.Count; i++) {
				dic.Add (dataTable [i] [QueryModel.STAGE].ToString (), dataTable [i] [QueryModel.SCORE].ToString ());
			}

			httpComponent.SyncPut (dic);
		} else {
			SyncGet ();
		}
	}

	private void SyncGet ()
	{
		httpComponent.OnDone = (object obj) => {
			QueryModel queryModel = QueryModel.Instance ();
				
			Dictionary<string, string> dic;
			if ((dic = obj as Dictionary<string, string>) != null) {
				queryModel.SyncGet (dic);
			}

			LoginCallback ();
		};
			
		httpComponent.SyncGet ();
	}

	private void LoginCallback ()
	{
		Debug.Log ("LoginCallback");

//		SSSceneManager.Instance.Screen (Config.LOGIN);
		SSSceneManager.Instance.GoHome ();
	}
}
