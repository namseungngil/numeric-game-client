using UnityEngine;
using System.Collections;

public class SwipeCompoent : MonoBehaviour
{
	public delegate void SwapeDelegate (string way);
	public SwapeDelegate swape {
		set {
			swapeDelegate = value;
		}
	}
	private SwapeDelegate swapeDelegate;

	private Vector2 touchStartPos;
	private bool touchStarted;
	private float minSwipeDistancePixels = 100f;

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
				swapeDelegate ("right");
			} else if (angle < 180) {
				// down
				swapeDelegate ("down");
			} else if (angle < 270) {
				// left
				swapeDelegate ("left");
			} else {
				// up
				swapeDelegate ("up");
			}
		}
	}
}
