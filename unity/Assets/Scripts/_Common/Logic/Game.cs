using UnityEngine;
using System.Collections.Generic;

public class Game
{
	private const int PLUS_SCORE = 20;

	public static List<int> Score (int numberMax)
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

		if (score1 > score3) {
			int tempChange = score3;
			score3 = score1;
			score1 = tempChange;
		} else if (score1 == score3) {
			score3 += PLUS_SCORE;
		}
		
		// Set score2
		int temp = (int)(score3 - score1 / 2);
		score2 = score1 + temp;

		if (score2 > score3) {
			int tempScore = score3;
			score3 = score2;
			score2 = tempScore;
		} else if (score2 == score3) {
			score3 += PLUS_SCORE;
		}

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
