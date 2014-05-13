using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using LSystem;
using LSystem.Commands;
using Vectrosity;

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