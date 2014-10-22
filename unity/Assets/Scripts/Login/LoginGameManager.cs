using UnityEngine;
using System.Collections;

public class LoginGameManager : GameManager
{
	new void Update ()
	{
		base.Update ();
	}

	protected override void AndroidBackButton ()
	{
		Application.Quit ();
	}
}
