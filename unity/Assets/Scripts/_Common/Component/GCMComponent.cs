using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GCMComponent : MonoBehaviour
{
	private string gcmID = null;
#if UNITY_ANDROID
	// Project Number on Google API Console
	private string[] SENDER_IDS = {"757437024240"};
	private string _text = "(null)";
	
	// Use this for initialization
	void Start ()
	{
		// Create receiver game object
		GCM.Initialize ();
		
		// Set callbacks
		GCM.SetErrorCallback ((string errorId) => {
			Debug.Log ("Error!!! " + errorId);
//			GCM.ShowToast ("Error!!!");
			_text = "Error: " + errorId;
		});
		
		GCM.SetMessageCallback ((Dictionary<string, object> table) => {
			Debug.Log ("Message!!!");
//			GCM.ShowToast ("Message!!!");
			_text = "Message: " + System.Environment.NewLine;
			foreach (var key in  table.Keys) {
				_text += key + "=" + table [key] + System.Environment.NewLine;
			}
		});
		
		GCM.SetRegisteredCallback ((string registrationId) => {
			Debug.Log ("Registered!!! " + registrationId);
			gcmID = registrationId;
//			GCM.ShowToast ("Registered!!!");
			_text = "Register: " + registrationId; 
		});
		
		GCM.SetUnregisteredCallback ((string registrationId) => {
			Debug.Log ("Unregistered!!! " + registrationId);
//			GCM.ShowToast ("Unregistered!!!");
			_text = "Unregister: " + registrationId;
		});
		
		GCM.SetDeleteMessagesCallback ((int total) => {
			Debug.Log ("DeleteMessages!!! " + total);
//			GCM.ShowToast ("DeleteMessaged!!!");
			_text = "DeleteMessages: " + total;
		});

		GCM.Register (SENDER_IDS);
		gcmID = GCM.GetRegistrationId ();
	}	
#endif

	public string GCMID ()
	{
		return gcmID;
	}
}
