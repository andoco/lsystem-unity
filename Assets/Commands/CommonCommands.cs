namespace LSystem.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using UnityEngine;

	public class CommonCommands : ILSysCommand
	{
		public CommonCommands()
		{
			this.Angle = 20;
			this.AngleAxis = Vector3.forward;
			this.SegmentLength = 1;
			this.SegmentAxis = Vector3.up;
		}

		public float Angle { get; set; }
		
		public Vector3 AngleAxis { get; set; }
		
		public float SegmentLength { get; set; }
		
		public Vector3 SegmentAxis { get; set; }

		#region ILSysCommand implementation

		public string[] CommandConstants { 
			get
			{
				return new [] { 
					"f", 
					"-", 
					"+", 
					"|", 
					"[", 
					"]",
					"!",
					"(",
					")",
					"<",
					">"
				};
			} 
		}

		public void Run (ILSystem lSystem, IDrawContext drawCtx, int generation, string c, ref GenerationState genState)
		{
			// Standard command symbols:
			// f signifies a move,
			// + and - rotate either left or right, | rotates 180 degrees,
			// [ and ] are for push() and pop(), e.g. offshoot branches,
			// < and > decrease or increases the segment length,
			// ( and ) decrease or increases the rotation angle.
			if (c == "f")
				drawCtx.Translate(this.SegmentAxis * -Math.Min(genState.length, genState.length * genState.time));
			else if (c == "-")
				drawCtx.Rotate(this.AngleAxis, Math.Min(+genState.angle, +genState.angle * genState.time));
			else if (c == "+")
				drawCtx.Rotate(this.AngleAxis, Math.Max(-genState.angle, -genState.angle * genState.time));
			else if (c == "|")
				drawCtx.Rotate(this.AngleAxis, 180f);
			else if (c == "[")
				drawCtx.Push();
			else if (c == "]")
				drawCtx.Pop();

			// Non-drawing constants
			else if (c == "!")
				genState.angle -= genState.angle;
			else if (c == "(") 
				genState.angle *= 1.1f;
			else if (c == ")") 
				genState.angle *= 0.9f;
			else if (c == "<") 
				genState.length *= 0.9f;
			else if (c == ">") 
				genState.length *= 1.1f;
		}

		#endregion
	}
	
}
