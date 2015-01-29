using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
	// array
	private ArrayList arrayList;
	// gameobject
	private GameObject prefab;
	// component
	private Transform parent;
	// variable
	private int dummyCount;
	
	public Pool (GameObject gObj, int dC, Transform p = null)
	{
		arrayList = new ArrayList ();
		dummyCount = dC;
		prefab = gObj;
		parent = p;

		Init ();
	}
	
	public GameObject Get ()
	{	
		foreach (GameObject gObj in arrayList) {
			if (gObj.activeSelf == false) {
				return gObj;
			}
		}

		GameObject temp = (GameObject)GameObject.Instantiate (prefab);
		Add (temp);

		return temp;
	}
	
	private void Init ()
	{
		GameObject temp;
		for (int i = 0; i < dummyCount; i++) {
			temp = (GameObject)GameObject.Instantiate (prefab);
			temp.SetActive (false);

			Add (temp);
		}
	}

	private void Add (GameObject gObj)
	{
		if (parent == null) {
			if (ObjectPool.ObjectPoolGObj () != null) {
				gObj.transform.parent = ObjectPool.ObjectPoolGObj ().transform;
			}
		} else {
			gObj.transform.parent = parent;
		}

		arrayList.Add (gObj);
	}
}

public class ObjectPool
{
	// const
	public const string OBJECTPOOL = "_ObjectPool";
	public const int CREATE_COUNT = 10;
	// static
	private static ObjectPool objectPool;
	// gameobject
	private GameObject objectPoolGObj;
	// array
	private Dictionary<GameObject, Pool> dic;

	static ObjectPool ()
	{
		if (objectPool == null) {
			objectPool = new ObjectPool ();
			
			objectPool.dic = new Dictionary<GameObject, Pool> ();
			
			if (objectPool.objectPoolGObj == null) {
				objectPool.objectPoolGObj = new GameObject (OBJECTPOOL);
			}
		}
	}

	private GameObject Get (GameObject prefab, int dummyCount, Transform parent = null)
	{
		if (!dic.ContainsKey (prefab)) {
			dic.Add (prefab, new Pool (prefab, dummyCount, parent));
		}
		
		Pool pool = dic [prefab];
		GameObject temp = pool.Get ();

		return temp;
	}

	private IEnumerator DelayDestory (GameObject gObj, float time)
	{
		yield return new WaitForSeconds (time);

		gObj.SetActive (false);
	}

	public static void Init (GameObject prefab, int count = CREATE_COUNT, Transform parent = null)
	{
		Transform temp = parent;
		if (temp != null) {
			GameObject parentGameObject = Logic.GetChildObject (temp.gameObject, OBJECTPOOL);
			if (parentGameObject == null) {
				GameObject tempGObj = new GameObject (OBJECTPOOL);
				tempGObj.transform.parent = parent;
				temp = tempGObj.transform;
			} else {
				temp = parentGameObject.transform;
			}

			if (objectPool.dic.ContainsKey (prefab)) {
				objectPool.dic.Remove (prefab);
			}
		}

 		objectPool.Get (prefab, count, temp);
	}

	public static GameObject ObjectPoolGObj ()
	{
		return objectPool.objectPoolGObj;
	}

	public static GameObject Instantiate (GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return Instantiate (prefab, position, rotation, CREATE_COUNT);
	}
	
	public static GameObject Instantiate (GameObject prefab, Vector3 position, Quaternion rotation, int count)
	{
		GameObject temp = objectPool.Get (prefab, count);
		temp.transform.position = position;
		temp.transform.rotation = rotation;
		
		temp.SetActive (true);

		return temp;
	}

	public static void Destroy (GameObject gObj)
	{
		gObj.SetActive (false);
	}
	
	public static void Destroy (MonoBehaviour mB, GameObject gObj, float time)
	{
		mB.StartCoroutine (objectPool.DelayDestory (gObj, time));
	}
}