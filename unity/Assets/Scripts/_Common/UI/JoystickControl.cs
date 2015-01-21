//------------------------------------------------------------------------------
// Comment : Joystick component.
// 
// @Date : 2014/07/14 12:03
// @Anthor : Nam seungil (南勝壹, Rio)
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

public class JoystickControl : MonoBehaviour
{


	// const
	private const string NAME = "PanelButton:200";
	// component
	private StarUIJoystick starUIJoystick;
	private UISprite uiSpirte;
	private GameObject target;
	// variable




	void Awake ()
	{
		starUIJoystick = GetComponentInChildren<StarUIJoystick> ();
		starUIJoystick.gameObject.SetActive (false);
		uiSpirte = GetComponentInChildren<UISprite> ();
		uiSpirte.gameObject.SetActive (false);


		target = GameObject.Find (NAME);
	}

	
	void OnDragEnter(Vector3 point)
	{

		SetActive (true);
		
		Vector3 uipos = NGUITools.FindCameraForLayer (gameObject.layer).camera.ScreenToWorldPoint (Input.mousePosition);
		float x = uipos.x;
		float y = uipos.y;
		//			float z = uipos.z;
		
		transform.position = new Vector3 (x, y, 0);
		starUIJoystick.StartOnPress (transform);
	}
	
	void OnDragLeave(Vector3 point)
	{
		starUIJoystick.EndOnPress ();
		SetActive (false);
	}


	void OnDrag()
	{
	}


	
	private void SetActive (bool flag)
	{
		starUIJoystick.gameObject.SetActive (flag);
		uiSpirte.gameObject.SetActive (flag);
	}
	
	private bool Search (string name)
	{
		if (target.name == name) return false;

		foreach (UIButton uIButton in target.GetComponentsInChildren<UIButton> ()) {
			if (uIButton.gameObject.name == name) {
				return false;
			}
		}

		return true;
	}
}