using UnityEngine;
using System.Collections;

public class FireflyComponent : MonoBehaviour
{
	// component
	private UISprite uISprite;
	// variable
	public float alphaSpeed = 0.1f;
	public float scaleSpeed = 0.2f;
	private float calAlpha;
	private float calScale;
	private bool scaleFlag;
	private bool startFlag;

	void Start ()
	{
		uISprite = gameObject.GetComponent<UISprite> ();

		calAlpha = 0;
		calScale = 0;
		scaleFlag = true;
		startFlag = false;

		StartCoroutine (StartC ());
	}

	void Update ()
	{
		if (!startFlag) {
			return;
		}

		calAlpha += Time.deltaTime * alphaSpeed;
		if (calAlpha >= 1f) {
			calAlpha = 0;
		}

		if (scaleFlag) {
			calScale += Time.deltaTime * scaleSpeed;
		} else {
			calScale -= Time.deltaTime * scaleSpeed;
		}

		if (calScale >= 2f) {
			scaleFlag = false;
		} else if (calScale <= 1f) {
			scaleFlag = true;
		}

		uISprite.alpha = calAlpha;
		transform.localScale = new Vector3 (calScale, calScale, calScale);
	}

	private IEnumerator StartC ()
	{
		yield return new WaitForSeconds (Random.Range (0f, 2f));
		startFlag = true;
	}
}
