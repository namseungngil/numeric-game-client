using UnityEngine;
using System.Collections.Generic;

public class AtRandom
{
	public static List<T> Randomize<T> (List<T> arr)
	{
		for (int t = 0; t < arr.Count; t++ )
		{
			T tmp = arr[t];
			int r = Random.Range(t, arr.Count);
			arr[t] = arr[r];
			arr[r] = tmp;
		}
		return arr;
	}
}