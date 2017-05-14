using UnityEngine;
using System.Collections;

public class VisiNode : MonoBehaviour
{

	public Color highlight;
	MeshRenderer renderer;

	bool canSelect = true;//lol reused code->bad names

	// Use this for initialization
	void Start ()
	{
		//renderer = GetComponent<MeshRenderer>();
	}

	public void ToggleHighlight()
	{
		//Debug.Log("Toggling Highlight");
		renderer = GetComponent<MeshRenderer>();
		renderer.material.EnableKeyword("_EMISSION");
		canSelect = true;
		renderer.material.SetColor("_EmissionColor", highlight);
	}
}
