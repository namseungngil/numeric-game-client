using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public delegate void InfobipPushDelegateWithNotificationArg(InfobipPushNotification notification);

public delegate void InfobipPushDelegateWithStringArg(string argument);

public delegate void InfobipPushDelegate();

#pragma warning disable 0162

public class InfobipPush : MonoBehaviour
{
    #region declaration of methods
    [DllImport ("__Internal")]
    private static extern void IBSetLogModeEnabled(bool isEnabled, int lLevel = 0);

    [DllImport ("__Internal")]
    private static extern bool IBIsLogModeEnabled();

    [DllImport ("__Internal")]
    private static extern void IBSetTimezoneOffsetInMinutes(int offsetMinutes);

    [DllImport ("__Internal")]
    private static extern void IBSetTimezoneOffsetAutomaticUpdateEnabled(bool isEnabled);
    
    [DllImport ("__Internal")]
    private static extern void IBInitialization(string appId, string appSecret);

    [DllImport ("__Internal")]
    private static extern void IBInitializationWithRegistrationData(string appId, string appSecret, string registrationData);

    [DllImport ("__Internal")]
    private static extern bool IBIsRegistered();

    [DllImport ("__Internal")]
    private static extern string IBDeviceId();

    [DllImport ("__Internal")]
    private static extern void IBSetUserId(string userId);
    
    [DllImport ("__Internal")]
    private static extern string IBUserId();

    [DllImport ("__Internal")]
    private static extern void IBUnregister();
    
    [DllImport ("__Internal")]
    private static extern void IBRegisterToChannels(string channelsData);
    
    [DllImport ("__Internal")]
    private static extern void IBGetRegisteredChannels();

    [DllImport ("__Internal")]
    private static extern void IBGetUnreceivedNotifications();

    [DllImport ("__Internal")]
    private static extern void IBSetBadgeNumber(int badgeNo);

    [DllImport ("__Internal")]
    private static extern void IBAddMediaView(string notif, string customiz);

    #endregion

    #region listeners
    public static InfobipPushDelegateWithNotificationArg OnNotificationReceived = delegate
    {
    };
    public static InfobipPushDelegateWithNotificationArg OnUnreceivedNotificationReceived = delegate
    {
    };
    public static InfobipPushDelegateWithNotificationArg OnNotificationOpened = delegate
    {
    };
    public static InfobipPushDelegate OnRegistered = delegate
    {
    };
    public static InfobipPushDelegate OnRegisteredToChannels = delegate
    {
    };
    public static InfobipPushDelegate OnUnregistered = delegate
    {
    };
    public static InfobipPushDelegate OnUserDataSaved = delegate
    {
    };
    public static InfobipPushDelegate OnLocationShared = delegate
    {
    };
    public static InfobipPushDelegate OnNotifyNotificationOpenedFinished = delegate
    {
    };
    public static InfobipPushDelegateWithStringArg OnGetChannelsFinished = delegate
    {
    };
    public static InfobipPushDelegateWithStringArg OnError = delegate
    {
    };
    #endregion

    private static InfobipPush _instance;
    private static readonly object synLock = new object();
    private const string SINGLETON_GAME_OBJECT_NAME = "InfobipPush Instance";

    public static InfobipPush GetInstance()
    {
        lock (synLock)
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(InfobipPush)) as InfobipPush;
                if (_instance == null)
                {
                    var gameObject = new GameObject(SINGLETON_GAME_OBJECT_NAME);
                    _instance = gameObject.AddComponent<InfobipPush>();
                }
            }
            return _instance;
        }
    }

    public static bool LogMode
    {
        get
        {
            #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return IBIsLogModeEnabled();
            }
            #elif UNITY_ANDROID
            return InfobipPushInternal.Instance.GetLogModeEnabled();
            #endif
            return false;
        }
        set
        {
            #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                IBSetLogModeEnabled(value);
            }
            #elif UNITY_ANDROID
                InfobipPushInternal.Instance.SetLogModeEnabled(value);
            #endif
        }
    }

    static IEnumerator SetLogModeEnabled_C(bool isEnabled, int logLevel)
    {
        IBSetLogModeEnabled(isEnabled, logLevel);
        yield return true;
    }

    public static void SetLogModeEnabled(bool isEnabled, int logLevel)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GetInstance().StartCoroutine(SetLogModeEnabled_C(isEnabled, logLevel));
        }
        #elif UNITY_ANDROID
            InfobipPushInternal.Instance.SetLogModeEnabled(isEnabled);
        #endif
    }
    public static bool IsLogModeEnabled()
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return IBIsLogModeEnabled();
        }
        #endif
        return false;
    }
    static IEnumerator SetTimezoneOffsetInMinutes_C(int offsetMinutes)
    {
        IBSetTimezoneOffsetInMinutes(offsetMinutes);
        yield return true;
    }

    public static void SetTimezoneOffsetInMinutes(int offsetMinutes)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GetInstance().StartCoroutine(SetTimezoneOffsetInMinutes_C(offsetMinutes));
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.SetTimeZoneOffset(offsetMinutes);
        #endif
    }

    public static void SetTimezoneOffsetAutomaticUpdateEnabled(bool isEnabled)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBSetTimezoneOffsetAutomaticUpdateEnabled(isEnabled);
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.SetTimeZoneAutomaticUpdateEnabled(isEnabled);
        #endif
    }

    static IEnumerator Register_C(string applicationId, string applicationSecret, InfobipPushRegistrationData registrationData = null)
    {
        InfobipPushInternal.GetInstance();
        if (registrationData == null)
        {
            IBInitialization(applicationId, applicationSecret);
        } else
        {
            var regdata = registrationData.ToString();
            IBInitializationWithRegistrationData(applicationId, applicationSecret, regdata);
        }
        yield return true;
    }

    public static void Register(string applicationId, string applicationSecret, InfobipPushRegistrationData registrationData = null)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GetInstance().StartCoroutine(Register_C(applicationId, applicationSecret, registrationData));
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.Register(applicationId, applicationSecret, registrationData);
        #endif
    }

    public static void Initialize()
    {
        InfobipPushInternal.GetInstance();
        InfobipPushLocation.GetInstance(); 
        InfobipPush.GetInstance();
    }

    public static bool IsRegistered()
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
           return IBIsRegistered();
        }
        #elif UNITY_ANDROID
        return InfobipPushInternal.Instance.IsRegistered();
        #endif
        return false;
    }

    public static string DeviceId
    {
        get
        {
            #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return IBDeviceId();
            }
            #elif UNITY_ANDROID
            return InfobipPushInternal.Instance.GetDeviceId();
            #endif
            return null;
        }
           
    }

    public static void SetBuilderData(InfobipPushBuilder builder)
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.SetBuilderData(builder.ToString());
        #endif
    }

    public static InfobipPushBuilder GetBuilderData()
    {
        #if UNITY_ANDROID
        return InfobipPushInternal.Instance.GetBuilderData();
        #endif
        return null;
    }

    public static void RemoveBuilderSavedData()
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.RemoveBuilderSavedData();
        #endif
    }

    public static void SetBuilderQuietTimeEnabled(bool isEnabled)
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.SetBuilderQuietTimeEnabled(isEnabled);
        #endif
    }

    public static bool IsBuilderInQuietTime() {
        #if UNITY_ANDROID
        return InfobipPushInternal.Instance.IsBuilderInQuietTime();
        #endif
        return false;
    }


    static IEnumerator BeginSetUserId_C(string value)
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.BeginSetUserId(value);
        #endif
        yield return true;
    }

    public static string UserId
    {
        get
        {
            #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return IBUserId();
            }
            #elif UNITY_ANDROID
            return InfobipPushInternal.Instance.GetUserId();
            #endif
            return null;
        }
        set
        {
            #if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                InfobipPushInternal.GetInstance();
                IBSetUserId(value);
            }
            #elif UNITY_ANDROID
            GetInstance().StartCoroutine(BeginSetUserId_C(value));
            #endif
        }
    }

    static IEnumerator Unregister_C()
    {
        IBUnregister();
        yield return true;
    }

    public static void Unregister()
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GetInstance().StartCoroutine(Unregister_C());
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.Unregister();
        #endif
    }

    public static void SetBadgeNumber(int badgeNo)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBSetBadgeNumber(badgeNo);
        }
        #endif
    }

    public static void AddMediaView(InfobipPushNotification notif, InfobipPushMediaViewCustomization customiz = null)
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBAddMediaView(notif.ToString(), customiz.ToString());
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.AddMediaView(notif);
        #endif
    }

    static IEnumerator RegisterToChannels_C(string[] channels, bool remove)
    {   
        IDictionary<string, object> dict = new Dictionary<string, object>(2);
        dict ["channels"] = channels;
        dict ["removeExistingChannels"] = remove;
        string channelsData = MiniJSON.Json.Serialize(dict);

        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBRegisterToChannels(channelsData);
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.RegisterToChannels(channelsData);
        #endif

        yield return true;
    }

    public static void RegisterToChannels(string[] channels, bool removeExistingChannels = false)
    {
        GetInstance().StartCoroutine(RegisterToChannels_C(channels, removeExistingChannels));
    }

    static IEnumerator BeginGetRegisteredChannels_C()
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBGetRegisteredChannels();
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.BeginGetRegisteredChannels();
        #endif

        yield return true;
    }

    public static void BeginGetRegisteredChannels()
    {
        GetInstance().StartCoroutine(BeginGetRegisteredChannels_C());
    }

    static IEnumerator GetListOfUnreceivedNotifications_C()
    {
        #if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IBGetUnreceivedNotifications(); 
        }
        #elif UNITY_ANDROID
        InfobipPushInternal.Instance.BeginGetUnreceivedNotifications();
        #endif
        yield return true;
    }

    public static void GetListOfUnreceivedNotifications()
    {
        GetInstance().StartCoroutine(GetListOfUnreceivedNotifications_C());  
    }


    static IEnumerator NotifyNotificationOpened_C(string id)
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.BeginNotifyNotificationOpened(id);
        #endif
        yield return true;
    }

    public static void NotifyNotificationOpened(string notificationId)
    {
        GetInstance().StartCoroutine(NotifyNotificationOpened_C(notificationId));
    }


    public static void SetOverrideDefaultMessageHandling(bool enable)
    {
        #if UNITY_ANDROID
        InfobipPushInternal.Instance.OverrideDefaultMessageHandling(enable);
        #endif
    }

    public static bool IsDefaultMessageHandlingOverriden()
    {
        #if UNITY_ANDROID
        return InfobipPushInternal.Instance.IsDefaultMessageHandlingOverriden();
        #endif
        return false;
    }

}
#pragma warning restore 0162
