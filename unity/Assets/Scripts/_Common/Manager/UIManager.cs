using UnityEngine;
using System.Collections;

public class UIManager : SSController
{
	protected const string GRIDLIST = "GridList";

	public void Cancel ()
	{
		SSSceneManager.Instance.Close ();
	}
}
