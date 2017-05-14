using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
	public Text name1, name2;
	public Text nodes1, nodes2;
	public Text path1, path2;

	// Use this for initialization
	void Start ()
	{
		nodes1.text = "";
		nodes2.text = "";
		path1.text = "";
		path2.text = "";
	}

	public void SetName(int i, string n)
	{
		if(i == 0)
		{
			name1.text = n;
		}
		else if(i == 1)
		{
			name2.text = n;
		}
	}
	
	public void SetText(int i, int n, int p)
	{
		if(i == 0)
		{
			nodes1.text = n.ToString();
			path1.text = p.ToString();
		}
		else if(i == 1)
		{
			nodes2.text = n.ToString();
			path2.text = p.ToString();
		}
	}
}
