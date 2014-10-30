using UnityEngine;
using System.Collections;

public class UIManager : SSController
{
	public void Cancel ()
	{
		SSSceneManager.Instance.Close ();
	}
}
