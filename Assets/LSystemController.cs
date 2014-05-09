using UnityEngine;
using System.Collections.Generic;

using LSystem;
using Vectrosity;

public class LSystemController : MonoBehaviour {
	
	private float totalTime;
	private float time;
	private LSystem.LSystem lsys;
	
	public int generations = 6;

	// Use this for initialization
	void Start () {
		this.lsys = new LSystem.LSystem();
		lsys.Segment = new UnitySegmentDrawer();
		lsys.Rules = new Dictionary<string, string> { { "1", "FF-[1]++F+F" } };
//		lsys.Rules = new Dictionary<string, string> { { "1", "FF-[1]++F+F+1" } };

		this.totalTime = lsys.Duration(this.generations);
	}

	void Update()
	{
		if (this.time < this.totalTime)
		{
			lsys.Draw(Vector3.zero, this.generations, this.time, 0f);
			this.time += Time.deltaTime / 2f;
		}
	}
}

public class UnitySegmentDrawer : ISegmentDrawer
{
	private Dictionary<int, VectorLine> lines = new Dictionary<int, VectorLine>();

	#region ISegmentDrawer implementation

	public void Segment (Vector3 from, Vector3 to, int generation, float time, int id)
	{
		VectorLine line;
		Debug.Log(id);
		if (this.lines.TryGetValue(id, out line))
		{
			line.points3[0] = from;
			line.points3[1] = to;
		}
		else
		{
			line = VectorLine.SetLine3D(Color.white, from, to);
			this.lines[id] = line;
		}
	}

	#endregion
}