using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OBJECT
{
	public class ObjectPool
	{
		public const int CREATE_COUNT = 10;
		public const string DUMMY_NAME = "_Dummy";
		private GameObject dummy;
		private Object prefab;
		private ArrayList list;
		private int dummyCount;
		
		public ObjectPool (Object objPrefab, int dC)
		{
			dummyCount = dC;
			list = new ArrayList (dummyCount);
			prefab = objPrefab;
			CreateGameObject ();
		}
		
		public GameObject FindGameObject ()
		{
			
			foreach (GameObject info in list) {
				if (info.activeSelf == false) {
					info.SetActive (true);
					return info;
				}
			}
			CreateGameObject ();
			return FindGameObject ();
			
		}
		
		private void CreateGameObject ()
		{
			dummy = GameObject.Find (DUMMY_NAME);
			if (dummy == null) {
				dummy = new GameObject (DUMMY_NAME);
			}
			ArrayList array = new ArrayList (dummyCount);
			GameObject obj;
			for (int i = 0; i < dummyCount; i++) {
				obj = (GameObject)GameObject.Instantiate (prefab);
				if (dummy != null) {
					obj.transform.parent = dummy.transform;
				}
				obj.SetActive (false);
				array.Add (obj);
			}
			list.AddRange (array);
		}
		
	}
	
	public class ObjectPoolManager
	{
		private Dictionary<Object, ObjectPool> objectPool;

		private ObjectPoolManager ()
		{
			objectPool = new Dictionary<Object, ObjectPool> ();
		}
		
		public GameObject CreateClone (Object o, Vector3 position, Quaternion rotation)
		{
			return CreateClone (o, position, rotation, ObjectPool.CREATE_COUNT);
		}
		
		public GameObject CreateClone (Object o, Vector3 position, Quaternion rotation, int dummyCount)
		{
			GameObject obj = FindGameObject (o, dummyCount);
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			return obj;
		}
		
		private GameObject FindGameObject (Object o, int dummyCount)
		{
			if (!objectPool.ContainsKey (o)) {
				objectPool.Add (o, new ObjectPool (o, dummyCount));
			}
			ObjectPool pool = objectPool [o];
			return pool.FindGameObject ();
		}
		
		private void Destroy (GameObject gO)
		{
			gO.SetActive (false);
		}
		
		public void DestroyClone (GameObject gO)
		{
			Destroy (gO);
		}
		
		public void DestroyClone (GameObject gO, float time)
		{
			
			MonoBehaviour mono = gO.GetComponent<MonoBehaviour> ();
			mono.StartCoroutine (DelayDestoryClone (gO, time));
		}
		
		public IEnumerator DelayDestoryClone (GameObject gO, float time)
		{
			yield return new WaitForSeconds (time);
			Destroy (gO);
		}
	}
}


