using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotUIManager : UIManager
{
	// const
	public const int MAX = 10;
	private const int REWARD3 = 5;
	private const int REWARD2 = 3;
	private const string X = "X";
	// array
	public GameObject[] panel;
	public int[] percent;
	private Dictionary<string, Transform[]> board;
	private Dictionary<string, bool[]> slotFlag;
	// gameobject
	public GameObject prefab;
	public GameObject effectPrefab;
	private GameObject reward;
	private GameObject button;
	private GameObject cancel;
	private GameObject effectUIWidget;
	// component;
	private Register register;
	private LoveComponent loveComponent;
	private UISprite info;
	private UILabel heart;
	private EffectCameraManager effectCameraManager;
	// variable
	public float slotSpeed = 10f;
	private string slotName;
	private string slotSecondName;
	private int count;
	private bool slotStartFlag;

	public override void Awake ()
	{
		BgmType = Bgm.SAME;
//		BgmName = string.Empty;
		
		IsCache = false;
	}

	public override void Start ()
	{
		List<string> list = new List<string> ();
		for (int i = 1; i <= MAX; i++) {
			list.Add (i.ToString ());
		}

		list = AtRandom.Randomize<string> (list);
		list = list.GetRange (0, 5);

		board = new Dictionary<string, Transform[]> ();
		foreach (GameObject gObj in panel) {
			list = AtRandom.Randomize<string> (list);

			List<UIWidget> uIWidget = new List<UIWidget> ();

			foreach (UIWidget uIW in gObj.GetComponentsInChildren<UIWidget> ()) {
				if (uIW.name != "Sprite") {
					uIWidget.Add (uIW);
				}
			}

			board.Add (gObj.name, new Transform[uIWidget.Count]);

			for (int i = 0; i < uIWidget.Count; i++) {
				board [gObj.name] [i] = uIWidget [i].transform;

				if (i >= uIWidget.Count - 1) {
					continue;
				}

				GameObject temp = Instantiate (prefab, uIWidget [i].transform.position, Quaternion.identity) as GameObject;
				
				temp.transform.parent = gObj.transform;
				temp.transform.localScale = new Vector3 (1, 1, 1);
				
				temp.name = uIWidget [i].name;
				temp.GetComponent<UISprite> ().spriteName = list [int.Parse (uIWidget [i].name)];
			}
		}

		slotFlag = new Dictionary<string, bool[]> ();
		for (int i = 0; i < panel.Length; i++) {
			slotFlag.Add (panel [i].name, new bool [] {false, false});
		}

		slotStartFlag = false;
		count = 0;

		GameObject.Find ("RewardInfo1").GetComponent<UILabel> ().text = X + REWARD3;
		GameObject.Find ("RewardInfo2").GetComponent<UILabel> ().text = X + REWARD2;

		register = Register.Instance ();

		loveComponent = GameObject.Find (Config.ROOT_MANAGER).GetComponent<LoveComponent> ();
		info = GameObject.Find ("Info").GetComponent<UISprite> ();
		heart = Logic.GetChildObject (info.gameObject, "Heart").GetComponent<UILabel> ();
		button = GameObject.Find (Config.ANIMATION_BUTTON);
		cancel = GameObject.Find ("Cancel");
		effectUIWidget = GameObject.Find ("EffectUIWidget");

		reward = GameObject.Find ("Reward");
		reward.SetActive (false);

		if (register.GetSlot () == Date.Day ()) {
			Logic.GetChildObject (info.gameObject, "Label").GetComponent<UILabel> ().text = "1";
		} else {
			Logic.GetChildObject (info.gameObject, "Label").GetComponent<UILabel> ().text = "FREE";
			heart.gameObject.SetActive (false);
		}

		effectCameraManager = GameObject.Find (Config.EFFECT_CAMERA).GetComponent<EffectCameraManager> ();

	}

	void Update ()
	{
		if (heart.gameObject.activeInHierarchy) {
			heart.text = loveComponent.GetLove ().ToString ();
		}

		if (!slotStartFlag) {
			int index = 0;
			foreach (KeyValuePair<string, bool[]> kVP in slotFlag) {
				if (kVP.Value [0]) {
					bool stopFlag = false;
					foreach (SlotControl sC in panel [index].GetComponentsInChildren<SlotControl> ()) {
						if (!kVP.Value [1]) {
							if (index == 0) {
								if (!stopFlag) {
									stopFlag = true;
									break;
								}
							} else {
								if (!stopFlag) {
									if (sC.name == "1") {
										if (slotName != sC.GetSpriteName ()) {
											if (index == 1) {
												slotSecondName = sC.GetSpriteName ();
												stopFlag = true;
											} else {
												if (slotSecondName != sC.GetSpriteName ()) {
													stopFlag = true;
												}
											}
											break;
										}
									}
								}
							}
						} else {
							if (!stopFlag) {
								if (sC.name == "1") {
									if (slotName == sC.GetSpriteName ()) {
										count++;
										stopFlag = true;
										break;
									}
								}
							}
						}
					}

					if (stopFlag) {
						foreach (SlotControl sc in panel [index].GetComponentsInChildren<SlotControl> ()) {
							sc.SlotStop ();
						}

						slotFlag [panel [index].name] [0] = false;

						if (index >= 2) {
							int rewardCount = 0;
							float time = 2f;
							if (count == index / 2) {
								// reward
								rewardCount = REWARD2;
							} else if (count == index) {
								// reward
								rewardCount = REWARD3;
							} else {
								time = 0.5f;
							}

							count = 0;

							if (rewardCount > 0) {
								loveComponent.Add (rewardCount);

								// animation
								reward.SetActive (true);
								reward.GetComponentInChildren<UILabel> ().text = rewardCount.ToString ();
								reward.GetComponent<Animation> ().Play (Config.ANIMATION_BUTTON);

								// effect
								effectCameraManager.GUIOnOverEffect (effectPrefab, effectUIWidget.gameObject);
							}

							StartCoroutine (AfterRewardOver (time));
						}
					}
				}

				index ++;
			}

			foreach (SlotControl sC in panel[0].GetComponentsInChildren<SlotControl> ()) {
				if (sC.name == "2") {
					slotName = sC.GetSpriteName ();
				}
			}
		}
	}

	private IEnumerator AfterRewardOver (float time)
	{
		yield return new WaitForSeconds (time);

		reward.SetActive (false);

		cancel.SetActive (true);
		button.SetActive (true);
		info.gameObject.SetActive (true);
		heart.gameObject.SetActive (true);

		info.GetComponentInChildren<UILabel> ().text = "1";
	}

	private IEnumerator SetFlag (float time = 0)
	{
		yield return new WaitForSeconds (time);

		if (!slotStartFlag) {
			slotStartFlag = true;

			foreach (GameObject gObj in panel) {
				foreach (SlotControl sC in gObj.GetComponentsInChildren<SlotControl> ()) {
					sC.SlotStart ();
				}
			}

			StartCoroutine (SetFlag (1.5f));
		} else {
			slotStartFlag = false;

			int index = -1;
			foreach (KeyValuePair<string, bool[]> kVP in slotFlag) {
				kVP.Value [0] = true;

				if (index > -1) {
					int random = Random.Range (0, 100) + 1;
					if (percent [index] >= random) {
						kVP.Value [1] = true;
					} else {
						kVP.Value [1] = false;
					}
				}
				index++;

				yield return new WaitForSeconds (1f);
			}
		}
	}

	public Transform Board (string panel, string n)
	{
		foreach (Transform trans in board [panel]) {
			if (n == trans.name) {
				return trans;
			}
		}

		return null;
	}

	protected void PopupOnActive (SSController s)
	{
		effectCameraManager.SetActive (false);
	}
	
	protected void PopupOnDeActive (SSController s)
	{
		effectCameraManager.SetActive (true);
	}

	public void SlotOnClick ()
	{
		if (slotStartFlag) {
			return;
		}

		if (register.GetSlot () == Date.Day ()) {
			if (!loveComponent.UseLove ()) {
				Cancel ();
				SSSceneManager.Instance.PopUp (Config.NOT, null, PopupOnActive, PopupOnDeActive);

				return;
			}
		} else {
			register.SetSlot ();
		}

		button.SetActive (false);
		cancel.SetActive (false);
		info.gameObject.SetActive (false);

		StartCoroutine (SetFlag ());
	}
}
