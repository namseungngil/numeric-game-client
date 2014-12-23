using UnityEngine;
using System.Collections;

public class ApnsComponent : MonoBehaviour
{
	private string tokenID = null;
#if UNITY_IPHONE
	void Start ()
	{
		InfobipPush.Initialize ();
		InfobipPush.Register ("", "");

		tokenID = InfobipPushInternal.TokenKey;
//		Debug.Log ("Apns token : " + tokenID);
	
//		string deviceId = InfobipPush.DeviceId;
//		ScreenPrinter.Print(deviceId);
	}

	void Update ()
	{
		if (tokenID == null && InfobipPushInternal.TokenKey != null) {
//			Debug.Log ("token : " + InfobipPushInternal.TokenKey);
			tokenID = InfobipPushInternal.TokenKey;
		}
	}

//	void OnGUI ()
//	{
//		if (GUI.Button (new Rect (10, 10, 200, 200), "Token id")) {
//			InfobipPush.Register ("", "");
//			Debug.Log (tokenID);
//		}
//	}
#endif

	public string TokenID ()
	{
		if (tokenID != null) {
			tokenID = tokenID.Replace("<", "");
			tokenID = tokenID.Replace(">", "");
			tokenID = tokenID.Replace(" ", "");
		}
		return tokenID;
	}
}
