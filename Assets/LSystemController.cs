using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using LSystem;
using LSystem.Commands;
using Vectrosity;

public class LSystemController : MonoBehaviour {
	
	private float totalTime;
	private float time;
	private LSystem.LSystem lsys;
	private ISegmentDrawer segmentDrawer;
	
	public int generations = 6;
	public Vector3 angleAxis = Vector3.forward;
	public float angle = 20f;
	public Vector3 segmentAxis = Vector3.up;
	public float segmentLength = 1;

	// Use this for initialization
	void Start () {
		this.lsys = new LSystem.LSystem();

		lsys.Rules = new Dictionary<string, string> { { "1", "FF-[1]++F+F" } };
//		lsys.Rules = new Dictionary<string, string> { { "1", "FF-[1]++F+F+1" } };
//		lsys.Rules = new Dictionary<string, string> { { "1", "F+F-F([1]" } };

//		lsys.Rules = new Dictionary<string, string> {
//			{ "1", "FFF+[2]F+(>[---1]" },
//			{ "2", "FFF[1]+[1]+[1]+[1]" }
//		};

		lsys.Angle = this.angle;
		lsys.SegmentLength = this.segmentLength;

		var cmd = new CommonCommands();
		cmd.Angle = this.angle;
		cmd.AngleAxis = this.angleAxis;
		cmd.SegmentAxis = this.segmentAxis;
		cmd.SegmentLength = this.segmentLength;
		lsys.AddCommand(cmd);

		this.segmentDrawer = new VectorLineSegmentDrawer();
		var segCmd = new SegmentCommand();
		segCmd.Segment = this.segmentDrawer;
		segCmd.SegmentAxis = this.segmentAxis;

		lsys.AddCommand(segCmd);

		this.totalTime = lsys.Duration(this.generations);
		Debug.Log(string.Format("totalTime = {0}", this.totalTime));

//		lsys.Draw(Vector3.zero, this.generations);
	}

	void Update()
	{
		if (this.time < this.totalTime)
		{
			this.segmentDrawer.DrawStart();
			lsys.Draw(Vector3.zero, this.generations, this.time, 3f);
			this.time += Time.deltaTime / 2f;
			this.segmentDrawer.DrawEnd();
		}
	}
}

public class VectorLineSegmentDrawer : ISegmentDrawer
{
	private Dictionary<int, VectorLine> lines = new Dictionary<int, VectorLine>();
	private HashSet<int> visited = new HashSet<int>();

	#region ISegmentDrawer implementation

	public void DrawStart()
	{
		this.visited.Clear();
	}

	public void DrawEnd()
	{
		foreach (var item in this.lines.Where(l => !this.visited.Contains(l.Key)).ToArray())
		{
			var line = item.Value;
			VectorLine.Destroy(ref line);
			this.lines.Remove(item.Key);
		}
	}
	
	public void Segment (Vector3 from, Vector3 to, int generation, float time, int id)
	{
		VectorLine line;

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

		this.visited.Add(id);
	}

	#endregion
}

public class CylinderSegmentDrawer : ISegmentDrawer
{
	private Dictionary<int, Transform> segments = new Dictionary<int, Transform>();
	private HashSet<int> visited = new HashSet<int>();

	#region ISegmentDrawer implementation

	public void DrawStart()
	{
		this.visited.Clear();
	}

	public void DrawEnd()
	{
		foreach (var item in this.segments)
		{
			if (!this.visited.Contains(item.Key))
			{
				item.Value.renderer.enabled = false;
			}
		}
	}
	
	public void Segment(Vector3 from, Vector3 to, int generation, float time, int id)
	{
		Transform seg;
		if (!this.segments.TryGetValue(id, out seg))
		{
			seg = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
			this.segments[id] = seg;
		}

		seg.localScale = (new Vector3(0.25f, 1f, 0.25f)) * (Vector3.Distance(from, to) * 0.5f);
		seg.position = Vector3.Lerp(from, to, 0.5f);
		seg.localRotation = Quaternion.FromToRotation(Vector3.up, to - from);
		seg.renderer.enabled = true;

		this.visited.Add(id);
	}
	
	#endregion
}

public class MeshExtrudeSegmentDrawer : ISegmentDrawer
{
	#region ISegmentDrawer implementation
	
	public void DrawStart()
	{
	}
	
	public void DrawEnd()
	{
	}
	
	public void Segment(Vector3 from, Vector3 to, int generation, float time, int id)
	{
		// Scale vertices based on Ctx.Scale

		// Extrude vertices from -> to
	}
	
	#endregion
}