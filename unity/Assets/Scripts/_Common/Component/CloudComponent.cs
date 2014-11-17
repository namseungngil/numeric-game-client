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
		transform.position = new Vector3 (transform.position.x + Time.deltaTime + speed, transform.position.y, transform.position.z);
		if (transform.position.x > (Config.CONTENT_WIDTH + uISPrite.pixelSize)) {
			transform.position = new Vector3 ((0 - uISPrite.pixelSize), transform.position.y, transform.position.z);
		}
	}
}
