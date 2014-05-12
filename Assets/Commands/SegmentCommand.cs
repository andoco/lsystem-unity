namespace LSystem.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using UnityEngine;

	public class SegmentCommand : ILSysCommand
	{
		private int segments;

		public SegmentCommand()
		{
			this.Cost = 0.25f;
			this.SegmentAxis = Vector3.up;
		}
		
		public Vector3 SegmentAxis { get; set; }

		public float Cost { get; set; }

		public ISegmentDrawer Segment { get; set; }

		#region ILSysCommand implementation

		public string[] CommandConstants
		{
			get
			{
				return new [] { "F", "" };
			}
		}

		public void Run(ILSystem lSystem, IDrawContext drawCtx, int generation, string rule, ref GenerationState genState)
		{
			if (rule == "F" || rule == "")
			{
				genState.time -= this.Cost;
				this.segments++;
				
				//if (draw && genState.time >= 0)
				if (genState.time >= 0)
				{
					genState.length = Math.Min(genState.length, genState.length * genState.time);
					
					var state = drawCtx.CurrentState;
					var p1 = state.Translation;
					
					drawCtx.Translate(this.SegmentAxis * genState.length);
					
					var p2 = state.Translation;
					
					if (lSystem.Timed)
					{
						this.Segment.Segment(p1, p2, generation, genState.time, this.segments);
					}
					else
					{
						this.Segment.Segment(p1, p2, generation, -1, this.segments);
					}
				}
			}
		}
		
		#endregion
	}
}
