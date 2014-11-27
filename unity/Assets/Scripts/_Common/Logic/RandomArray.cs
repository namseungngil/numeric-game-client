using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;

public class RandomArray
{
	public static string[] RandomizeStrings (string[] arr)
	{
		for (int t = 0; t < arr.Length; t++ )
		{
			string tmp = arr[t];
			int r = Random.Range(t, arr.Length);
			arr[t] = arr[r];
			arr[r] = tmp;
		}
		return arr;
	}
}