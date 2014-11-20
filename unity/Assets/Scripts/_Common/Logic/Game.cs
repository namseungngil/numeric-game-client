using UnityEngine;
using System.Collections.Generic;

public class Game
{
	private const int PLUS_SCORE = 20;

	public static List<int> Score (int numberMax)
	{
		int score1 = numberMax * Config.CARD_COUNT;
		int temp = score1 / 2;
		int score2 = score1 + (temp / 2);
		int score3 = score1 + temp;

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

	public static string Scene (string s)
	{
		Register register = Register.Instance ();
//		register.SetStage (0);
		int temp = register.GetStage ();
		if (temp < 0) {
			temp = 0;
		}
		
		temp = temp / Config.STAGE_COLOR_COUNT;

		return s + temp.ToString ();
	}
}
