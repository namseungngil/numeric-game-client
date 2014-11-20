using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HTTP;
using MiniJSON;

public class HttpComponent : MonoBehaviour
{
	// delegate
	public delegate void FinishedDelegate ();
	public FinishedDelegate OnDone
	{
		set {
			httpOnDone = value;
		}
	}
	private FinishedDelegate httpOnDone;

	// const
	private const string DEVICE_KEY = "deviceKey";
	private const string GCM_APNS = "GCMApns";
	private const string FACEBOOK_ID = "facebookID";
	// component
	private GCMComponent gCMComponent;
	private ApnsComponent apnsComponent;
	private FacebookManager facebookManager;
	private WWWClient http;
	// variable
	private string key = Config.ANDROID;
	private string id = null;
	private int regGCMApnsFacebookCount;
	private int regGCMApnsFacebookCountMax = 3;
	
	void Start ()
	{
		gCMComponent = gameObject.GetComponent<GCMComponent> ();
		apnsComponent = gameObject.GetComponent<ApnsComponent> ();
		facebookManager = gameObject.GetComponent<FacebookManager> ();
		regGCMApnsFacebookCount = 0;
	}

	private void Callback (bool flag)
	{
		if (flag) {
			Synchrozization ();
		} else {
			httpOnDone ();
		}
	}

	private void Request (bool flag)
	{
		http.OnDone = (WWW www) => {
			Debug.Log (www.text);
			Callback (flag);
		};
		
		// error
		http.OnFail = (WWW www) => {
			Debug.Log (www.error);
			Callback (flag);
		};
		
		// timed out
		http.OnDisposed = () => {
			Debug.Log ("Timed out");
			Callback (flag);
		};

		http.Request ();
	}

	private IEnumerator RegGCMApnsFacebook (float time, bool synchrozization)
	{
		yield return new WaitForSeconds (time);
		Debug.Log ("RegGCMApnsFacebook");
		
#if UNITY_ANDROID
		id = gCMComponent.GCMID ();
		key = Config.ANDROID;
#else
		id = apnsComponent.TokenID ();
		key = Config.IPHONE;
#endif
		
		if ((id == null || id == "") && regGCMApnsFacebookCount < regGCMApnsFacebookCountMax) {
			Debug.Log ("Re RegGCMApnsFacebook.");
			StartCoroutine (RegGCMApnsFacebook (Config.REG_GCM_APNS_FACEBOOK_WAIT, synchrozization));
			regGCMApnsFacebookCount++;
		}
		
		if (id != null && id != "") {
			string url = Config.URL + Config.LOGIN;
			
			http = new WWWClient (this, url);
			http.AddData (Config.KEY_NAME, Config.KEY);
			http.AddData (DEVICE_KEY, key);
			http.AddData (GCM_APNS, id);
			Debug.Log (Config.KEY_NAME + " : " + Config.KEY);
			Debug.Log (DEVICE_KEY + " : " + key);
			Debug.Log (GCM_APNS + " : " + id);

			bool flag = false;
			if (facebookManager.userID () != null) {
				flag = synchrozization;

				Debug.Log (FACEBOOK_ID + " : " + facebookManager.userID ());
				http.AddData (FACEBOOK_ID, facebookManager.userID ());
			}

			http.OnDone = (WWW www) => {
				Debug.Log (www.text);
				httpOnDone ();
			};
			
			// error
			http.OnFail = (WWW www) => {
				Debug.Log (www.error);
				httpOnDone ();
			};
			
			// timed out
			http.OnDisposed = () => {
				Debug.Log ("Timed out");
				httpOnDone ();
			};
			
			http.Request ();
		} else {
			httpOnDone ();
		}
	}

	private void Synchrozization ()
	{
		Debug.Log ("SetSynchrozization");
		httpOnDone ();
		////////////////////////////////////

		////////////////////////////////////
	}

	private void QuestUser (List<string> list)
	{
		if (facebookManager.userID () == null) {
			httpOnDone ();
		}

		string json = Json.Serialize (list);
		Debug.Log (json);

		string url = Config.URL + Config.BATTLE;
		http = new WWWClient (this, url);
		http.AddData ("json", json);

		Request (false);
	}

	public void Result (List<string> list)
	{
		Debug.Log ("Http result");
	}

	public void Login (float regGCMApnsFacebookWaitTime, bool synchrozization = false)
	{
		Debug.Log ("Http login");
		StartCoroutine (RegGCMApnsFacebook (regGCMApnsFacebookWaitTime, synchrozization));
	}

	public void Clear (string id, string stage, string score)
	{

	}
}
