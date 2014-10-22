using System.Collections.Generic;
using UnityEngine;
using System;

public class InfobipPushNotification
{
    public string OriginalNotification
    {
        get;
        set;
    }
    public string NotificationId
    { 
        get; 
        set; 
    }
    
    // iOS: path; Android: boolean string ("true" or "false")
    public string Sound
    {
        get;
        set;
    }
    
    public string Url
    {
        get;
        set;
    }
    
    // Android only
    public bool Lights
    {
        get;
        set;
    }
    
    // Android only
    public bool Vibrate
    {
        get;
        set;
    }
    
    public object AdditionalInfo
    {
        get;
        set;
    }
    
    public string MediaData
    {
        get;
        set;
    }
    
    public bool isMediaNotification()
    {
        if (String.IsNullOrEmpty(MediaData))
        {
            return false;
        } 
        return true;
    }
    
    public string Title
    {
        get;
        set;
    }
    
    public string Message
    {
        get;
        set;
    }
    
    public string MimeType
    {
        get;
        set;
    }
    
    // iOS only
    public int? Badge
    {
        get;
        set;
    }
    
    public override string ToString()
    {
        IDictionary<string, object> notif = new Dictionary<string, object>(9);
        notif ["notificationId"] = NotificationId;
        notif ["sound"] = Sound; 
        notif ["url"] = Url;
        notif ["additionalInfo"] = AdditionalInfo;
        notif ["mediaData"] = MediaData;
        notif ["title"] = Title;
        notif ["message"] = Message; 
        notif ["mimeType"] = MimeType;
        notif ["badge"] = Badge;
        return MiniJSON.Json.Serialize(notif);
    }
    
    public InfobipPushNotification(string notif)
    {
        Badge = null;
        IDictionary<string, object> dictNotif = MiniJSON.Json.Deserialize(notif) as Dictionary<string,object>;
        object varObj = null;
        int varInt;
        OriginalNotification = notif;
        if (dictNotif.TryGetValue("id", out varObj))
        {
            NotificationId = (string)varObj;
        }
        if (dictNotif.TryGetValue("notificationId", out varObj))
        {
            NotificationId = (string)varObj;
        }
        if (dictNotif.TryGetValue("title", out varObj))
        {
            Title = (string)varObj;
        }
        //IDictionary<string, int> dictNotifInt = dictNotif as Dictionary<string, int>;
        if (dictNotif.TryGetValue("badge", out varObj))
        {
            if (varObj == null || "".Equals(varObj as string))
            {
                Badge = null;
            } else
            {
                varInt = Convert.ToInt32(varObj);
                Badge = varInt;
            }
        }
        if (dictNotif.TryGetValue("sound", out varObj))
        {
            Sound = (string)varObj;
            #if UNITY_ANDROID
            
            #endif
        }
        if (dictNotif.TryGetValue("mimeType", out varObj))
        {
            MimeType = (string)varObj;
        }
        if (dictNotif.TryGetValue("url", out varObj))
        {
            Url = (string)varObj;
        }
        if (dictNotif.TryGetValue("additionalInfo", out varObj))
        {
            // string additionalInfo = MiniJSON.Json.Serialize(varObj);
            AdditionalInfo = varObj;
        }
        if (dictNotif.TryGetValue("mediaData", out varObj))
        {
            MediaData = (string)varObj;
        }
        if (dictNotif.TryGetValue("message", out varObj))
        {
            Message = (string)varObj;
        }
        if (dictNotif.TryGetValue("lights", out varObj))
        {
            Lights = (bool)varObj;
        }
        if (dictNotif.TryGetValue("vibrate", out varObj))
        {
            Vibrate = (bool)varObj;
        }
    }
    
    public InfobipPushNotification()
    {
    }
}
