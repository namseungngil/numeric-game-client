using UnityEngine;
using System.Collections;

public class Date
{
	public static int[] Slice (string time)
	{
		int year = int.Parse (time.Substring (0, 4));
		int month = int.Parse (time.Substring (4, 2));
		int day = int.Parse (time.Substring (6, 2));
		int hour = int.Parse (time.Substring (8, 2));
		int minute = int.Parse (time.Substring (10, 2));
		int second = int.Parse (time.Substring (12, 2));

		return new int[] {
			year, month, day, hour, minute, second
		};
	}
}
