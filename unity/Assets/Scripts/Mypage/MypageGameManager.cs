using UnityEngine;
using System.Collections;

public class MypageGameManager : GameManager
{
	// const
//	private const string MAP1 = "1";
	private const int LAYERX = 4;
	// component
	private QueryModel dataQuery;

	void Start ()
	{
		SetMypage ();
	}

	new void Update ()
	{ 
		base.Update ();
	}

	protected override void AndroidBackButton ()
	{
		Application.Quit ();
	}

	public void SetMypage ()
	{
//		Application.LoadLevelAdditive (MAP1);
		dataQuery = QueryModel.Instance ();
		DataTable dataTable = dataQuery.MypageQuestList ();
		
		GameObject grid = GameObject.Find (Config.GRID);
		GameObject stage9 = GameObject.Find (Config.STAGE9);

		UIButton[] childGameObject = grid.GetComponentsInChildren<UIButton> ();
		foreach (UIButton uiButton in childGameObject) {
			if (uiButton.gameObject.name != stage9.name) {
				Destroy (uiButton.gameObject);
			}
		}

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
				
				GameObject dump = NGUITools.AddChild (grid, stage9);
				dump.name = dumpName;
				Debug.Log (dumpName);
				UILabel uiLabel = dump.GetComponentInChildren<UILabel> ();
				uiLabel.text = dumpName;
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

}
