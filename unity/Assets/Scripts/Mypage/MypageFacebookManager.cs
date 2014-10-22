using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class MypageFacebookManager : FacebookManager
{
	NumericDebug numericDebug;
	
	new void Start ()
	{
		base.Start ();
		
		numericDebug = GameObject.Find ("Debug").GetComponent<NumericDebug> ();
	}
	
	private void AppRequestCallback (FBResult result) {
		Debug.Log ("AppRequestCallback");
		
		if (result != null) {
			Dictionary <string, object> responseObject = Json.Deserialize (result.Text) as Dictionary<string, object>;
			object obj = 0;
			if (responseObject.TryGetValue ("cancelled", out obj)) {
				Debug.Log ("AppRequestCallback cancelled");
			} else if (responseObject.TryGetValue ("request", out obj)) {
				Debug.Log ("AppRequestCallback send");
			}
		}
	}

	private void FriendsCallback (FBResult result)
	{
		Debug.Log ("FriendsCallback");

		if (result.Error != null) {
			Debug.Log ("FriendsCallback error : " + result.Error);
			
			// Let's just try again
			string temp = FRIENDS_QUERY_STRING;
			FB.API (temp, Facebook.HttpMethod.GET, FriendsCallback);
			return;
		}
		
		friends = DeserializeJSONFriends (result.Text);
		Debug.Log ("friend count : " + friends.Count);

//		var fd = ((Dictionary<string, object>)(friends[UnityEngine.Random.Range(0, friends.Count)]));
//		var friend = new Dictionary<string, string>();
//		friend["id"] = (string)fd["id"];
//		friend["first_name"] = (string)fd["first_name"];
//		var pictureDict = ((Dictionary<string, object>)(fd["picture"]));
//		var pictureDataDict = ((Dictionary<string, object>)(pictureDict["data"]));
//		friend["image_url"] = (string)pictureDataDict["url"];
//		return friend;

		string tempFriends = "";
		if (friends.Count > 0) {
			foreach (Dictionary<string, object> friend in friends) {
				tempFriends += ("[" + (string)friend["first_name"] + " - " + (string)friend["last_name"] + "]");
			}
		}

		numericDebug.Set ("friends : " + tempFriends);
	}
	
	public void onChallengeClicked ()
	{
		if (FB.IsLoggedIn && friends != null && friends.Count > 0) {
			string[] temp = new string[friends.Count];
			int index = 0;
			foreach (Dictionary<string, object> friend in friends) {
				temp[index] = (string)friend["id"];
				index++;
			}

			FB.AppRequest (
				message: "Let's go together",
				to: temp,
				filters: null,
				excludeIds: null,
				maxRecipients: null,
				data: "Numeric",
				title: "Numeric",
				callback: AppRequestCallback);
		}
	}

	public void Friends ()
	{
		string tempFriends = FRIENDS_QUERY_STRING;
		Debug.Log (tempFriends);

		if (friends != null) {
			return;
		}
		
		if (FB.IsLoggedIn) {
			FB.API (tempFriends, Facebook.HttpMethod.GET, FriendsCallback);
		}
	}
}
