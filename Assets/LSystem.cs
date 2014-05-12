namespace LSystem
{
	using System;
	using System.Collections.Generic;

	using UnityEngine;

    public class LSystem : ILSystem
    {
        private const float LSYS_DURATION_MAX = 100000f;

        private int segments;

        private float duration;

        private bool timed;

        public float Angle { get; set; }

		public Vector3 AngleAxis { get; set; }

        public float SegmentLength { get; set; }

		public Vector3 SegmentAxis { get; set; }

        public float Threshold { get; set; }

        public IDictionary<string, string> Rules { get; set; }

        public IDictionary<string, ILSysCommand> Commands { get; set; }

        public float Decrease { get; set; }

        public float Cost { get; set; }

        public string Root { get; set; }

        public IDrawContext Ctx { get; set; }

        public ISegmentDrawer Segment { get; set; }

        public LSystem()
        {
            this.Angle = 20;
			this.AngleAxis = Vector3.forward;
            this.SegmentLength = 1;
			this.SegmentAxis = Vector3.up;
            this.Decrease = 0.7f;
//            this.Threshold = 3.0f;
			this.Threshold = 0.01f;
            this.Cost = 0.25f;
            this.Root = "1";

            this.Rules = new Dictionary<string, string> { { "1", "FF-1" } };
			this.Commands = new Dictionary<string, ILSysCommand>();
			this.Ctx = new DrawContext();
        }

        public void Reset()
        {
            this.segments = 0;
            this.duration = 0;
        }
		
        public void DrawGeneration(int generation, string rule, float angle, float length, float time, bool draw)
        {
            if (generation == 0)
            {
                this.duration = 1 + (LSYS_DURATION_MAX - time);
            }

            if (length <= this.Threshold)
            {
                this.duration = 1 + (LSYS_DURATION_MAX - time);
            }

            // Custom command symbols:
            // If the rule is a key in the LSsytem.commands dictionary,
            // execute its value which is a function taking 6 parameters:
            // lsystem, generation, rule, angle, length and time.
            ILSysCommand cmd;
            if (this.Commands.TryGetValue(rule, out cmd))
            {
                cmd.Run(this, generation, rule, angle, length, time);
            }

			if (!this.ExpandRule(rule, out rule))
				return;

        	for (var i = 0; i < rule.Length; i++)
        	{
				var c = Convert.ToString(rule[i]);

				if (draw)
				{
					// Standard command symbols:
					// f signifies a move,
					// + and - rotate either left or right, | rotates 180 degrees,
					// [ and ] are for push() and pop(), e.g. offshoot branches,
					// < and > decrease or increases the segment length,
					// ( and ) decrease or increases the rotation angle.
					if (c == "f")
						this.Ctx.Translate(this.SegmentAxis * -Math.Min(length, length*time)); //this.Ctx.Translate(new Vector3(0f, -Math.Min(length, length*time), 0f));
					else if (c == "-")
						this.Ctx.Rotate(this.AngleAxis, Math.Min(+angle, +angle*time));
					else if (c == "+")
						this.Ctx.Rotate(this.AngleAxis, Math.Max(-angle, -angle*time));
					else if (c == "|")
						this.Ctx.Rotate(this.AngleAxis, 180f);
					else if (c == "[")
						this.Ctx.Push();
					else if (c == "]")
						this.Ctx.Pop();
				}

	            if (c == "F") 
	                time -= this.Cost;
	            else if (c == "!")
	                angle -= angle;
	            else if (c == "(") 
	                angle *= 1.1f;
	            else if (c == ")") 
	                angle *= 0.9f;
	            else if (c == "<") 
	                length *= 0.9f;
	            else if (c == ">") 
	                length *= 1.1f;

				if (c == "F" || c == "")
				{
	                this.segments++;
	
	                if (draw && time >= 0)
	                {
	                    length = Math.Min(length, length * time);
	
	                    var state = this.Ctx.CurrentState;
	                    var p1 = state.Translation;
	
						this.Ctx.Translate(this.SegmentAxis * length);
	
	                    var p2 = state.Translation;
	
	                    if (timed)
	                    {
	                        this.Segment.Segment(p1, p2, generation, time, this.segments);
	                    }
	                    else
	                    {
	                        this.Segment.Segment(p1, p2, generation, -1, this.segments);
	                    }
	                }
				}

				if (generation > 0 && time > 0)
				{
					this.DrawGeneration(generation - 1, c, angle, length * this.Decrease, time, draw);
				}
        	}
        }

        public float Duration(int generation)
        {
            this.Ctx.Push();
            this.Reset();
            this.DrawGeneration(generation, this.Root, this.Angle, this.SegmentLength, LSYS_DURATION_MAX, false);
            this.Ctx.Pop();

            return this.duration;
        }

        public void Draw(Vector3 pos, int generation)
        {
            this.Draw(pos, generation, -1, -1);
        }

        public void Draw(Vector3 pos, int generation, float time, float ease)
        {
            var angleToUse = this.Angle;
            if (time != -1 && ease != -1)
                angleToUse = Math.Min(this.Angle, this.Angle * time / ease);
        
            this.timed = true;
            if (time == -1)
            {
                this.timed = false;
                time = LSYS_DURATION_MAX;
            }

            this.Ctx.Push();
            this.Ctx.Translate(pos);
            this.Reset();
            this.DrawGeneration(generation, this.Root, angleToUse, this.SegmentLength, time, true);
            this.Ctx.Pop();
        }

		#region Private methods

		private bool ExpandRule(string rule, out string expanded)
		{
			if (this.Rules.TryGetValue(rule, out expanded))
			{
				return true;
			}
			
			expanded = rule;
			
			return false;
		}

		#endregion
    }
}
