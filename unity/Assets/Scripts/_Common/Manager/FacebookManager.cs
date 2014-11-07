using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;

public class FacebookManager : MonoBehaviour
{
	protected const string FRIENDS_QUERY_STRING = "/me?fields=friends.fields(first_name,last_name,id,picture.width(128).height(128))";
//	protected const string FRIENDS_QUERY_STRING = "/v2.0/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture.width(128).height(128)),invitable_friends.limit(100).fields(first_name,id,picture.width(128).height(128))";
	protected Texture userTexture;
	protected List<object> friends = null;

	private void OnInitComplete ()
	{
		Debug.Log ("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}
	
	private void OnHideUnity (bool isGameShown)
	{
		Debug.Log ("Is game showing? " + isGameShown);
	}
	
	private void Callback (FBResult result)
	{
		if (!String.IsNullOrEmpty (result.Error)) {
			// "Error Response:\n" + result.Error;
		} else if (!String.IsNullOrEmpty (result.Text)) {
			// "Success Response:\n" + result.Text;
		} else if (result.Texture != null) {
			// "Success Response: texture\n";
			//			lastResponseTexture = result.Texture;
		} else {
			// "Empty Response\n";
		}
	}
	
	private IEnumerator LoadPictureEnumerator (string url, LoadPictureCallback callback)
	{
		WWW www = new WWW (url);
		yield return www;
		callback (www.texture);
	}

	private string DeserializePictureURLString (string response)
	{
		return DeserializePictureURLObject (Json.Deserialize (response));
	}
	
	private static string DeserializePictureURLObject (object pictureObj)
	{
		var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj) ["data"]);
		object urlH = null;
		if (picture.TryGetValue ("url", out urlH)) {
			return (string)urlH;
		}
		
		return null;
	}

	protected virtual void Start ()
	{
		CallFBInit ();
	}

	protected void CallFBInit ()
	{
		FB.Init (OnInitComplete, OnHideUnity);
	}
	
	protected void CallFbPostToGamerGroup ()
	{
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		dict ["message"] = "facebook API test.";
		
		FB.API ("" + "/feed", Facebook.HttpMethod.POST, Callback, dict);
	}

	protected delegate void LoadPictureCallback (Texture texture);

	protected void LoadPictureAPI (string url, LoadPictureCallback callback)
	{
		FB.API (url, Facebook.HttpMethod.GET, result => {
			if (result.Error != null) {
				Debug.Log ("LoadPictureAPK Load error : " + result.Error);
				return;
			}
			
			var imageUrl = DeserializePictureURLString (result.Text);
			StartCoroutine (LoadPictureEnumerator (imageUrl, callback));
		});
	}

	protected static string GetPictureURL (string facebookID, int? width = null, int? height = null, string type = null)
	{
		string url = string.Format ("/{0}/picture", facebookID);
		string query = width != null ? "&width=" + width.ToString () : "";
		query += height != null ? "&height=" + height.ToString () : "";
		query += type != null ? "&type=" + type : "";
		query += "&redirect=false";
		if (query != "")
			url += ("?g" + query);
		return url;
	}
	
	protected void MyPicktureCallback (Texture texture)
	{
		Debug.Log ("MyPicktureCallback");
		
		if (texture == null) {
			// Let's just try again
			LoadPictureAPI (GetPictureURL ("me", 128, 128), MyPicktureCallback);
			return;
		}
		
		userTexture = texture;
	}

	protected Dictionary<string, string> RandomFriend(List<object> friends)
	{
		var fd = ((Dictionary<string, object>)(friends[UnityEngine.Random.Range(0, friends.Count)]));
		var friend = new Dictionary<string, string>();
		friend["id"] = (string)fd["id"];
		friend["first_name"] = (string)fd["first_name"];
		var pictureDict = ((Dictionary<string, object>)(fd["picture"]));
		var pictureDataDict = ((Dictionary<string, object>)(pictureDict["data"]));
		friend["image_url"] = (string)pictureDataDict["url"];
		return friend;
	}

	protected List<object> DeserializeJSONFriends (string response)
	{
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object friendsH;
		var friends = new List<object>();
		if (responseObject.TryGetValue("invitable_friends", out friendsH))
		{
			friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
		}
		if (responseObject.TryGetValue("friends", out friendsH))
		{
			friends.AddRange((List<object>)(((Dictionary<string, object>)friendsH)["data"]));
		}
		return friends;
	}


	public string userID ()
	{
		if (FB.IsLoggedIn) {
			return FB.UserId;
		}
		
		return null;
	}
}
