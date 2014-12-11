using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroManager : GameManager
{
	// const
	private const float WAIT_SECOND = 1f;
	// component
	private UILabel uiLabel;
	private HttpComponent httpComponent;
	private QueryModel queryModel;
	// variable
	private string text;
	private string defaultText;
	private string lastText;
	private float time;
	private int dicCount;
	
	void Start ()
	{
		dicCount = 50;
		time = 0;
		defaultText = "LOADING";
		lastText = "LOADING....";
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

	private void OnInitComplete ()
	{
		Debug.Log ("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		HttpLogin ();
	}
	
	private void OnHideUnity (bool isGameShown)
	{
		Debug.Log ("Is game showing? " + isGameShown);
		HttpLogin ();
	}

	private void Login ()
	{
		FB.Init (OnInitComplete, OnHideUnity);
	}

	private void HttpLogin ()
	{
		httpComponent.OnDone = (object obj) => {
			Sync ();
		};

		httpComponent.Login (0);
	}

	private void Sync ()
	{
		if (!FB.IsLoggedIn) {
			LoginCallback ();
			return;
		}

		DataTable dataTable = queryModel.QuestList ();

		if (dataTable.Rows.Count > 0) {
			List<Dictionary<string, string>> syncList = new List<Dictionary<string, string>> ();
			Dictionary<string, string> dic = new Dictionary<string, string> ();
			int index = 0;
			for (int i = 0; i < dataTable.Rows.Count; i++) {
				dic.Add (dataTable [i] [QueryModel.STAGE].ToString (), dataTable [i] [QueryModel.SCORE].ToString ());

				if (index >= dicCount) {
					index = 0;
					syncList.Add (dic);
					dic = new Dictionary<string, string> ();
//					dic.Clear ();
				}

				index++;
			}

			if (dic.Count > 0) {
				syncList.Add (dic);
			}

			SyncPut (syncList);

		} else {
			SyncGet ();
		}
	}

	private void SyncPut (List<Dictionary<string, string>> lDSS)
	{
		httpComponent.OnDone = (object obj) => {
			if (lDSS != null && lDSS.Count > 0) {
				SyncPut (lDSS);
			} else {
				SyncGet ();
			}
		};

		httpComponent.SyncPut (lDSS [0]);
		lDSS.RemoveAt (0);
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

		SSSceneManager.Instance.GoHome ();
	}
}
