using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MypageGameManager : GameManager
{
	// class
	private class QuestData
	{
		public GameObject myself = null;
		public UILabel uILabel = null;
		public UISprite uISprite = null;
		public UISprite star1 = null;
		public UISprite star2 = null;
		public UISprite star3 = null;
		public UISprite qlock = null;

		public QuestData (GameObject gO, UILabel uL)
		{
			myself = gO;
			uILabel = uL;
		}
	}
	// const
	public const string DISABLE_QUEST = "-1";
	private const int LAYERX = 4;
	private const string BACKGROUND = "Background";
	private const string STAR1 = "Star1";
	private const string STAR2 = "Star2";
	private const string STAR3 = "Star3";
	private const string LOCK = "Lock";
	private const string UNTAGGED = "Untagged";
	// component
	private QueryModel dataQuery;
	Register register;
	private UIAtlas uIAtlas;
	// array
	public UIAtlas[] map;
	private List<QuestData> quest;
	// variable
	private int index;
	private bool nextFlag;

	void Start ()
	{
		GameObject.Find (Config.ROOT_MANAGER).GetComponent<LoveComponent> ().Set ();

		quest = new List<QuestData> ();
		GameObject panel200 = GameObject.Find (Config.PANEL200);
		foreach (UIButton uIButton in panel200.GetComponentsInChildren<UIButton> ()) {
			QuestData tempQuestData = new QuestData (uIButton.gameObject, uIButton.gameObject.GetComponentInChildren<UILabel> ());
			foreach (UISprite u in uIButton.gameObject.GetComponentsInChildren<UISprite> ()) {
				if (u.gameObject.name == BACKGROUND) {
					tempQuestData.uISprite = u;
				} else if (u.gameObject.name == STAR1) {
					tempQuestData.star1 = u;
				} else if (u.gameObject.name == STAR2) {
					tempQuestData.star2 = u;
				} else if (u.gameObject.name == STAR3) {
					tempQuestData.star3 = u;
				} else if (u.gameObject.name == LOCK) {
					tempQuestData.qlock = u;
				}
			}

			quest.Add (tempQuestData);
		}

		dataQuery = QueryModel.Instance ();
		register = Register.Instance ();
		index = 0;
		nextFlag = false;
		SetQuest ();
	}

	private void SetQuest (int ind = -1)
	{
		// exception
		if (ind >= (map.Length * Config.STAGE_COLOR_COUNT)) {
			return;
		}

		// set index
		if (ind == -1) {
			index = register.GetStage ();
		} else {
			index = ind;
			register.SetStage (index);
		}

		// Set static
		SenceData.currentLevel = Config.MYPAGE + (index / Config.STAGE_COLOR_COUNT).ToString ();

		// set map
		foreach (UISprite uS in gameObject.GetComponentsInChildren<UISprite> ()) {
			if (uS.gameObject.tag != UNTAGGED) {
				uS.atlas = map [index / Config.STAGE_COLOR_COUNT];
			}
		}

		// set name
		List<int> questList = Game.Quest (index);
		int tempMin = questList [0];
		foreach (QuestData qD in quest) {
			qD.myself.SetActive (true);

			qD.myself.name = tempMin.ToString ();
			qD.uILabel.text = tempMin.ToString ();

			qD.star1.gameObject.SetActive (false);
			qD.star2.gameObject.SetActive (false);
			qD.star3.gameObject.SetActive (false);
			qD.qlock.gameObject.SetActive (false);

			tempMin++;
		}

		// set user clear quest
		DataTable dataTable = dataQuery.MypageQuestList (index);

		if (dataTable.Rows.Count > 0) {
			if (dataTable.Rows.Count == Config.CHAPTER_IN_QUEST) {
				nextFlag = true;
			}

			for (int i = 0; i < dataTable.Rows.Count; i++) {
				int score = (int)dataTable [i] [QueryModel.SCORE];
				Debug.Log (score);
				int stage = int.Parse (quest[i].myself.name);

				List<int> list = Game.Score (stage);
				Debug.Log (list[0] + " : " + list[1] + " : " + list[2]);
				if (score >= list[0]) {
					quest[i].star1.gameObject.SetActive (true);
					if (score >= list[1]) {
						quest[i].star2.gameObject.SetActive (true);
						if (score >= list[2]) {
							quest[i].star3.gameObject.SetActive (true);
						}
					}
				}
			}
		}

		// set next quest
		if (!nextFlag) {
			quest[dataTable.Rows.Count].uISprite.gameObject.SetActive (false);
		}

		// set impossible quest
		for (int i = dataTable.Rows.Count + 1; i < quest.Count; i++) {
			quest[i].myself.name = DISABLE_QUEST;
			quest[i].uISprite.gameObject.SetActive (false);
			quest[i].qlock.gameObject.SetActive (true);
		}
	}

	public void NextQuest ()
	{
		if (!NextQuestStatus ()) {
			return;
		}

		index++;
		SetQuest (index);
	}

	public void BackQuest ()
	{
		if (!BackQuestStatus ()) {
			return;
		}

		index--;
		SetQuest (index);
	}

	public bool NextQuestStatus ()
	{
		if (nextFlag) {
			if (index < map.Length) {
				return true;
			}
		}

		return false;
	}

	public bool BackQuestStatus ()
	{
		if (index != 0) {
			return true;
		}

		return false;
	}

	protected override void AndroidBackButton ()
	{
		Application.Quit ();
	}
}
