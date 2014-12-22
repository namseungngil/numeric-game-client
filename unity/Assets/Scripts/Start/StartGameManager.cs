using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGameManager : MonoBehaviour
{
	// component
	private RankFacebookManager rankFacebookManager;
	private HttpComponent httpComponent;
	private QueryModel queryModel;

	void Start ()
	{
		rankFacebookManager = gameObject.GetComponent<RankFacebookManager> ();
		httpComponent = gameObject.GetComponent<HttpComponent> ();

		queryModel = QueryModel.Instance ();

		Score ();
	}

	private void Score ()
	{
//		Debug.Log ("StartGameManager Score");
		if (!FB.IsLoggedIn) {
			return;
		}

		int tempScore = int.Parse (SceneData.score);
		if (tempScore > 0) {
			DataTable dataTable = queryModel.MypageStage (SceneData.stageLevel);
			if (dataTable.Rows.Count > 0) {
				httpComponent.OnDone = (object obj) => {
//					Debug.Log ("httpComponent OnDone");
					rankFacebookManager.Rank ();
				};

				Dictionary<string, string> dic = new Dictionary<string, string> ();

				foreach (string s in queryModel.questUserColumnName) {
					dic.Add (s, dataTable[0][s].ToString ());
				}

				httpComponent.Over (dic, false);
			} else {
				rankFacebookManager.Rank ();
			}
		} else {
			rankFacebookManager.Rank ();
		}
	}
}
