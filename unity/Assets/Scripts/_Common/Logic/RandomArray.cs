using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;

public class RandomArray
{
//	private static System.Random _random = new System.Random ();

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

//		List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>> ();
//		// Add all strings from array
//		// Add new random int each time
//		foreach (string s in arr) {
//			list.Add (new KeyValuePair<int, string> (_random.Next (), s));
//		}
//		// Sort the list by the random number
////		List<KeyValuePair<int, string>> sorted = from item in list
////			orderby item.Key
////				select item;
//		// Allocate new string array
//		string[] result = new string[arr.Length];
//		// Copy values to array
//		int index = 0;
//		foreach (KeyValuePair<int, string> pair in list) {
//			result [index] = pair.Value;
//			index++;
//		}
//		// Return copied array
//		return result;
	}
}