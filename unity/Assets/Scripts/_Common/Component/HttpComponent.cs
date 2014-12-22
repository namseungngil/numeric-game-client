using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HTTP;
using MiniJSON;

public class HttpComponent : MonoBehaviour
{
	// delegate
	public delegate void FinishedDelegate (object obj);

	public FinishedDelegate OnDone {
		set {
			httpOnDone = value;
		}
	}

	private FinishedDelegate httpOnDone;

	// const
	private const string DEVICE_KEY = "deviceKey";
	private const string GCM_APNS = "GCMApns";
	private const string FACEBOOK_ID = "facebookID";
	private const string FACEBOOK_JSON = "facebookJson";
	private const string RESULT = "result";
	private const string COMMA = ",";
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

	private IEnumerator RegGCMApnsFacebook (float time)
	{
		yield return new WaitForSeconds (time);
//		Debug.Log ("RegGCMApnsFacebook");
		
#if UNITY_ANDROID
		id = gCMComponent.GCMID ();
		key = Config.ANDROID;
#else
		id = apnsComponent.TokenID ();
		key = Config.IPHONE;
#endif

		if ((id == null || id == "") && regGCMApnsFacebookCount < regGCMApnsFacebookCountMax) {
//			Debug.Log ("Re RegGCMApnsFacebook.");
			StartCoroutine (RegGCMApnsFacebook (Config.REG_GCM_APNS_FACEBOOK_WAIT));
			regGCMApnsFacebookCount++;
		}
		
		if (id != null && id != "") {
			string url = Config.URL + Config.LOGIN;
			
			http = new WWWClient (this, url);
			http.AddData (Config.KEY_NAME, Config.KEY);
			http.AddData (DEVICE_KEY, key);
			http.AddData (GCM_APNS, id);
//			Debug.Log (Config.KEY_NAME + " : " + Config.KEY);
//			Debug.Log (DEVICE_KEY + " : " + key);
//			Debug.Log (GCM_APNS + " : " + id);

			if (facebookManager.userID () != null) {
//				Debug.Log (FACEBOOK_ID + " : " + facebookManager.userID ());
				http.AddData (FACEBOOK_ID, facebookManager.userID ());
			}

			http.OnDone = (WWW www) => {
//				Debug.Log (www.text);
				httpOnDone (null);
			};
			
			// error
			http.OnFail = (WWW www) => {
//				Debug.Log (www.error);
				httpOnDone (null);
			};
			
			// timed out
			http.OnDisposed = () => {
//				Debug.Log ("Timed out");
				httpOnDone (null);
			};
			
			http.Request ();
		} else {
			httpOnDone (null);
		}
	}

	private void OverCallback (SSController s = null)
	{
//		Debug.Log ("OverCallback");

		http.OnDone = (WWW www) => {
//			Debug.Log (www.text);
			if (s != null) {
				s.gameObject.GetComponent<UIManager> ().Cancel ();
			}
			httpOnDone (null);
		};
		
		http.OnFail = (WWW www) => {
//			Debug.Log (www.error);
			if (s != null) {
				s.gameObject.GetComponent<UIManager> ().Cancel ();
			}
			httpOnDone (null);
		};
		
		http.OnDisposed = () => {
//			Debug.Log ("Timed out");
			if (s != null) {
				s.gameObject.GetComponent<UIManager> ().Cancel ();
			}
			httpOnDone (null);
		};
		
		http.Request ();
	}

	public void Login (float regGCMApnsFacebookWaitTime)
	{
//		Debug.Log ("Http login");
		StartCoroutine (RegGCMApnsFacebook (regGCMApnsFacebookWaitTime));
	}

	public void SyncPut (Dictionary<string, string> d)
	{
//		Debug.Log ("HttpComponent SyncPut");
		string url = Config.URL + Config.INTRO + "put";
		http = new WWWClient (this, url);
		http.AddData (Config.KEY_NAME, Config.KEY);
		http.AddData (FACEBOOK_ID, FB.UserId);
		
		string temp = Json.Serialize (d);
//		Debug.Log (temp);
		http.AddData (FACEBOOK_JSON, temp);

		http.OnDone = (WWW www) => {
//			Debug.Log (www.text);
			httpOnDone (null);
		};
		
		http.OnFail = (WWW www) => {
//			Debug.Log (www.error);
			httpOnDone (null);
		};
		
		http.OnDisposed = () => {
//			Debug.Log ("Time out");
			httpOnDone (null);
		};

		http.Request ();
	}

	public void SyncGet ()
	{
		if (facebookManager.userID () == null) {
			httpOnDone (null);
			return;
		}

		string url = Config.URL + Config.INTRO + "get";
		
		http = new WWWClient (this, url);
		http.AddData (Config.KEY_NAME, Config.KEY);
		http.AddData (FACEBOOK_ID, facebookManager.userID ());

		http.OnDone = (WWW www) => {
//			Debug.Log (www.text);
			Dictionary <string, string> dic = new Dictionary<string, string> ();
			IDictionary iDictionary = (IDictionary)Json.Deserialize (www.text);
			
			IList iList = (IList)iDictionary [RESULT];
			if (iList.Count > 0) {
				foreach (string str in iList) {
					if (str == "null") {
						httpOnDone (null);
						return;
					}
					string[] tempString = str.Split (new string[] {COMMA}, StringSplitOptions.None);
					dic.Add (tempString [0], tempString [1]);
				}
				httpOnDone (dic);
			} else {
				httpOnDone (null);
			}
		};
		
		http.OnFail = (WWW www) => {
//			Debug.Log (www.error);
			httpOnDone (null);
		};
		
		http.OnDisposed = () => {
//			Debug.Log ("Time out");
			httpOnDone (null);
		};
		
		http.Request ();
	}
	
	public void Over (Dictionary<string, string> data, bool popupFlag = true)
	{
//		Debug.Log ("HttpComponent Over");

		string url = Config.URL + Config.OVER;
		http = new WWWClient (this, url);
		http.AddData (Config.KEY_NAME, Config.KEY);
		http.AddData (FACEBOOK_ID, FB.UserId);
		foreach (KeyValuePair<string, string> kVP in data) {
			http.AddData (kVP.Key, kVP.Value);
		}

		if (popupFlag) {
			SSSceneManager.Instance.PopUp (Config.LOADING, null, OverCallback);
		} else {
			OverCallback ();
		}
	}

	public void StartGame (List<string> l, string stage)
	{
//		Debug.Log ("HttpComponent Start");
		string url = Config.URL + Config.START;
		http = new WWWClient (this, url);
		http.AddData (Config.KEY_NAME, Config.KEY);
		http.AddData (FACEBOOK_ID, FB.UserId);
		http.AddData (QueryModel.STAGE, stage);

		string temp = Json.Serialize (l);
//		Debug.Log (temp);
		http.AddData (FACEBOOK_JSON, temp);

		http.OnDone = (WWW www) => {
//			Debug.Log (www.text);
			Dictionary <string, int> dic = new Dictionary<string, int> ();
			IDictionary iDictionary = (IDictionary)Json.Deserialize (www.text);

			IList iList = (IList)iDictionary [RESULT];
			if (iList.Count > 0) {
				foreach (string str in iList) {
					if (str == "null") {
						httpOnDone (null);
						return;
					}
					string[] tempString = str.Split (new string[] {COMMA}, StringSplitOptions.None);
					dic.Add (tempString [0], int.Parse (tempString [1]));
				}
				httpOnDone (dic);
			} else {
				httpOnDone (null);
			}
		};

		http.OnFail = (WWW www) => {
//			Debug.Log (www.error);
			httpOnDone (null);
		};

		http.OnDisposed = () => {
//			Debug.Log ("Time out");
			httpOnDone (null);
		};

		http.Request ();
	}
}
