using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotUIManager : UIManager
{
	// const
	public const int MAX = 10;
	// array
	public GameObject[] panel;
	public int[] percent;
	private Dictionary<string, Transform[]> board;
	private Dictionary<string, bool[]> slotFlag;
	// gameobject
	public GameObject prefab;
	// component;
	private Register register;
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

		register = Register.Instance ();
	}

	void Update ()
	{
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
							if (count == index / 2) {
								// reward
								Debug.Log ("Reward 1");
							} else if (count == index) {
								// reward
								Debug.Log ("Reward 2");
							}

							count = 0;
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

	private IEnumerator SetFlag ()
	{
		yield return new WaitForSeconds (0);

		if (!slotStartFlag) {
			slotStartFlag = true;

			foreach (GameObject gObj in panel) {
				foreach (SlotControl sC in gObj.GetComponentsInChildren<SlotControl> ()) {
					sC.SlotStart ();
				}
			}
		} else {
			slotStartFlag = false;

			int index = -1;
			foreach (KeyValuePair<string, bool[]> kVP in slotFlag) {
				kVP.Value [0] = true;

				if (index > -1) {
					int random = Random.Range (0, 100) + 1;
					if (percent [index] >= random) {
						kVP.Value [1] = true;
					}
				}
				index++;

				yield return new WaitForSeconds (2f);
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

	public void SlotOnClick ()
	{
		if (register.GetSlot () == Date.Day ()) {
		} else {

		}

		StartCoroutine (SetFlag ());
	}
}
