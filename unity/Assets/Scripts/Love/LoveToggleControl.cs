using UnityEngine;
using System.Collections;

public class LoveToggleControl : MonoBehaviour
{
	public void OnToggleChange ()
	{
		gameObject.GetComponentInParent<LoveUIManager> ().Check (gameObject.GetComponent<UIToggle> ());
	}

	public void OnEnabled (bool flag)
	{
		gameObject.GetComponent<UIToggle> ().value = flag;
	}
}
