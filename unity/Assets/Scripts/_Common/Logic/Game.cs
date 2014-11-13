using UnityEngine;
using System.Collections.Generic;

public class Game
{
	public static List<int> Stage (int numberMax)
	{
		int score1 = 0;
		int score2 = 0;
		int score3 = 0;

		// Set score1
		score1 = numberMax * Config.CARD_COUNT;
		
		// Set score3
		for (int i = 1; i <= numberMax; i++) {
			score3 += i;
		}
		
		// Set score2
		int temp = (int)(score3 - score1 / 2);
		score2 = score1 + temp;

		List<int> list = new List<int> ();
		list.Add (score1);
		list.Add (score2);
		list.Add (score3);

		return list;
	}

	public static List<int> Quest (int index)
	{
		int min = (index * Config.CHAPTER_IN_QUEST) + Config.CARD_COUNT;
		int max = min + Config.CHAPTER_IN_QUEST - 1;

		List<int> list = new List<int> ();
		list.Add (min);
		list.Add (max);

		return list;
	}
}
