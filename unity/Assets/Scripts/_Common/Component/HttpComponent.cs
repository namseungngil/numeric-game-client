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

			if (facebookManager.userID () != null) {
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

	public void Login (float regGCMApnsFacebookWaitTime, bool synchrozization = false)
	{
		Debug.Log ("Http login");
		StartCoroutine (RegGCMApnsFacebook (regGCMApnsFacebookWaitTime, synchrozization));
	}

	public void Over (Dictionary<string, string> data)
	{
		SSSceneManager.Instance.PopUp (Config.LOADING);
		string url = Config.URL + Config.OVER;
		http = new WWWClient (this, url);
		http.AddData (Config.KEY_NAME, Config.KEY);
		http.AddData (FACEBOOK_ID, FB.UserId);
		foreach (KeyValuePair<string, string> kVP in data) {
			http.AddData (kVP.Key, kVP.Value);
		}

		http.OnDone = (WWW www) => {
			Debug.Log (www.text);
			SSSceneManager.Instance.Close ();
		};

		http.OnFail = (WWW www) => {
			Debug.Log (www.error);
			SSSceneManager.Instance.Close ();
		};

		http.OnDisposed = () => {
			Debug.Log ("Timed out");
			SSSceneManager.Instance.Close ();
		};

		http.Request ();
	}
}
