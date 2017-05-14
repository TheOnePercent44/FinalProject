using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Collider))]
public class Selectable : MonoBehaviour
{
	MoveObject mover;
	//public variables exposed in editor
	public Color highlight;

	//private variables kept to ourself
	//Usable myUse;
	MeshRenderer renderer;
	bool canSelect = false;
	//Selectable[] parents, children;

	// Use this for initialization
	void Start ()
	{
		mover = GetComponent<MoveObject>();
		//myUse = GetComponent<Usable>();
		renderer = GetComponent<MeshRenderer>();
		renderer.material.EnableKeyword("_EMISSION");

		//parents = GetComponentsInParent<Selectable>();
		//children = GetComponentsInChildren<Selectable>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(canSelect && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0)))
		{
			Select();
		}
	}

	void OnMouseEnter()
	{
		ToggleHighlight();
		//FlipRelations();
	}

	void OnMouseExit()
	{
		ToggleHighlight();
		//FlipRelations();
	}

	/*void FlipRelations()
	{
		foreach(Selectable s in parents)
		{
			s.ToggleHighlight();
		}
		foreach(Selectable s in children)
		{
			s.ToggleHighlight();
		}
	}*/

	public void Select()
	{
		mover.FollowMouse();
		//myUse.Use();
		//Add RelationUse?
		//Do other things? Like briefly change colors?
	}

	void ToggleHighlight()
	{
		if(canSelect)
		{
			canSelect = false;
			renderer.material.SetColor("_EmissionColor", Color.black);
		}
		else
		{
			canSelect = true;
			renderer.material.SetColor("_EmissionColor", highlight);
		}
	}
}
