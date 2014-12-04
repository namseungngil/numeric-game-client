using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RankFacebookManager : FacebookManager
{
	// compoent
	private HttpComponent httpComponent;
	// array
	private List<GameObject> rank;
	private static List<object> friends = null;
	private List<string> rankFriendsList;

	protected override void Start ()
	{
		rank = new List<GameObject> ();
		rank.Add (GameObject.Find ("S" + Config.RANK1));
		rank.Add (GameObject.Find ("S" + Config.RANK2));
		rank.Add (GameObject.Find ("S" + Config.RANK3));
		foreach (GameObject gObj in rank) {
			gObj.SetActive (false);
		}

		httpComponent = gameObject.GetComponent<HttpComponent> ();
	}

	private void QueryScores ()
	{
		string tempFriends = FRIENDS_QUERY;
		Debug.Log (tempFriends);

		if (friends != null && friends.Count > 0) {
			RankExecute ();
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

		DeserializeJSONProfile (result.Text);
		friends = DeserializeJSONFriends (result.Text);
		rankFriendsList = new List<string> ();
		rankFriendsList.Add (FB.UserId);

		if (friends.Count > 0) {
			foreach (Dictionary<string, object> friend in friends) {
				rankFriendsList.Add (friend ["id"].ToString ());
			}
		}

		RankExecute ();
	}

	private void RankExecute ()
	{
		if (rankFriendsList == null) {
			return;
		}

		httpComponent.OnDone = (object obj) => {
			Dictionary<string, int> dic;
			if ((dic = obj as Dictionary<string, int>) != null) {
				var items = dic.Values.ToList ();
				items.Sort ();

				int count = dic.Count ();
				foreach (var key in items) {
					count--;

					rank [count].SetActive (true);
					GameObject gObj = rank [count];
					UITexture uITexture = gObj.GetComponent<UITexture> ();
					UIButton uIButton = GetChildObject (gObj, BUTTON).GetComponent<UIButton> ();
					UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
					UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();

					string id = MypageGameManager.DISABLE_QUEST;
					string score = key.ToString ();

					foreach (KeyValuePair<string, int> kVP in dic) {
						if (kVP.Value == (int)key) {
							id = kVP.Key;
							break;
						}
					}

					dic.Remove (id);
					uIButton.name = id;
					uILabel2.text = score;

					if (id == FB.UserId) {
						uILabel1.text = userFristName + " " + userLastName;
						if (userTexture != null) {
							uITexture.mainTexture = userTexture;
						}
						uIButton.gameObject.SetActive (false);
						continue;
					}

					if (friends.Count > 0) {
						foreach (Dictionary<string, object> f in friends) {
							if (id == f ["id"].ToString ()) {
								uILabel1.text = (string)f ["first_name"] + " " + (string)f ["last_name"];

								LoadPictureAPI (GetPictureURL (f ["id"].ToString (), TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture =>
								{
									if (pictureTexture != null) {
										uITexture.mainTexture = pictureTexture;
									}
								});
								break;
							}
						}
					}
				}
			}
		};
		
		httpComponent.StartGame (rankFriendsList, SceneData.stageLevel);
	}

	public void Rank ()
	{
		QueryScores ();
	}
}
