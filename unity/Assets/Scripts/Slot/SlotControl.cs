using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotControl : MonoBehaviour
{
	// component
	private SlotUIManager slotUIManager;
	private UISprite uISprite;
	private Vector3 position;
	// variable
	private int nameS;
	private bool stop;
	private bool slotStop;
	
	void Start ()
	{
		slotUIManager = GameObject.Find (Config.SLOT).GetComponent<SlotUIManager> ();
		uISprite = gameObject.GetComponent<UISprite> ();

		stop = true;
		slotStop = true;
	}

	void Update ()
	{
		nameS = int.Parse (gameObject.name);

		if (stop) {
			return;
		}

		int temp = nameS + 1;
		if (temp <= SlotUIManager.MAX / 2) {
			position = slotUIManager.Board (transform.parent.name, (temp).ToString ()).position;
		}

		float move = Time.deltaTime * slotUIManager.slotSpeed;
		transform.position = Vector3.MoveTowards (transform.position, position, move);

		if (transform.position == position) {
			if ((temp) >= SlotUIManager.MAX / 2) {
				gameObject.name = "0";
				transform.position = slotUIManager.Board (transform.parent.name, "0").position;
			} else {
				gameObject.name = temp.ToString ();
			}

			if (!slotStop) {
				stop = true;
			}
		}
	}

	public void SlotStart ()
	{
		stop = false;
		slotStop = true;
	}

	public void SlotStop ()
	{
		if (slotStop) {
			slotStop = false;
		}
	}

	public string GetSpriteName ()
	{
		return uISprite.spriteName;
	}
}
