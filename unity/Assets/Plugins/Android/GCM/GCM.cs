using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GCM
{
	private const string CLASS_NAME = "com.riogames.unitygcmplugin.UnityGCMRegister";
	private static GameObject _receiver = null;

	public static void Initialize ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			if (_receiver == null) {
				_receiver = new GameObject ("GCMReceiver");
				_receiver.AddComponent ("GCMReceiver");
			}
		}
	}
	
	public static void ShowToast (string message)
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass ("com.riogames.unitygcmplugin.Util")) {
				;
				cls.CallStatic ("showToast", message);
			}
		}
	}

	public static void Register (params string[] senderIds)
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				string senderIdsStr = string.Join (",", senderIds);
				cls.CallStatic ("register", senderIdsStr);
			}
		}
	}

	public static void Unregister ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				cls.CallStatic ("unregister");
			}
		}
	}

	public static bool IsRegistered ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				return cls.CallStatic<bool> ("isRegistered");
			}
		} else {
			return false;
		}
	}

	public static string GetRegistrationId ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				return cls.CallStatic<string> ("getRegistrationId");
			}
		} else {
			return null;
		}
	}

	public static void SetRegisteredOnServer (bool isRegistered)
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				cls.CallStatic ("setRegisteredOnServer", isRegistered);
			}
		}
	}

	public static bool IsRegisteredOnServer ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				return cls.CallStatic<bool> ("isRegisteredOnServer");
			}
		} else {
			return false;
		}
	}

	public static void SetRegisterOnServerLifespan (long lifespan)
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				cls.CallStatic ("setRegisterOnServerLifespan", lifespan);
			}
		}
	}

	public static long GetRegisterOnServerLifespan ()
	{
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				return cls.CallStatic<long> ("getRegisterOnServerLifespan");
			}
		} else {
			return 0L;
		}
	}

	public static void SetErrorCallback (System.Action<string> onError)
	{
		GCMReceiver._onError = onError;
	}


	public static void SetMessageCallback (System.Action<Dictionary<string, object>> onMessage)
	{
		GCMReceiver._onMessage = onMessage;
	}

	public static void SetRegisteredCallback (System.Action<string> onRegistered)
	{
		GCMReceiver._onRegistered = onRegistered;
	}


	public static void SetUnregisteredCallback (System.Action<string> onUnregistered)
	{
		GCMReceiver._onUnregistered = onUnregistered;
	}


	public static void SetDeleteMessagesCallback (System.Action<int> onDeleteMessages)
	{
		GCMReceiver._onDeleteMessages = onDeleteMessages;
	}
}
