using UnityEngine;
using System.Collections;

public class MypageFacebookManager : FacebookManager
{
	protected override void Start ()
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("Me Data");
			if (userTexture == null) {
				MyPicktureCallback (userTexture);
			}
		}
	}

	public void SetMeFicture (UITexture u)
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("Me Data");
			if (userTexture == null) {
				LoadPictureAPI (GetPictureURL (FB.UserId, TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture =>
				{
					if (pictureTexture != null) {
						userTexture = pictureTexture;
						u.mainTexture = pictureTexture;
					}
				});
			} else {
				u.mainTexture = userTexture;
			}
		}
	}
}
