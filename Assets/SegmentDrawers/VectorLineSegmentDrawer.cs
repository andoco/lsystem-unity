using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using LSystem;
using LSystem.Commands;
using Vectrosity;

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
