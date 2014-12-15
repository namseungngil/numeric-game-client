using UnityEngine;
using System.Collections;

public class LoadingControl : MonoBehaviour
{
	// const
	private const float ANGLE = 360f;
	// variable
	public float speed = 1f;
	private float z;

	void Start ()
	{
		z = ANGLE;
	}
	
	void Update ()
	{
		z -= Time.deltaTime * speed;
		if (z <= 0) {
			z = ANGLE;
		}
		transform.localRotation = Quaternion.Euler (transform.localRotation.x, transform.localRotation.y, z);
	}
}
