using UnityEngine;
using System.Collections;
using DATA;

public class MypageUIManager : UIManager
{
	// const
	private const string NEXT = "Next";
	private const string BACK = "Back";
	private const string LAST = "LastLabel";
	private const float MIN_SWIPTE_DISTANCE_PIXELS = 100f;
	//gameobject
	private GameObject upGameObject;
	private GameObject downGameObject;
	// component
	private MypageGameManager mypageGameManager;
	private LoveComponent loveComponent;
	private Vector2 touchStartPos;
	// array
	private UISprite[] uiSpriteList;
	// variable
	private bool touchStarted;
	private float minSwipeDistancePixels;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		base.Start ();

		touchStarted = false;
		minSwipeDistancePixels = MIN_SWIPTE_DISTANCE_PIXELS;

		GameObject gObj = GameObject.Find (Config.ROOT_MANAGER);
		if (gObj != null) {
			loveComponent = gObj.GetComponent<LoveComponent> ();
		}

		upGameObject = GameObject.Find (NEXT);
		downGameObject = GameObject.Find (BACK);
		mypageGameManager = gameObject.GetComponent<MypageGameManager> ();

		if (mypageGameManager.LastQuestStatus ()) {
			upGameObject.SetActive (false);
		}

		if (!mypageGameManager.NextQuestStatus ()) {
			upGameObject.SetActive (false);
		}

		if (!mypageGameManager.BackQuestStatus ()) {
			downGameObject.SetActive (false);
		}

		if (SceneData.nextStage != "") {
			StartCoroutine (NextGameStart ());
		}

		GameObject.Find (LAST).GetComponent<UILabel> ().text = SceneData.lastStage.ToString ();
	}

	void Update ()
	{
		if (Input.touchCount > 0) {
			var touch = Input.touches [0];
			
			switch (touch.phase) {
			case TouchPhase.Began:
				touchStarted = true;
				touchStartPos = touch.position;
				break;
			case TouchPhase.Ended:
				if (touchStarted) {
					TestForSwipeGesture (touch);
					touchStarted = false;
				}
				break;
			case TouchPhase.Canceled:
				touchStarted = false;
				break;
			case TouchPhase.Stationary:
				break;
			case TouchPhase.Moved:
				break;
			}
		}
	}

	private IEnumerator NextGameStart ()
	{
		yield return new WaitForSeconds (0.5f);
		string temp = SceneData.nextStage;
		SceneData.nextStage = "";
		GameStart (temp);
	}

	private void TestForSwipeGesture (Touch touch)
	{
		// test min distance
		
		var lastPos = touch.position;
		var distance = Vector2.Distance (lastPos, touchStartPos);
		
		if (distance > minSwipeDistancePixels) {
			float dy = lastPos.y - touchStartPos.y;
			float dx = lastPos.x - touchStartPos.x;
			
			float angle = Mathf.Rad2Deg * Mathf.Atan2 (dx, dy);
			
			angle = (360 + angle - 45) % 360;
			
			if (angle < 90) {
				// right
			} else if (angle < 180) {
				// down
				Next ();
			} else if (angle < 270) {
				// left
			} else {
				// up
				Back ();
			}
		}
	}

	private void SetUISprite (bool flag)
	{
		if (uiSpriteList == null) {
			uiSpriteList = gameObject.GetComponentsInChildren<UISprite> ();
		}

		foreach (UISprite uS in uiSpriteList) {
			uS.gameObject.SetActive (flag);
		}
	}

	protected override void PopupOnActive (SSController sSC)
	{
		base.PopupOnActive (sSC);

		mypageGameManager.SetEffect (!popupFlag);
	}

	protected override void PopupOnDeactive (SSController sSC)
	{
		base.PopupOnDeactive (sSC);

		mypageGameManager.SetEffect (!popupFlag);
	}

	public void Love ()
	{
		if (FB.IsLoggedIn) {
			if (loveComponent.NotFlag ()) {
				SSSceneManager.Instance.PopUp (Config.LOVE, null, PopupOnActive, PopupOnDeactive);
			}
		}
	}

	public void Setting ()
	{
		SceneData.tutorialStartScene = Config.MYPAGE;
		SSSceneManager.Instance.PopUp (Config.SETTING, null, PopupOnActive, PopupOnDeactive);
	}

	public void GameStart (string temp = null)
	{
		if (temp == null) {
			temp = UIButton.current.name.ToString ();
		}

		if (temp == MypageGameManager.DISABLE_QUEST) {
			return;
		}

		if (SceneData.nextStage != "") {
			return;
		}

		SceneData.score = mypageGameManager.Score (temp);
		SceneData.stageLevel = temp;

		SSSceneManager.Instance.PopUp (Config.START, null, PopupOnActive, PopupOnDeactive);
	}

	public void Next ()
	{
		if (popupFlag) {
			return;
		}
		mypageGameManager.NextQuest ();
	}

	public void Back ()
	{
		if (popupFlag) {
			return;
		}
		mypageGameManager.BackQuest ();
	}
}
