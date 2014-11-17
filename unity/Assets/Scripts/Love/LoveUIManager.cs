using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoveUIManager : UIManager
{
	// component
	private LoveFacebookManager loveFacebookManager;
	// array
	private List<string> list;

	public override void Awake ()
	{
		BgmType = Bgm.NONE;
		BgmName = string.Empty;
		
		IsCache = false;
	}
	
	public override void Start ()
	{
		loveFacebookManager = GameObject.Find (GRIDLIST).GetComponent<LoveFacebookManager> ();
		list = new List<string> ();
	}
	
	public void Check ()
	{
		UIToggle uIToggle = UIToggle.current.gameObject.GetComponent<UIToggle> ();
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
}
