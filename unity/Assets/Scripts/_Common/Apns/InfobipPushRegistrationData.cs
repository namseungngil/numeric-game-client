using System.Collections;
using System.Collections.Generic;

public class InfobipPushRegistrationData
{
    public string UserId
    {
        get;
        set;
    }
    
    public string[] Channels
    {
        get;
        set;
    }
    
    public override string ToString()
    {
        IDictionary<string, object> regData = new Dictionary<string, object>(2);
        regData ["userId"] = UserId;
        regData ["channels"] = Channels;
        return MiniJSON.Json.Serialize(regData);
    }
}
