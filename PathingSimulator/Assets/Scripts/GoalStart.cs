using UnityEngine;
using System.Collections;

public class GoalStart : MonoBehaviour
{
	Collider col;

	// Use this for initialization
	void Start ()
	{
		col = GetComponent<Collider>();
	}

	public void SwapCollider()
	{
		col.enabled = !col.enabled;
	}
}
