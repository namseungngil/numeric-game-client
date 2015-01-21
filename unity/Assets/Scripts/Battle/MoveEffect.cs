using UnityEngine;
using System.Collections;

public class MoveEffect : MonoBehaviour
{
	// gameobject
	public GameObject effect;
	// component
	public Vector3 target;
	private Vector3 prevPos;
	private Vector3 startPos;
	private Transform trans;
	// variable
	public float speed = 5f;
	private float time;
	private float timer = 0.0f;
	private float xPow;
	private float yPow;
	private float zPow;

	void OnEnable ()
	{
		trans = transform;
		prevPos = trans.position;
		startPos = transform.position;
		time = (target - trans.position).magnitude / speed;
	}

	void Start ()
	{
		xPow = Random.Range (0.4f, 3.0f);
		yPow = Random.Range (0.4f, 3.0f);
		zPow = Random.Range (0.4f, 3.0f);
	}

	void Update ()
	{
		Vector3 v3 = startPos;
		v3.x = Mathf.Lerp (v3.x, target.x, Mathf.Pow (timer/time, xPow));
		v3.y = Mathf.Lerp (v3.y, target.y, Mathf.Pow (timer/time, yPow));
		v3.z = Mathf.Lerp (v3.z, target.z, Mathf.Pow (timer/time, zPow));
		trans.position = v3;
		timer += Time.deltaTime;
		
		if (trans.position != prevPos) {
			trans.rotation = Quaternion.LookRotation (trans.position - prevPos);
		}
		
		prevPos = trans.position;
		
		if (transform.position == target) {
			StartCoroutine (EDestory ());
		}
	}
	
	private IEnumerator EDestory ()
	{
		yield return new WaitForSeconds (0f);
		ObjectPool.Destroy (gameObject);
		
		if (effect != null) {
//			ObjectPoolManager.CreateClone (effect, trans.position, Quaternion.identity);
		}
	}

}
