using UnityEngine;
using System.Collections;

public class MypageGameManager : GameManager
{
	// const
//	private const string MAP1 = "1";
	private const int LAYERX = 4;
	// component
	private QueryModel dataQuery;

	void Awake ()
	{
//		Application.LoadLevelAdditive (MAP1);
		dataQuery = QueryModel.Instance ();
		DataTable dataTable = dataQuery.MypageQuestList ();

		GameObject grid = GameObject.Find (Config.GRID);
		GameObject stage9 = GameObject.Find (Config.STAGE9);
		UIGrid uiGrid = grid.GetComponent<UIGrid> ();
		UISprite uiSprite = stage9.GetComponent<UISprite> ();
		Vector2 size = uiSprite.localSize;
		Debug.Log ("size" + size);
		Vector3 position = stage9.transform.localPosition;
		Debug.Log ("position" + position);
		
		uiGrid.cellWidth = size.x;
		uiGrid.cellHeight = size.y;
		
		if (dataTable.Rows.Count > 0) {
//			Destroy (stage9);
			for (int i = dataTable.Rows.Count - 1; i >= 0; i--) {
				string dumpName = "" + dataTable [i] [Query.questStage];
				Debug.Log (dumpName);
//				if (dumpName == stage9.name) {
//					continue;
//				}

				GameObject dump = NGUITools.AddChild (grid, stage9);
				dump.name = dumpName;
				Debug.Log (dumpName);
				UILabel uiLabel = dump.GetComponentInChildren<UILabel> ();
				uiLabel.text = dumpName;
				
//				int division = i / layerX;
//				int reset = i;
//				if (i > layerX) {
//					reset = i % layerX;
//				}
				
//				float dumpX = position.x + (reset * size.x);
//				float dumpY = position.y + (division * size.y);
//				Vector3 dumpPosition = new Vector3 (dumpX, dumpY, 0);
//				dump.transform.localPosition = dumpPosition;
			}

			float defaultX = size.x / 2;
			float gridX = defaultX;
			if (dataTable.Rows.Count < LAYERX) {
				for (int i = 0; i < dataTable.Rows.Count - 1; i++) {
					gridX += defaultX;
				}
			} else {
				gridX = defaultX * LAYERX;
			}

			Debug.Log ("grid X : " + gridX);
			grid.transform.localPosition = new Vector3 (gridX, 0, 0);
			stage9.SetActive (false);
		}
	}
	
	new void Update ()
	{ 
		base.Update ();
	}



	protected override void AndroidBackButton ()
	{
		Application.Quit ();
	}


}
