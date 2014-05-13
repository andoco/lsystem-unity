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
	public float segmentLength = 0.5f;

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