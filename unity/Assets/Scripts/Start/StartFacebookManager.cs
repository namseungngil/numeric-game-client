using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StartFacebookManager : FacebookManager
{
	// gameobject
	private GameObject ranks;
	// compoent
	private HttpComponent httpComponent;
	// array
	private static List<object> friends = null;
	private List<string> rankFriendsList;

	protected override void Start ()
	{
		ranks = gameObject.GetComponentInChildren<UIWidget> ().gameObject;
		httpComponent = gameObject.GetComponentInParent<HttpComponent> ();

		if (FB.IsLoggedIn) {
			UITexture uITexture = GetChildObject (ranks, TEXTURE).GetComponent<UITexture> ();
			UILabel uILabel1 = GetChildObject (ranks, LABEL1).GetComponent<UILabel> ();
			UILabel uILabel2 = GetChildObject (ranks, LABEL2).GetComponent<UILabel> ();
			if (userTexture != null) {
				uITexture.mainTexture = userTexture;
			}
			uILabel1.text = QUESTION_MARK;
			uILabel2.text = QUESTION_MARK;
			QueryScores ();
		}

		GameObject temp = GameObject.Find (Config.ROOT_MANAGER);
		if (temp != null) {
			loveComponent = temp.GetComponent<LoveComponent> ();
		}
	}

	private void QueryScores ()
	{
		string tempFriends = FRIENDS_QUERY;
		Debug.Log (tempFriends);

		if (rankFriendsList != null && rankFriendsList.Count > 0) {
			Rank ();
		} else {
			FB.API (tempFriends, Facebook.HttpMethod.GET, FriendsCallback);
		}
	}

	private void FriendsCallback (FBResult result)
	{
		Debug.Log ("FriendsCallback");
		
		if (result.Error != null) {
			Debug.Log ("FriendsCallback error : " + result.Error);
			
			// Let's just try again
			string temp = FRIENDS_QUERY;
			FB.API (temp, Facebook.HttpMethod.GET, FriendsCallback);
			return;
		}
		
		friends = DeserializeJSONFriends (result.Text);
		rankFriendsList = new List<string> ();
		rankFriendsList.Add (FB.UserId);
		Debug.Log ("StarFacebookManager friendsCallback count : " + friends.Count);

		if (friends.Count > 0) {
			foreach (Dictionary<string, object> friend in friends) {
				rankFriendsList.Add (friend ["id"].ToString ());
			}
		}
		Rank ();
	}

	private void Rank ()
	{
		if (rankFriendsList == null) {
			return;
		}

		httpComponent.OnDone = (object obj) => {
			Dictionary<string, int> dic;
			if ((dic = obj as Dictionary<string, int>) != null) {
				var items = from pair in dic
					orderby pair.Value ascending
						select pair;

				foreach (KeyValuePair<string, int> kVP in items) {

				}
			}
		};
		
		httpComponent.StartGame (rankFriendsList, SenceData.stageLevel);
	}
	
	private void ScoresCallback (FBResult result)
	{
		Debug.Log ("ScoresCallback");
		if (result.Error != null) {
			Debug.LogError (result.Error);
			return;
		}
		
		//		List<object> scores = new List<object> ();
		Debug.Log (result.Text);
		List<object> scoresList = DeserializeScores (result.Text);
		if (scoresList.Count > 0) {
			Debug.Log ("scoreList : " + scoresList.Count);
			int rank = 0;
			foreach (object score in scoresList) {
				Dictionary<string, object> entry = (Dictionary<string, object>)score;
				string tempScore = "" + entry ["score"];
				if (int.Parse (tempScore) == 0) {
					continue;
				}
				Dictionary<string, object> user = (Dictionary<string, object>)entry ["user"];

				string name = (string)user ["name"];
				string userID = (string)user ["id"];

				GameObject gObj = Instantiate (ranks, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				
				gObj.transform.parent = this.transform;
				gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
				gObj.name = userID;

				UITexture uITexture = GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
				UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
				UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();
				UIButton uIButton = GetChildObject (gObj, BUTTON).GetComponent<UIButton> ();

				rank ++;
				uILabel1.text = rank.ToString ();
				uILabel2.text = name + "\n" + tempScore;
				uIButton.name = userID;

				// me
				if (string.Equals (userID, FB.UserId)) {
					// this entry is the current player
					uIButton.gameObject.SetActive (false);
				}

				LoadPictureAPI (GetPictureURL (userID, TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture => {
					if (pictureTexture != null) {
						uITexture.mainTexture = pictureTexture;
					}
				});
			}

			scoresList.Sort (delegate(object firstObj,
			                     object secondObj) {
				return -getScoreFromEntry (firstObj).CompareTo (getScoreFromEntry (secondObj));
			});

			ranks.SetActive (false);
			gameObject.GetComponent<UIGrid> ().Reposition ();
		} else {
			// loading...
			ranks.SetActive (false);
		}
	}

	private void Test ()
	{
		int rank = 0;
		for (int i = 0; i < 20; i++) {
			GameObject gObj = Instantiate (ranks, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			gObj.transform.parent = this.transform;
			gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
			
			string name = i.ToString ();
			string tempScore = i.ToString ();
			
			UITexture uITexture = GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
			UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
			UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();
			UIButton uIButton = GetChildObject (gObj, "Button").GetComponent<UIButton> ();
			
			rank ++;
			uILabel1.text = rank.ToString ();
			uILabel2.text = name + "\n" + tempScore;
			uIButton.name = i.ToString ();
		}
		
		ranks.SetActive (false);
		gameObject.GetComponent<UIGrid> ().Reposition ();
	}
}
