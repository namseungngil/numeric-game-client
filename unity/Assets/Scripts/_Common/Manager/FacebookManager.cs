using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;

public class FacebookManager : MonoBehaviour
{
	// const
	public const string ME_SCORE_QUERY = "/me/scores";
	protected const string SCORES_QUERY = "/app/scores?fields=score,user.limit(20)";
	protected const string FRIENDS_QUERY = "/me?fields=friends.fields(first_name,last_name,id,picture.width(128).height(128))";
	protected const int TEXTURE_SIZE = 128;
	protected const string TEXTURE = "Texture";
	protected const string LABEL1 = "Label1";
	protected const string LABEL2 = "Label2";
	
//	protected const string FRIENDS_QUERY_STRING = "/v2.0/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture.width(128).height(128)),invitable_friends.limit(100).fields(first_name,id,picture.width(128).height(128))";

	// component
	protected LoveComponent loveComponent;
	protected UIManager uIManager;
	protected Texture userTexture;
	// array
//	protected List<object> friends = null;
	// variable
	protected int count;
	 
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

	private void AppRequestCallback (FBResult result)
	{
		Debug.Log ("AppRequestCallback");
		
		if (result != null) {
			Dictionary<string, object> responseObject = Json.Deserialize (result.Text) as Dictionary<string, object>;
			object obj = 0;
			if (responseObject.TryGetValue ("cancelled", out obj)) {
				Debug.Log ("AppRequestCallback cancelled");
			} else if (responseObject.TryGetValue ("request", out obj)) {
				Debug.Log ("AppRequestCallback send");
				
				if (loveComponent != null) {
					loveComponent.Add (count);
				}
				
				if (uIManager != null) {
					uIManager.Cancel ();
				}
			}
		}
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

	protected List<object> DeserializeScores(string response) 
	{
		
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object scoresh;
		var scores = new List<object>();
		if (responseObject.TryGetValue ("data", out scoresh)) 
		{
			scores = (List<object>) scoresh;
		}
		
		return scores;
	}

	protected int getScoreFromEntry(object obj)
	{
		Dictionary<string,object> entry = (Dictionary<string,object>) obj;
		return Convert.ToInt32(entry["score"]);
	}

	protected GameObject GetChildObject (GameObject gO, string strName)
	{ 
		Transform[] AllData = gO.GetComponentsInChildren<Transform> (); 
		GameObject target = null;
		
		foreach (Transform Obj in AllData) { 
			if (Obj.name == strName) { 
				target = Obj.gameObject;
				break;
			} 
		}
		
		return target;
	}

	public void onChallengeClicked (string[] list)
	{
		if (list.Length <= 0) {
			return;
		}
		
		if (FB.IsLoggedIn) {
			count = list.Length;
			
			FB.AppRequest (
				message: "Let's go together",
				to: list,
				filters: null,
				excludeIds: null,
				maxRecipients: null,
				data: Config.GAME_TITME,
				title: Config.GAME_TITME,
				callback: AppRequestCallback);
		}
	}
	
	public string userID ()
	{
		if (FB.IsLoggedIn) {
			return FB.UserId;
		}
		
		return null;
	}
}
