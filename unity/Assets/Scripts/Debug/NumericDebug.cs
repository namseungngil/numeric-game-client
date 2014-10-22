using UnityEngine;
using System.Collections;

public class NumericDebug : MonoBehaviour
{
	private UILabel uiLabel;

	void Start ()
	{
		uiLabel = GetComponent<UILabel> ();
		uiLabel.text = Device.Language ();
	}
	
	public void Set (string str)
	{
		uiLabel.text = str;
	}
}
