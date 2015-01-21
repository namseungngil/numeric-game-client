//------------------------------------------------------------------------------
// Comment : Save UIRoot GameObject.
// 
// @Date : 2014/08/14 16:56
// @Anthor : Nam seungil (南勝壹, Rio)
//------------------------------------------------------------------------------
using UnityEngine;

public class StarUIRoot : MonoBehaviour
{
	static public GameObject go;

	void Awake ()
	{
		go = gameObject;
	}
}