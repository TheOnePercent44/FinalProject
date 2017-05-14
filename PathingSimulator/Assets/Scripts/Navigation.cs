using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Navigation : MonoBehaviour
{
	public Transform target;
	public VisiNode visiNode;
	public int id = 0;

	TextScript ui;

	MeshRenderer floor;
	float widthx = 10f;
	float widthz = 10f;

	Vector3 origin;
	Vector3 goal;
	NavMeshAgent agent;
	List<Node> nodes;
	Stack<Node> path;
	Node current;
	MoveObject mover;
	float posy = 0.5f;//just for ease of exchange

	bool start = false;
	bool build = false;

	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		mover = GetComponent<MoveObject>();
		floor = GameObject.Find("Floor").GetComponent<MeshRenderer>();
		ui = GameObject.Find("InfoPanel").GetComponent<TextScript>();

		ui.SetName(id, name);

		widthx = floor.bounds.size.x;
		widthz = floor.bounds.size.z;
		Debug.Log("Widthx: "+widthx+"; Widthz: "+widthz);

		origin = transform.position;

		nodes = new List<Node>(10);
		path = new Stack<Node>(10);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!start)
		{
			//do nothing
			Debug.Log("Not ready to start");
		}
		else if((transform.position - goal).magnitude < 0.4f)
			EndSim();
		else if((transform.position - current.position).magnitude < 0.4f)
			GetNext();
		else
		{
			//Debug.Log("Continue Navigating to: "+current.position.ToString());
		}
	}

	public void Reset()
	{
		agent.Stop();
		mover.FlipCollider();
		transform.position = origin;

		foreach(Node n in nodes)
		{
			VisiNode v = n.visual;
			Destroy(v.gameObject);
		}

		nodes.Clear();
		path.Clear();
		//mover.FlipCollider();
		//mover.canMove = true;
		StartCoroutine(RestoreMovability());
	}

	IEnumerator RestoreMovability()
	{
		yield return new WaitForSeconds(0.1f);
		mover.FlipCollider();
		mover.canMove = true;
	}

	public void BuildAndStart()
	{
		mover.canMove = false;
		goal = target.position;
		current = new Node(transform.position);
		current.visual = GameObject.Instantiate(visiNode) as VisiNode;
		current.visual.gameObject.transform.position = current.position;
		nodes.Add(current);
		StartCoroutine(BuildTree());
	}

	IEnumerator BuildTree()
	{
		//build = true;
		float posx;
		float posz;
		Vector3 temp;

		Node nearest;

		while(true)
		{
			Debug.Log("Testing Random Point");
			RaycastHit hitInfo;

			if(CheckBuildEnd())
				break;

			posx = Random.Range(-widthx/2f, widthx/2f);
			posz = Random.Range(-widthz/2f, widthz/2f);
			temp = new Vector3(posx, posy, posz);

			if((nearest = GetNearest(temp)) != null)
			{
				Vector3 dir = temp-nearest.position;
				if(!Physics.Raycast(nearest.position, dir.normalized, out hitInfo, dir.magnitude))
				{
					Node newguy = new Node(temp);
					newguy.visual = GameObject.Instantiate(visiNode) as VisiNode;
					newguy.visual.gameObject.transform.position = newguy.position;
					nodes.Add(newguy);
					newguy.parent = nearest;

				}
			}
		}

		//build = false;
		BuildPath();
		Debug.Log("Starting");
		//current = nodes[0];
		//agent.destination = current.position;
		Debug.Log("# Nodes: "+nodes.Count);
		ui.SetText(id, nodes.Count, path.Count);
		start = true;
		agent.Resume();
		yield return new WaitForSeconds(0.0001f);
	}

	void BuildPath()
	{
		while(current.parent != null)
		{
			path.Push(current);
			if(current.visual != null)
			{
				//Debug.Log("Flipping color");
				current.visual.ToggleHighlight();
			}
			current = current.parent;
		}
		if(current.visual != null)
		{
			//Debug.Log("Flipping color");
			current.visual.ToggleHighlight();
		}
	}

	bool CheckBuildEnd()
	{
		RaycastHit hitInfo;
		foreach(Node n in nodes)
		{
			if(!Physics.Raycast(n.position, goal-n.position, out hitInfo, (goal-n.position).magnitude))
			{
				Node end = new Node(goal);
				end.visual = GameObject.Instantiate(visiNode) as VisiNode;
				end.visual.gameObject.transform.position = end.position;
				nodes.Add(end);
				//n.AddNeighbor(end);
				//end.AddNeighbor(n);//maybe not do this?
				end.parent = n;
				current = end;

				Debug.Log("Ready to Start");
				return true;
			}
		}

		//if()

		return false;
	}

	void GetNext()
	{
		current = path.Pop();
		agent.destination = current.position;
	}

	Node GetNearest(Vector3 pos)
	{
		Node nearest = null;
		float dist = float.MaxValue;
		foreach(Node n in nodes)
		{
			Vector3 dir = pos-n.position;
			if(dir.magnitude < dist && !Physics.Raycast(pos, dir, dir.magnitude))
			{
				nearest = n;
				dist = dir.magnitude;
			}
		}

		return nearest;
	}

	void EndSim()
	{
		Debug.LogWarning("Ending Sim");
		start = false;
		agent.Stop();
	}

	class Node
	{
		public Vector3 position;
		public List<Node> neighbors;
		public Node parent;
		public VisiNode visual;

		public Node(Vector3 pos)//consider only adding neighbors in a forward direction
		{
			parent = null;
			position = pos;
			neighbors = new List<Node>(3);
		}

		void CheckEdges()
		{
			for(float rot = 0; rot < 360; rot += 22.5f)
			{
				Vector3 dir = Quaternion.Euler(0, rot, 0)*Vector3.right;
				if(Physics.Raycast(position, dir, dir.magnitude))
				{
					Debug.Log("Adjusting Position");
					position = position-dir;
				}
			}
		}

		public void AddNeighbor(Node n)
		{
			neighbors.Add(n);
		}

		public Node GetNeighbor(int i)
		{
			return neighbors[i];
		}
	}
}
