using System.Collections.Generic;
using UnityEngine;
using System;

public class InfobipPushBuilder
{
    public InfobipPushBuilder()
    {
    }

    private int ConvertToJavaHexColor(Color? clr)
    {
        int hex = 0x0;
        hex |= (byte)(Math.Round(255 * clr.Value.r)) << 4 * 4 | (byte)(Math.Round(255 * clr.Value.g)) << 4 * 2 | (byte)(Math.Round(255 * clr.Value.b));
        return hex;
    }

    public InfobipPushBuilder(string json)
    {
        this.setBuilderFromJson(json);
    }
   
    private string myTickerText = null;

    public string TickerText
    {
        get { return this.myTickerText; }
        set { this.myTickerText = value; }
    }
    
    private string myApplicationName = null;

    public string ApplicationName
    {
        get { return this.myApplicationName; }
        set { this.myApplicationName = value; }
    }
    
    private int mySound = -1;

    public int Sound
    {
        get { return this.mySound; }
        set { this.mySound = value; }
    }
    
    private int myVibrate = -1;

    public int Vibrate
    {
        get { return this.myVibrate; }
        set { this.myVibrate = value; }
    }
    
    private int myLights = -1;

    public int Lights
    {
        get { return this.myLights; }
        set { this.myLights = value; }
    }

    private int[] myVibrationPattern;

    public int[] VibrationPattern
    {
        get { return this.myVibrationPattern; }
        set { this.myVibrationPattern = value; }
    }
    
    private Color? myLightsColor;
    public Color? LightsColor
    {
        get { return this.myLightsColor; }
        set { this.myLightsColor = value; }
    }
    
    private int myLightsOnDurationInMs = -1;

    public int LightsOnDurationInMs
    {
        get { return this.myLightsOnDurationInMs; }
        private set { this.myLightsOnDurationInMs = value; }
    }
    
    private int myLightsOffDurationInMs = -1;

    public int LightsOffDurationInMs
    {
        get { return this.myLightsOffDurationInMs; }
        set { this.myLightsOffDurationInMs = value; }
    }

    private TimeSpan? myQuietTimeStart = null;

    public TimeSpan? QuietTimeStart
    {
        get { return this.myQuietTimeStart; }
        private set { this.myQuietTimeStart = value; }
    }

    private TimeSpan? myQuietTimeEnd = null;

    public TimeSpan? QuietTimeEnd
    {
        get { return this.myQuietTimeEnd; }
        private set { this.myQuietTimeEnd = value; }
    }

    private bool myQuietTimeEnabled;

    public bool QuietTimeEnabled
    {
        get { return this.myQuietTimeEnabled; }
        private set { this.myQuietTimeEnabled = value; }
    }

    public override string ToString()
    {
        IDictionary<string, int> lightsOnOff = new Dictionary<string, int>();
        if (LightsOnDurationInMs != -1)
            lightsOnOff.Add("on", LightsOnDurationInMs);
        if (LightsOffDurationInMs != -1)
            lightsOnOff.Add("off", LightsOffDurationInMs);
       
        IDictionary<string, int> quietTime = new Dictionary<string, int>();
        if (this.QuietTimeStart != null)
        {
            quietTime.Add("startHour", this.QuietTimeStart.Value.Hours);
            quietTime.Add("startMinute", this.QuietTimeStart.Value.Minutes);
        }
        if (this.QuietTimeEnd != null)
        {
            quietTime.Add("endHour", this.QuietTimeEnd.Value.Hours);
            quietTime.Add("endMinute", this.QuietTimeEnd.Value.Minutes);
        }

        
        IDictionary<string, object> builder = new Dictionary<string, object>(12);
        if(TickerText != null)      builder ["tickerText"] = TickerText;
        if(ApplicationName != null) builder ["applicationName"] = ApplicationName;
        if(Sound != -1)             builder ["sound"] = Sound;
        if(Vibrate != -1)           builder ["vibration"] = Vibrate;
        if(Lights != -1)            builder ["light"] = Lights;
        if(VibrationPattern != null)builder ["vibrationPattern"] = VibrationPattern;
        if(LightsColor != null)     builder ["lightsColor"] = this.ConvertToJavaHexColor(LightsColor);
        if(lightsOnOff.Count > 0)   builder ["lightsOnOffMS"] = lightsOnOff;
        if(quietTime.Count > 0)     builder ["quietTime"] = quietTime;
        if(layoutIdName != null)      builder ["layoutIdName"] = layoutIdName;
        if(fileLayoutName != null)      builder ["fileLayoutName"] = fileLayoutName;
        if(packageName != null)      builder ["packageName"] = packageName;
        if(textIdName != null)      builder ["textIdName"] = textIdName;
        if(fileTextName != null)      builder ["fileTextName"] = fileTextName;
        if(titleIdName != null)      builder ["titleIdName"] = titleIdName;
        if(fileTitleName != null)      builder ["fileTitleName"] = fileTitleName;
        if(imageIdName != null)      builder ["imageIdName"] = imageIdName;
        if(imageName != null)      builder ["imageName"] = imageName;
        if(fileImageName != null)      builder ["fileImageName"] = fileImageName;
        if(fileImageIdName != null)      builder ["fileImageIdName"] = fileImageIdName;
        if(iconIdName != null)      builder ["iconIdName"] = iconIdName;
        if(fileIconName != null)      builder ["fileIconName"] = fileIconName;
        if(soundIdName != null)      builder ["soundIdName"] = soundIdName;
        if(fileSoundName != null)      builder ["fileSoundName"] = fileSoundName;
        if(dateIdName != null)      builder ["dateIdName"] = dateIdName;
        if(fileDateName != null)      builder ["fileDateName"] = fileDateName;
        // TODO add Quiet time enabled (bool)

        return MiniJSON.Json.Serialize(builder);
    }

    public void SetQuietTime(TimeSpan start, TimeSpan end)
    {
        this.QuietTimeStart = start;
        this.QuietTimeEnd = end;
    }

    public void SetLightsOnOffDurationsMs(int on, int off)
    {
        this.LightsOnDurationInMs = on;
        this.LightsOffDurationInMs = off;
    }

    
    private string layoutIdName;
    private string fileLayoutName;
    private string packageName;
    public void SetLayoutId(string layoutIdName,string fileLayoutName,string packageName)
    {
        this.layoutIdName = layoutIdName;
        this.fileLayoutName = fileLayoutName;
        this.packageName = packageName;
    } 
    private string textIdName;
    private string fileTextName;
    public void SetTextId(string textIdName,string fileTextName,string packageName)
    {
        this.textIdName = textIdName;
        this.fileTextName = fileTextName;
        this.packageName = packageName;
    }
    private string dateIdName;
    private string fileDateName;
    public void SetDateId(string dateIdName,string fileDateName,string packageName)
    {
        this.dateIdName = dateIdName;
        this.fileDateName = fileDateName;
        this.packageName = packageName;
    }
    private string titleIdName;
    private string fileTitleName;
    public void SetTitleId(string titleIdName,string fileTitleName,string packageName)
    {
        this.titleIdName = titleIdName;
        this.fileTitleName = fileTitleName;
        this.packageName = packageName;
    }
    private string imageIdName;
    private string fileImageIdName;
    public void SetImageId(string imageIdName, string fileImageIdName, string packageName)
    {
        this.imageIdName = imageIdName;
        this.fileImageIdName = fileImageIdName;
        this.packageName = packageName;
    }
    private string imageName;
    private string fileImageName;
    public void SetImageDrawableId(string imageName, string fileImageName, string packageName)
    {
        this.imageName = imageName;
        this.fileImageName=fileImageName;
        this.packageName = packageName;
    }
    private string iconIdName;
    private string fileIconName;
    public void SetIconDrawableId(string iconIdName, string fileIconName, string packageName)
    {
        this.iconIdName = iconIdName;
        this.fileIconName=fileIconName;
        this.packageName = packageName;
    }
    private string soundIdName;
    private string fileSoundName;
    public void SetSoundResourceId(string soundIdName, string fileSoundName, string packageName)
    {
        this.soundIdName = soundIdName;
        this.fileSoundName=fileSoundName;
        this.packageName = packageName;
    }
    private void setBuilderFromJson(string json)
    {
        IDictionary<string, object> dictBuilder = MiniJSON.Json.Deserialize(json) as Dictionary<string,object>;
        object varObj = null;

        if (dictBuilder.TryGetValue("tickerText", out varObj))
        {
            this.TickerText = (string)varObj;
        }

        if (dictBuilder.TryGetValue("applicationName", out varObj))
        {
            this.ApplicationName = (string)varObj;
        }

        if (dictBuilder.TryGetValue("sound", out varObj))
        {
            this.Sound = Convert.ToInt32(varObj);
        }

        if (dictBuilder.TryGetValue("vibration", out varObj))
        {
            this.Vibrate = Convert.ToInt32(varObj);
        }

        if (dictBuilder.TryGetValue("light", out varObj))
        {
            this.Lights = Convert.ToInt32(varObj);
        }

        if (dictBuilder.TryGetValue("vibrationPattern", out varObj))
        {
            List<int> tempVibArray = new List<int>();
            List<object> patternList = (List<object>) varObj;
            foreach (var item in patternList) {
                tempVibArray.Add(Convert.ToInt32(item));
            }
            this.VibrationPattern = tempVibArray.ToArray();
        }

        if (dictBuilder.TryGetValue("lightsColor", out varObj))
        {
            int color = Convert.ToInt32(varObj);
            byte[] values = BitConverter.GetBytes(color);
            if(!BitConverter.IsLittleEndian) Array.Reverse(values);
            this.LightsColor = new Color(values[0], values[1], values[2]);
        }

        if (dictBuilder.TryGetValue("lightsOnOffMS", out varObj))
        {
            Dictionary<string, object> lights = (Dictionary<string, object>)varObj;
            int on, off;
            object onObj, offObj;
            if(lights.TryGetValue("on", out onObj)) 
            {
                on = Convert.ToInt32(onObj);
                if(lights.TryGetValue("off", out offObj)) 
                {
                    off = Convert.ToInt32(offObj);
                    this.SetLightsOnOffDurationsMs(on, off);
                }
            }
        }

        if (dictBuilder.TryGetValue("quietTime", out varObj))
        {
            Dictionary<string, object> quietTime = (Dictionary<string, object>)varObj;
            int sh, sm, eh, em;
            object shObj, smObj, ehObj, emObj;
            TimeSpan start = new TimeSpan(); 
            TimeSpan end = new TimeSpan();
            if(quietTime.TryGetValue("startHour", out shObj)) 
            {
                sh = Convert.ToInt32(shObj);
                if(quietTime.TryGetValue("startMinute", out smObj)) 
                {
                    sm = Convert.ToInt32(smObj);
                    start = new TimeSpan(sh, sm, 0);
                }
            }
            if(quietTime.TryGetValue("endHour", out ehObj)) 
            {
                eh = Convert.ToInt32(ehObj);
                if(quietTime.TryGetValue("endMinute", out emObj)) 
                {
                    em = Convert.ToInt32(emObj);
                    end = new TimeSpan(eh, em, 0);
                }
            }
            this.SetQuietTime(start, end);
        }

        if (dictBuilder.TryGetValue("quietTimeEnabled", out varObj))
        {
            this.QuietTimeEnabled = (bool)varObj;
        }
    }
}