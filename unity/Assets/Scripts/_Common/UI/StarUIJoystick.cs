using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/Interaction/StarJoyStick")]
public class StarUIJoystick : MonoBehaviour
{
	Transform target;
	public Vector3 scale = Vector3.one;
	public Vector2 range = new Vector2 (100f, 100f);
	public float deadZone = 4f;
	public bool circularPadConstraint = false;
	public float springBackSpeed = 20f;
	public Vector2 padPosition;
	public float padAngle;
	public Vector3 padPositionAndAngle;
	Vector3 mStartPos;
	Vector3 mStartLocalPos;
	Plane mPlane;
	Vector3 mLastPos;
	Vector3 totalOffset;
	Vector3 startOffset;
	Vector3 totalWorldOffset;
	bool mDragStarted;
	Vector2 mDragStartOffset;
	bool started;

	void Update ()
	{
		if (started) {
			StartOnDrag ();
		}
	}

	static bool Vector3AlmostEquals (Vector3 target, Vector3 second, float sqrMagniturePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagniturePrecision;
	}
	
	void LateUpdate ()
	{
		if (!started) {
			return;
		}
		
		Vector3 pos = target.transform.localPosition;
		
		if (!circularPadConstraint) {
			pos.x = Mathf.Clamp (pos.x, mStartLocalPos.x - range.x, mStartLocalPos.x + range.x);
			pos.y = Mathf.Clamp (pos.y, mStartLocalPos.y - range.y, mStartLocalPos.y + range.y);
		} else {
			pos = Vector3.ClampMagnitude (pos, range.x);
		}
		
		target.transform.localPosition = pos;
		
		// feed the feedback values.
		Vector3 offset = pos - mStartLocalPos;
		//deadzone
		if (offset.magnitude <= deadZone) {
			padPosition = Vector2.zero;
			padAngle = 0f;
			padPositionAndAngle = Vector3.zero;
		} else {
			
			// get the pad input values;
			padPosition.x = offset.x / (range.x);
			padPosition.y = offset.y / (range.y);
			
			padAngle = Mathf.Atan2 (padPosition.x, padPosition.y) * 180.0f / 3.14159f;

			padPositionAndAngle.x = padPosition.x;
			padPositionAndAngle.y = padPosition.y;
			padPositionAndAngle.z = padAngle;
		}
	}

	private void StartOnDrag ()
	{
		if (enabled && NGUITools.GetActive (gameObject)) {

			if (!mDragStarted) {
				mDragStarted = true;
				mLastPos = UICamera.lastHit.point;
			}
			
			Ray ray = UICamera.currentCamera.ScreenPointToRay (Input.mousePosition);
			float dist = 0f;
			
			if (mPlane.Raycast (ray, out dist)) {
				Vector3 currentPos = ray.GetPoint (dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;
				
				if (offset.x != 0f || offset.y != 0f) {
					offset = target.InverseTransformDirection (offset);
					offset.Scale (scale);
					offset = target.TransformDirection (offset);
				}
				
				totalOffset += offset;
				target.position = mStartPos + totalOffset;
			}
		}
	}

	public void StartOnPress (Transform T)
	{
		if (enabled && NGUITools.GetActive (gameObject)) {
			started = true;
			target = transform;
			mStartPos = T.position;
			mStartLocalPos = T.localPosition;
			mDragStarted = false;
			totalOffset = Vector3.zero;				

			target.position = mStartPos;

			// Create the plane to drag along
			Transform trans = UICamera.currentCamera.transform;
			mPlane = new Plane (trans.rotation * Vector3.back, mLastPos);
		}
	}

	public void EndOnPress ()
	{
		started = false;
	}
}