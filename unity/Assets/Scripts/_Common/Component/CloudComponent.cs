using UnityEngine;
using System.Collections;

public class CloudComponent : MonoBehaviour
{
	public float speed = 1f;
	private UISprite uISPrite;

	void Start ()
	{
		uISPrite = gameObject.GetComponent<UISprite> ();
	}

	void Update ()
	{
		transform.localPosition = new Vector3 (transform.localPosition.x + Time.deltaTime + speed, transform.localPosition.y, transform.localPosition.z);
		if (transform.localPosition.x > (Screen.width + uISPrite.pixelSize)) {
			transform.localPosition = new Vector3 (((-Screen.width) - uISPrite.pixelSize), transform.localPosition.y, transform.localPosition.z);
		}
	}
}
