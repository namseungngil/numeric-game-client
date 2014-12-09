using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logic
{
	public static GameObject GetChildObject (GameObject gO, string strName)
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

	public static void SetActive (List<GameObject> list, bool flag)
	{
		foreach (GameObject gObj in list) {
			gObj.SetActive (flag);
		}
	}
}
