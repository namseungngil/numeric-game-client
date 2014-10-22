using UnityEngine;

public class LoadingFacebookManager : FacebookManager
{
	new void Start ()
	{
		if (LoadingData.currentLevel == Config.LOGIN) {
			base.Start ();
		}
	}
}
