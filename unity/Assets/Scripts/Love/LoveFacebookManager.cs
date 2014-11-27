using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class LoveFacebookManager : FacebookManager
{
	// gameobject
	public GameObject firend;
	// component
	private LoveUIManager loveUIManager;
	// array
	private List<object> friends = null;
	
	protected override void Start ()
	{
		Debug.Log ("LoveFacebookManager Start");
		count = 0;
		loveUIManager = gameObject.GetComponentInParent<LoveUIManager> ();
		if (loveUIManager != null) {
			uIManager = loveUIManager;
		}

		GameObject temp = GameObject.Find (Config.ROOT_MANAGER);
		if (temp != null) {
			loveComponent = temp.GetComponent<LoveComponent> ();
		}

		string tempFriends = FRIENDS_QUERY;
		
		if (FB.IsLoggedIn) {
//			if (friends != null && friends.Count > 0) {
//				FriendsView ();
//			} else {
			Debug.Log (tempFriends);
			FB.API (tempFriends, Facebook.HttpMethod.GET, FriendsCallback);
//			}
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

		FriendsView ();
	}

	private void FriendsView ()
	{
		Debug.Log ("friend count : " + friends.Count);
		Debug.Log (firend);
		if (friends.Count > 0) {
			foreach (Dictionary<string, object> f in friends) {
				Debug.Log ("[" + (string)f ["first_name"] + " - " + (string)f ["last_name"] + "]");
				
				GameObject gObj = Instantiate (firend, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				Debug.Log (gObj);
				gObj.name = f ["id"].ToString ();
				gObj.transform.parent = this.transform;
				gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
				Vector3 vector3 = gObj.transform.localPosition;
				gObj.transform.localPosition = new Vector3 (0, vector3.y, vector3.z);
				
				UITexture uITexture = GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
				UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
				UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();
				
				uILabel1.text = f ["first_name"].ToString ();
				uILabel2.text = f ["last_name"].ToString ();

				LoadPictureAPI (GetPictureURL (f ["id"].ToString (), TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture =>
				{
					if (pictureTexture != null) {
						uITexture.mainTexture = pictureTexture;
					}
				});

			}

			gameObject.GetComponent<UIGrid> ().Reposition ();
		} else {
			// loading...

		}
	}
}
