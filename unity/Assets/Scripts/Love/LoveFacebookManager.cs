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
	
	protected override void Start ()
	{
		count = 0;
		loveUIManager = gameObject.GetComponentInParent<LoveUIManager> ();
		if (loveUIManager != null) {
			uIManager = loveUIManager;
		}

		GameObject temp = GameObject.Find (Config.ROOT_MANAGER);
		if (temp != null) {
			loveComponent = temp.GetComponent<LoveComponent> ();
		}

		if (FB.IsLoggedIn) {
			if (friends != null && friends.Count > 0) {
				InvitableFriendsCallback ();
			} else {
				FriendsCallback ();
			}
		}
	}

	private void FriendsCallback (FBResult result = null)
	{
//		Debug.Log ("FriendsCallback");

		bool flag = false;
		if (result == null) {
			flag = true;
		}

		if (!flag && result.Error != null) {
//			Debug.Log ("FriendsCallback error : " + result.Error);
			flag = true;
		}

		if (flag) {
			string temp = FRIENDS_QUERY;
			FB.API (temp, Facebook.HttpMethod.GET, FriendsCallback);
			return;
		}

		friends = DeserializeJSONFriends (result.Text);

		if (invitableFriends != null && invitableFriends.Count > 0) {
			FriendsView ();
		} else {
			InvitableFriendsCallback ();
		}
	}

	private void InvitableFriendsCallback (FBResult result = null)
	{
//		Debug.Log ("InvitableFriendsCallback");

		bool flag = false;
		if (result == null) {
			flag = true;
		}

		if (!flag && result.Error != null) {
//			Debug.Log ("InvitableFriendsCallback error : " + result.Error);
			flag = true;
		}

		if (flag) {
			string temp = INVITABLE_FRIENDS_QUERY;
			FB.API (temp, Facebook.HttpMethod.GET, InvitableFriendsCallback);
			return;
		}

		invitableFriends = DeserializeJSONFriends (result.Text);

		FriendsView ();
	}

	private void SetFriends (List<object> l)
	{
		foreach (Dictionary<string, object> f in l) {
//			Debug.Log ("[" + (string)f ["first_name"] + " - " + (string)f ["last_name"] + "]");
			
			GameObject gObj = Instantiate (firend, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
//			Debug.Log (gObj);
			gObj.name = f ["id"].ToString ();
			gObj.transform.parent = this.transform;
			gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
			Vector3 vector3 = gObj.transform.localPosition;
			gObj.transform.localPosition = new Vector3 (0, vector3.y, vector3.z);
			
			UITexture uITexture = Logic.GetChildObject (gObj, TEXTURE).GetComponent<UITexture> ();
			UILabel uILabel1 = Logic.GetChildObject (gObj, LABEL1).GetComponent<UILabel> ();
			UILabel uILabel2 = Logic.GetChildObject (gObj, LABEL2).GetComponent<UILabel> ();
			
			uILabel1.text = f ["first_name"].ToString ();
			uILabel2.text = f ["last_name"].ToString ();
			
			LoadPictureAPI (GetPictureURL (f ["id"].ToString (), TEXTURE_SIZE, TEXTURE_SIZE), pictureTexture =>
			                {
				if (pictureTexture != null) {
					uITexture.mainTexture = pictureTexture;
				}
			});
		}
	}

	private void FriendsView ()
	{
//		Debug.Log ("Friends count : " + friends.Count);
//		Debug.Log ("InvitableFriends count : " + invitableFriends.Count);

		if (friends != null && friends.Count > 0) {
			SetFriends (friends);
		}

		if (invitableFriends != null && invitableFriends.Count > 0) {
			SetFriends (invitableFriends);
		}

		gameObject.GetComponent<UIGrid> ().Reposition ();
	}
}
