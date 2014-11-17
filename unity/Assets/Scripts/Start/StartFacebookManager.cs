using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartFacebookManager : FacebookManager
{
	protected override void Start ()
	{
		if (FB.IsLoggedIn) {
			QueryScores ();
		}
	}

	private void QueryScores ()
	{
		FB.API (SCORES_QUERY, Facebook.HttpMethod.GET, ScoresCallback);
	}

	private void ScoresCallback (FBResult result)
	{
		Debug.Log ("ScoresCallback");
		if (result.Error != null) {
			Debug.LogError (result.Error);
			return;
		}

//		List<object> scores = new List<object> ();
		List<object> scoresList = DeserializeScores (result.Text);

		foreach (object score in scoresList) {
			var entry = (Dictionary<string, object>)score;
			var user = (Dictionary<string, object>)entry["user"];

			string userID = (string)user["id"];

			// me
			if (string.Equals (userID, FB.UserId)) {
				// this entry is the current player
				int playerHighScore = getScoreFromEntry (entry);
				Debug.Log ("Local players score on server is : " + playerHighScore);
			}

			if (userID != FB.UserId) {
				LoadPictureAPI (GetPictureURL (userID, TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture => {
					if (pictureTexture != null) {

					}
				});
			} else {

			}
		}
	}
}
