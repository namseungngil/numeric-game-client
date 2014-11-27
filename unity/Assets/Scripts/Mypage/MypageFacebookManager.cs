using UnityEngine;
using System.Collections;

public class MypageFacebookManager : FacebookManager
{
	protected override void Start ()
	{
		if (FB.IsLoggedIn) {
			if (userTexture == null) {
				MyPicktureCallback (userTexture);
			}

			if (userFristName == null || userLastName == null) {
				FB.API(ME_QUERY, Facebook.HttpMethod.GET, APICallback);
			}
		}
	}

	void APICallback(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log (result.Error);
			// Let's just try again
			FB.API(ME_QUERY, Facebook.HttpMethod.GET, APICallback);
			return;
		}
//		
		var profile = DeserializeJSONProfile(result.Text);
		userFristName = profile ["first_name"];
		userLastName = profile ["last_name"];
	}
}
