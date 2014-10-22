using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfobipPushDemo : MonoBehaviour
{   
	private const string applicationID = "063bdab564eb";
	private const string applicationSecret = "a5cf819f36e2";
	InfobipPushMediaViewCustomization customiz = null;
	private const int rowNumber = 16;
	private GUIStyle labelStyle = new GUIStyle ();
	private float centerX = Screen.width / 2;
	private float buttonWidth = Screen.width / 4;
	private float buttonHeight = Screen.height / 24;
	private float buttonSpace = Screen.width / 20;
	private float[] rowY = new float[rowNumber];
	private LocationService locationService = new LocationService ();
	private ScreenOrientation lastScreenOrientation;
	private float lastScreenWidth = Screen.width;

	void Start ()
	{   
		lastScreenOrientation = Screen.orientation;
		InfobipPush.Initialize ();

		for (int i = 0; i < rowNumber; i++) {
			rowY [i] = i * (buttonHeight + buttonHeight / 4);
		}
		labelStyle.fontSize = 24;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;

//				locationService.Start ();

		InfobipPush.OnNotificationReceived = (notif) => {
			ScreenPrinter.Print ("Notif: " + notif.Message + " #" + UnityEngine.Random.value);
			//            Dictionary<string,object> addInfo = (Dictionary<string,object>)notif.AdditionalInfo;
			if (InfobipPush.IsDefaultMessageHandlingOverriden ()) {
				ScreenPrinter.Print ("IBPush - Default message handling overriden, notifying notif opened w/ id: " + notif.NotificationId);
				InfobipPush.NotifyNotificationOpened (notif.NotificationId);
			}
          
		};

		InfobipPush.OnNotificationOpened = (notif) => {
			ScreenPrinter.Print ("Notification OPENED: " + notif.Message);
			bool isMediaNotification = notif.isMediaNotification ();
			if (isMediaNotification) {
				ScreenPrinter.Print ("IBPush - Is Media notification: " + isMediaNotification);
				#if UNITY_IPHONE
                customiz = new InfobipPushMediaViewCustomization 
                {
                    X = 20,
                    Y = 20,
                    Width = 250,
                    Height = 350,
                    Shadow = false,
                    Radius = 50,
                    DismissButtonSize = 40,
                    ForegroundColor = new Color(1.0f, 0, 0, 1.0f),
                    BackgroundColor = new Color(0, 1.0f, 0, 1.0f)
                };
                InfobipPush.AddMediaView(notif, customiz);
				#endif

				#if UNITY_ANDROID
            if (isMediaNotification){
                 InfobipPush.AddMediaView(notif, customiz);
            }
				#endif
			}
		};

		InfobipPush.OnRegistered = () => {
			InfobipPushLocation.BackgroundLocationUpdateModeEnabled = true;
			InfobipPushLocation.IBSetLiveGeoAccuracy (1.0);
			InfobipPushLocation.IBEnableLocation ();
			InfobipPushLocation.IBEnableLiveGeo ();
			ScreenPrinter.Print (("IBPush - Successfully registered!"));
		};
		InfobipPush.OnUnregistered = () => {
			ScreenPrinter.Print (("IBPush - Successfully unregistered!"));
		};
		InfobipPush.OnError = (errorCode) => {
			if (errorCode == "7") {
				ScreenPrinter.Print (("IBPush - Received silent notification..."));
			}
			ScreenPrinter.Print (("IBPush - ERROR: " + errorCode));
		};
		InfobipPush.OnUserDataSaved = () => {
			ScreenPrinter.Print (("IBPush - User data saved"));
		};
		InfobipPush.OnUnregistered = () => {
			ScreenPrinter.Print ("IBPush - Successfully unregistered!");
		};
		InfobipPush.OnGetChannelsFinished = (channels) => {
			ScreenPrinter.Print ("IBPush - You are registered to: " + channels.ToString ());
		};
		InfobipPush.OnUnreceivedNotificationReceived = (notification) => {
			ScreenPrinter.Print ("IBPush - Unreceived notification: ");
			ScreenPrinter.Print (notification);
		};
		InfobipPush.OnRegisteredToChannels += () => {
			ScreenPrinter.Print (("IBPush - Successfully registered to CHANNELS!"));
		};
		InfobipPush.OnNotifyNotificationOpenedFinished = () => {
			ScreenPrinter.Print (("IBPush - Successfully notified notification opened event to IB server!"));
		};


	}

	void OnGUI ()
	{
		if (!Screen.orientation.Equals (lastScreenOrientation) && lastScreenWidth != Screen.width) {
			centerX = Screen.width / 2;
			buttonWidth = Screen.width / 4;
			buttonHeight = Screen.height / 24;
			buttonSpace = Screen.width / 20;
			lastScreenOrientation = Screen.orientation;
			lastScreenWidth = Screen.width;
		}

		// Title
		GUI.Label (new Rect (centerX - buttonWidth * 2, 0, Screen.width, buttonHeight), "Infobip Push Demo", labelStyle);

		// First row
		if (GUI.Button (new Rect (centerX - buttonWidth - buttonSpace, rowY [1], buttonWidth, buttonHeight), "Enable Debug Mode")) {
			InfobipPush.LogMode = true;
			InfobipPush.SetLogModeEnabled (true, 0);
		}
		if (GUI.Button (new Rect (centerX + buttonSpace, rowY [1], buttonWidth, buttonHeight), "Disable Debug Mode")) {
			InfobipPush.LogMode = false;
		}
        
		// Second row
		if (GUI.Button (new Rect (buttonSpace, rowY [2], buttonWidth, buttonHeight), "Initialize Push")) {
			InfobipPushRegistrationData regData = new InfobipPushRegistrationData {
	                UserId = "test New User", 
	                Channels = new string[] {"a", "b", "c", "d", "News"}
	            };
			Debug.Log (regData);
			ScreenPrinter.print (regData);
			InfobipPush.Register (applicationID, applicationSecret, regData);
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [2], buttonWidth, buttonHeight), "Is Registered")) {
			bool isRegistered = InfobipPush.IsRegistered ();
			ScreenPrinter.Print (isRegistered);
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [2], buttonWidth, buttonHeight), "Unregister")) {
			InfobipPush.Unregister ();
		}

		// Third row
		if (GUI.Button (new Rect (buttonSpace, rowY [3], buttonWidth, buttonHeight), "Device Id")) {
			string deviceId = InfobipPush.DeviceId;
			ScreenPrinter.Print (deviceId);
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [3], buttonWidth, buttonHeight), "User Id")) {
			string userId = InfobipPush.UserId;
			ScreenPrinter.Print (userId);
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [3], buttonWidth, buttonHeight), "Set User Id")) {
			InfobipPush.UserId = "Malisica " + UnityEngine.Random.value.ToString ();
		}

		// Fourth row
		if (GUI.Button (new Rect (centerX - buttonWidth - buttonSpace, rowY [4], buttonWidth, buttonHeight), "Register To Channels")) {
			string[] channels = new string[8] {
                "a", "b", "c", "d", "e",
                "F", "G", "H"
            };
			InfobipPush.RegisterToChannels (channels, false);
		}
		if (GUI.Button (new Rect (centerX + buttonSpace, rowY [4], buttonWidth, buttonHeight), "Get Registered Channels")) {
			InfobipPush.BeginGetRegisteredChannels ();
		}

		// Fifth row
		if (GUI.Button (new Rect (centerX - buttonWidth - buttonSpace, rowY [5], buttonWidth, buttonHeight), "Get Unreceived Notifications")) {
			InfobipPush.GetListOfUnreceivedNotifications ();
		}
		if (GUI.Button (new Rect (centerX + buttonSpace, rowY [5], buttonWidth, buttonHeight), "Set Badge Number")) {
			InfobipPush.SetBadgeNumber (0);
		}

		// Sixth row
		if (GUI.Button (new Rect (buttonSpace, rowY [6], buttonWidth, buttonHeight), "Enable Location")) {
			InfobipPushLocation.EnableLocation ();
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [6], buttonWidth, buttonHeight), "Disable Location")) {
			InfobipPushLocation.DisableLocation ();
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [6], buttonWidth, buttonHeight), "Is Loc. Enabled")) {
			bool isLocation = InfobipPushLocation.IsLocationEnabled ();
			ScreenPrinter.Print ("isLocation = " + isLocation);
		}
    
		// Seventh row
		if (GUI.Button (new Rect (buttonSpace, rowY [7], buttonWidth, buttonHeight), "Enable Background Loc.")) {
			InfobipPushLocation.BackgroundLocationUpdateModeEnabled = true;
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [7], buttonWidth, buttonHeight), "Disable Background Loc.")) {
			InfobipPushLocation.BackgroundLocationUpdateModeEnabled = false;
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [7], buttonWidth, buttonHeight), "Background Location")) {
			bool back = InfobipPushLocation.BackgroundLocationUpdateModeEnabled;
			ScreenPrinter.Print (back);
		}

		// Eighth row
		if (GUI.Button (new Rect (buttonSpace, rowY [8], buttonWidth, buttonHeight), "Time Update")) {
			int time = InfobipPushLocation.LocationUpdateTimeInterval;
			ScreenPrinter.Print (time);
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [8], buttonWidth, buttonHeight), "Set Time Update")) {
			InfobipPushLocation.LocationUpdateTimeInterval = 5;
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [8], buttonWidth, buttonHeight), "Share Location")) {
			ScreenPrinter.Print ("IBPush - Location Enabled: " + (InfobipPushLocation.LocationEnabled ? "true" : "false"));
			LocationInfo location = locationService.lastData;
			InfobipPushLocation.ShareLocation (location);
           
		}

		// Ninth row
		if (GUI.Button (new Rect (buttonSpace, rowY [9], buttonWidth, buttonHeight), "Enable Live Geo")) {
			InfobipPushLocation.LiveGeo = true;
			ScreenPrinter.Print ("Live geo is enabled");
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [9], buttonWidth, buttonHeight), "Disable Live Geo")) {
			InfobipPushLocation.LiveGeo = false;
			ScreenPrinter.Print ("Live geo is disabled");
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [9], buttonWidth, buttonHeight), "Is Live Geo")) {
			ScreenPrinter.Print (InfobipPushLocation.LiveGeo);
		}
        
		// Tenth row
		if (GUI.Button (new Rect (buttonSpace, rowY [10], buttonWidth, buttonHeight), "Number Of Regions")) {
			int regions = InfobipPushLocation.NumberOfCurrentLiveGeoRegions ();
			ScreenPrinter.Print ("Number Of Regions: " + regions.ToString ());
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [10], buttonWidth, buttonHeight), "Stop Live Geo Monitoring")) {
			int regions = InfobipPushLocation.StopLiveGeoMonitoringForAllRegions ();
			ScreenPrinter.Print ("Stop Live Geo Monitoring for all Regions: " + regions.ToString ());
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [10], buttonWidth, buttonHeight), "Set Accuracy")) {
			double accur = 100.43;
			InfobipPushLocation.IBSetLiveGeoAccuracy (accur);
			ScreenPrinter.Print ("Live geo Accuracy is set to " + accur.ToString ());
		}

		// Eleventh row
		if (GUI.Button (new Rect (buttonSpace, rowY [11], buttonWidth, buttonHeight), "Enable Timezone Update")) {
			InfobipPush.SetTimezoneOffsetAutomaticUpdateEnabled (true);
		}
		if (GUI.Button (new Rect (centerX - buttonWidth / 2.0f, rowY [11], buttonWidth, buttonHeight), "Disable Timezone Update")) {
			InfobipPush.SetTimezoneOffsetAutomaticUpdateEnabled (false);
		}
		if (GUI.Button (new Rect (centerX + buttonWidth / 2.0f + buttonSpace, rowY [11], buttonWidth, buttonHeight), "Set Timezone Offset")) {
			InfobipPush.SetTimezoneOffsetInMinutes (5);
		}
		#if UNITY_ANDROID
        // Twelfth row
        if (GUI.Button(new Rect(buttonSpace, rowY [12], buttonWidth, buttonHeight), "Enable Custom Location"))
        {
            InfobipPushLocation.UseCustomLocationService(true);
        }
        if (GUI.Button(new Rect(centerX - buttonWidth / 2.0f, rowY [12], buttonWidth, buttonHeight), "Disable Custom Location"))
        {
            InfobipPushLocation.UseCustomLocationService(false);
        }
        if (GUI.Button(new Rect(centerX + buttonWidth / 2.0f + buttonSpace, rowY [12], buttonWidth, buttonHeight), "Is Custom Location"))
        {
            bool custom = InfobipPushLocation.IsUsingCustomLocationService();
            ScreenPrinter.Print("Use custom location: " + custom);
        }

        // Thirteenth row
        if (GUI.Button(new Rect(buttonSpace, rowY [13], buttonWidth, buttonHeight), "Enable Override MesgHandling"))
        {
            InfobipPush.SetOverrideDefaultMessageHandling(true);
            ScreenPrinter.Print("Override Message Handling is enabled");
        }
        if (GUI.Button(new Rect(centerX - buttonWidth / 2.0f, rowY [13], buttonWidth, buttonHeight), "Disable Override MesgHandling"))
        {
            InfobipPush.SetOverrideDefaultMessageHandling(false);
            ScreenPrinter.Print("Override Message Handling is disbled");
        }
        if (GUI.Button(new Rect(centerX + buttonWidth / 2.0f + buttonSpace, rowY [13], buttonWidth, buttonHeight), "Builder"))
        {
            ScreenPrinter.Print("Push Notification Builder");
            InfobipPushBuilder builder = new InfobipPushBuilder();
            builder.TickerText = "New ticker text";
            TimeSpan startTime = new TimeSpan(12,0,0);
            TimeSpan endTime = new TimeSpan(8,50,0);
            builder.SetQuietTime(startTime, endTime);
            builder.SetLightsOnOffDurationsMs(2000, 200);
            builder.ApplicationName = "New application name";
            builder.SetLayoutId("notification_layout","layout","com.infobip.unity.demo");
            builder.SetTextId("textDoista","id","com.infobip.unity.demo");
            builder.SetImageId("image", "id","com.infobip.unity.demo");
            builder.SetImageDrawableId("ic_launcher","drawable","com.infobip.unity.demo");
            builder.SetTitleId("title", "id","com.infobip.unity.demo");
            builder.SetDateId("date", "id", "com.infobip.unity.demo");
            builder.SetIconDrawableId("ic_launcher","drawable","com.infobip.unity.demo");
            builder.SetSoundResourceId("push_sound","raw","com.infobip.unity.demo");
            builder.VibrationPattern = new int[2] {1000, 100};
            builder.LightsColor = Color.red;
            builder.Lights = 1;
            builder.Vibrate = 1;
            builder.Sound = 1;
           // ScreenPrinter.Print(builder.ToString());
            InfobipPush.SetBuilderData(builder);
        }

        // Fourteenth row
        if (GUI.Button(new Rect(buttonSpace, rowY [14], buttonWidth, buttonHeight), "Get Builder Data"))
        {
            InfobipPushBuilder builder =  InfobipPush.GetBuilderData();
            ScreenPrinter.Print("BuilderData is: " + builder.ToString());
        }
        if (GUI.Button(new Rect(centerX - buttonWidth / 2.0f, rowY [14], buttonWidth, buttonHeight), "Remove Builder data"))
        {
            InfobipPush.RemoveBuilderSavedData();
            ScreenPrinter.Print("Remove builder saved Data");
        }
        if (GUI.Button(new Rect(centerX + buttonWidth / 2.0f + buttonSpace, rowY [14], buttonWidth, buttonHeight), "Set Quiet Time Enabled"))
        {
            InfobipPush.SetBuilderQuietTimeEnabled(true);
            ScreenPrinter.Print("Set Builder Quiet Time Enabled");
        }

		// Fiftheenth row
		if (GUI.Button(new Rect(buttonSpace, rowY [15], buttonWidth, buttonHeight), "Toggle rotation"))
		{
			if (Screen.orientation == ScreenOrientation.Portrait)
			{
				Screen.orientation = ScreenOrientation.Landscape;
			}
			else
			{
				Screen.orientation = ScreenOrientation.Portrait;
			}
		}
		#endif

	}
}
