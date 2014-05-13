using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using LSystem;
using LSystem.Commands;
using Vectrosity;

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
		foreach (var item in this.segments.Where(x => !this.visited.Contains(x.Key)).ToArray())
		{
			GameObject.Destroy(item.Value.gameObject);
			this.segments.Remove(item.Key);
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

		this.visited.Add(id);
	}
	
	#endregion
}
