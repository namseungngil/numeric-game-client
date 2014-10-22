using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfobipPushMediaViewCustomization
{
    public InfobipPushMediaViewCustomization()
    {
        ForegroundColor = BackgroundColor = null;
        Width = Height = 320;
        X = Y = 0;
    }

    public int X
    {
        get;
        set;
    }

    public int Y
    {
        get;
        set;
    }
    
    public int Width
    {
        get;
        set;
    }
    
    public int Height
    {
        get;
        set;
    }
    
    public bool Shadow
    {
        get;
        set;
    }

    public int Radius
    {
        get;
        set;
    }

    public int DismissButtonSize
    {
        get;
        set;    
    }

    public Color? ForegroundColor
    {
        get;
        set;
    }

    public Color? BackgroundColor
    {
        get;
        set;    
    }

    private int ConvertToHex(Color? clr)
    {
        int hex = 0x0;
        hex |= (byte)(Math.Round(255 * clr.Value.r)) << 4 * 4 | (byte)(Math.Round(255 * clr.Value.g)) << 4 * 2 | (byte)(Math.Round(255 * clr.Value.b));
        return hex;
    }
    
    public override string ToString()
    {
        IDictionary<string, object> customiz = new Dictionary<string, object>(9);
        customiz ["x"] = X;
        customiz ["y"] = Y; 
        customiz ["width"] = Width;
        customiz ["height"] = Height; 
        customiz ["shadow"] = Shadow;
        customiz ["radius"] = Radius;
        customiz ["dismissButtonSize"] = DismissButtonSize;
        if (ForegroundColor == null)
        {
            customiz ["forgroundColorHex"] = null;
        } else
        {
            customiz ["forgroundColorHex"] = ConvertToHex(ForegroundColor);
        }
        if (BackgroundColor == null)
        {
            customiz ["backgroundColorHex"] = null;
        } else
        {
            customiz ["backgroundColorHex"] = ConvertToHex(BackgroundColor);
        }
        return MiniJSON.Json.Serialize(customiz);
    }
}
