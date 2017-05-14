using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
	public Transform target;
	Collider col;
	//Rigidbody rb;
	bool doToggle = true;
	public Slider xslide;
	public Slider zslide;
	public bool canMove = true;
	bool follow = false;

	Vector3 screenPoint, offset;

	// Use this for initialization
	void Start ()
	{
		if(target == null)
		{
			target = gameObject.transform;
			//Debug.LogError("No target assigned for MoveObject on "+name);
		}

		if(xslide != null && zslide != null)
		{
			xslide.value = target.position.x;
			zslide.value = target.position.z;
		}

		col = target.GetComponent<Collider>();
		//rb = target.GetComponent<Rigidbody>();
		if(col == null)
			doToggle = false;
	}

	public void ShiftX(float xt)
	{
		if(canMove)
		{
			Vector3 pos = target.position;
			pos.x = xt;
			target.position = pos;
		}
	}

	public void ShiftZ(float zt)
	{
		if(canMove)
		{
			Vector3 pos = target.position;
			pos.z = zt;
			target.position = pos;
		}
	}

	public void FlipCollider()
	{
		if(doToggle)
			col.enabled = !col.enabled;
	}

	public void FollowMouse()
	{
		follow = !follow;
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void Update()
	{
		if(follow)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint)+offset;
			transform.position = curPosition;
		}
	}
}
