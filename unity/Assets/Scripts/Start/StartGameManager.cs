using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGameManager : MonoBehaviour
{
	// component
	private StartFacebookManager startFacebookManager;
	private HttpComponent httpComponent;
	private QueryModel queryModel;

	void Start ()
	{
		startFacebookManager = gameObject.GetComponent<StartFacebookManager> ();
		httpComponent = gameObject.GetComponent<HttpComponent> ();

		queryModel = QueryModel.Instance ();

		Score ();
	}

	private void Score ()
	{
		if (!FB.IsLoggedIn) {
			return;
		}

		int tempScore = int.Parse (SceneData.score);
		if (tempScore > 0) {
			httpComponent.OnDone = (object obj) => {
				Debug.Log ("StartGameManager Score OnDone");
				startFacebookManager.Rank ();
			};

			DataTable dataTable = queryModel.MypageStage (SceneData.stageLevel);
			if (dataTable.Rows.Count > 0) {
				Dictionary<string, string> dic = new Dictionary<string, string> ();

				foreach (string s in queryModel.questUserColumnName) {
					dic.Add (s, (string)dataTable[0][s]);
				}

				httpComponent.Over (dic, false);
			} else {
				startFacebookManager.Rank ();
			}
		} else {
			startFacebookManager.Rank ();
		}
	}
}
