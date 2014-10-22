using UnityEngine;

public class Device
{
	public static string Language ()
	{
		Debug.Log (Application.systemLanguage);
		return Application.systemLanguage.ToString ();
	}
}
