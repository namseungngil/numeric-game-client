using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class LoveFacebookManager : FacebookManager
{
	// gameobject
	private GameObject firend;
	// component
	private LoveUIManager loveUIManager;
	// array
	private List<object> friends = null;

	protected override void Start ()
	{
		count = 0;
		firend = gameObject.GetComponentInChildren<UIToggle> ().gameObject;
		loveUIManager = gameObject.GetComponentInParent<LoveUIManager> ();
		if (loveUIManager != null) {
			uIManager = loveUIManager;
		}

		GameObject temp = GameObject.Find (Config.ROOT_MANAGER);
		if (temp != null) {
			loveComponent = temp.GetComponent<LoveComponent> ();
		}

		Friends ();
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
		Debug.Log ("friend count : " + friends.Count);

		if (friends.Count > 0) {
			foreach (Dictionary<string, object> friend in friends) {
				Debug.Log ("[" + (string)friend ["first_name"] + " - " + (string)friend ["last_name"] + "]");
	
				GameObject gObj = Instantiate (firend, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				gObj.name = friend ["id"].ToString ();
				gObj.transform.parent = this.transform;
				gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
				Vector3 vector3 = gObj.transform.localPosition;
				gObj.transform.localPosition = new Vector3 (0, vector3.y, vector3.z);

				UITexture uITexture = GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
				UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
				UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();

				uILabel1.text = friend ["first_name"].ToString ();
				uILabel2.text = friend ["last_name"].ToString ();

				LoadPictureAPI (GetPictureURL (friend ["id"].ToString (), TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture =>
				{
					if (pictureTexture != null) {
						uITexture.mainTexture = pictureTexture;
					}
				});
			}

			firend.SetActive (false);
			gameObject.GetComponent<UIGrid> ().Reposition ();
		} else {
			// loading...
			firend.SetActive (false);
		}
	}
	
	private void Friends ()
	{
		string tempFriends = FRIENDS_QUERY;
		Debug.Log (tempFriends);

//		for (int i = 0; i < 10; i++) {
//			GameObject gObj = Instantiate (firend, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
//			gObj.name = i.ToString ();
//			gObj.transform.parent = this.transform;
//			gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
//			Vector3 vector3 = gObj.transform.localPosition;
//			gObj.transform.localPosition = new Vector3 (0, vector3.y, vector3.z);
//			
//			UITexture uITexture = GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
//			UILabel uILabel1 = GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
//			UILabel uILabel2 = GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();
//			
//			uILabel1.text = i.ToString ();
//			uILabel2.text = i.ToString ();
//		}
//
//		firend.SetActive (false);
//		gameObject.GetComponent<UIGrid> ().Reposition ();

		if (friends != null) {
			return;
		}
		
		if (FB.IsLoggedIn) {
			FB.API (tempFriends, Facebook.HttpMethod.GET, FriendsCallback);
		}
	}
}
