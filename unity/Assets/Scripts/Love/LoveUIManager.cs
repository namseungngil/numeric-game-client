using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoveUIManager : UIManager
{
	// const
	private const string GRIDLIST = "GridList";
	private const string SEND = "Send";
	private const string MYPINVITE_CHECK10 = "myPinvite_check10";
	private const string MYPINVITE_CHECK11 = "myPinvite_check11";
	// gameobject
	private GameObject send;
	private GameObject gridList;
	// component
	private LoveFacebookManager loveFacebookManager;
	private UISprite allUISprite;
	// array
	private List<string> list;
	private List<LoveToggleControl> allUIToggle;
	private bool allFlag;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}
	
	public override void Start ()
	{
		send = GameObject.Find (SEND);
		gridList = GameObject.Find (GRIDLIST);
		loveFacebookManager = gridList.GetComponent<LoveFacebookManager> ();

		list = new List<string> ();
		allFlag = false;

		send.SetActive (false);
	}

	protected override void Update ()
	{
		base.Update ();
	}
	
	public void Check (UIToggle uIToggle)
	{
		if (uIToggle.value) {
			if (!list.Contains (uIToggle.name)) {
				list.Add (uIToggle.name);
			}
		} else {
			if (list == null) {
				return;
			}

			if (list.Contains (uIToggle.name)) {
				list.Remove (uIToggle.name);
			}
		}

		if (list.Count > 0) {
			send.SetActive (true);
		} else {
			send.SetActive (false);
		}
	}

	public void Request ()
	{
		if (list.Count <= 0) {
			return;
		}
		
		if (loveFacebookManager != null) {
			string[] temp = new string[list.Count];
			for (int i = 0; i < list.Count; i++) {
				temp[i] = list[i];
			}
			Debug.Log ("Request : " + temp.Length);
			loveFacebookManager.onChallengeClicked (temp);
		}
	}

	private void AllCheck ()
	{
		if (allFlag) {
			allUISprite.spriteName = MYPINVITE_CHECK10;
		} else {
			allUISprite.spriteName = MYPINVITE_CHECK11;
		}
	}

	public void All ()
	{
		if (allUISprite == null) {
			allUISprite = Logic.GetChildObject (UIButton.current.gameObject, "Background").GetComponent<UISprite> ();
		}

		if (allUIToggle == null) {
			allUIToggle = new List<LoveToggleControl> ();
			foreach (LoveToggleControl lTC in gridList.GetComponentsInChildren<LoveToggleControl> ()) {
				Debug.Log (lTC.name);
				allUIToggle.Add (lTC);
			}
		}

		if (allFlag) {
			allFlag = false;
			allUISprite.spriteName = MYPINVITE_CHECK10;
		} else {
			allFlag = true;
			allUISprite.spriteName = MYPINVITE_CHECK11;
		}

		if (allUIToggle != null && allUIToggle.Count > 0) {
			foreach (LoveToggleControl ltC in allUIToggle) {
				ltC.OnEnabled (allFlag);
			}
		}
	}
}
