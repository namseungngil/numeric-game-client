using UnityEngine;
using System.Collections;

public class FireflyComponent : MonoBehaviour
{
	public float moveSpeed = 1;
	private float maxX = 6.1f;
	private float minX = -6.1f;
	private float maxY = 4.2f;
	private float minY = -4.2f;
	private float tChange = 0;
	private float randomX;
	private float randomY;

	void Update ()
	{
		// change to random direction at random intervals
		if (Time.time >= tChange) {
			randomX = Random.Range (-2.0f, 2.0f); // with float parameters, a random float
			randomY = Random.Range (-2.0f, 2.0f); //  between -2.0 and 2.0 is returned
			// set a random interval between 0.5 and 1.5
			tChange = Time.time + Random.Range (0.5f, 1.5f);
		}
		transform.Translate (new Vector3 (randomX, randomY, 0) * moveSpeed * Time.deltaTime);
		// if object reached any border, revert the appropriate direction
		if (transform.position.x >= maxX || transform.position.x <= minX) {
			randomX = -randomX;
		}
		if (transform.position.y >= maxY || transform.position.y <= minY) {
			randomY = -randomY;
		}
		// make sure the position is inside the borders
		float tempX = Mathf.Clamp (transform.localPosition.x, minX, maxX);
		float tempY = Mathf.Clamp (transform.localPosition.y, minY, maxY);
		float tempZ = transform.position.z;

		transform.position = new Vector3 (tempX, tempY, tempZ);
	}
}
