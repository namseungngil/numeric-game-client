using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Notification
{
	private const string CLASS_NAME = "com.riogames.unitygcmplugin.UnityTimerNotificationManager";
	private const string ID = "LocalNotification";

	/// <summary>
	/// Register the specified stringArray.
	/// </summary>
	/// <param name="stringArray">String array. [0]date [1]title [2]comment</param>
	public static void Register (params string[] stringArray)
	{
//		Debug.Log ("notification register");
#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				string str = string.Join (",", stringArray);
//				Debug.Log ("register : " + str);
				cls.CallStatic ("register", str);
			}
		}
#elif UNITY_IPHONE
		int[] dateArray = Date.Slice (stringArray[0]);
		DateTime dateTime = new DateTime (dateArray[0], dateArray[1], dateArray[2], dateArray[3], dateArray[4], dateArray[5]);

		IDictionary userInfo = new Dictionary<string, string> (1);
		userInfo["id"] = ID;
		
		LocalNotification localNotification = new LocalNotification ();
//		localNotification.applicationIconBadgeNumber = 1;
		localNotification.fireDate = dateTime;
		localNotification.alertBody = stringArray[2];
		localNotification.soundName = LocalNotification.defaultSoundName;
		localNotification.userInfo = userInfo;

		NotificationServices.ScheduleLocalNotification (localNotification);
#endif
	}

	public static void Unregister ()
	{
//		Debug.Log ("notification unregister");
#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			using (AndroidJavaClass cls = new AndroidJavaClass (CLASS_NAME)) {
				cls.CallStatic ("unregister");
			}
		}
#elif UNITY_IPHONE
		NotificationServices.CancelAllLocalNotifications ();
#endif
	}

	public static void CancelAll ()
	{
#if UNITY_IPHONE
		LocalNotification localNotification = new LocalNotification ();
		localNotification.applicationIconBadgeNumber = -1;
		NotificationServices.PresentLocalNotificationNow (localNotification);
		NotificationServices.ClearLocalNotifications ();
#endif
	}
}
