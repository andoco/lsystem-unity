namespace LSystem
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using UnityEngine;

    public class LSystem : ILSystem
    {
        private const float LSYS_DURATION_MAX = 100000f;

        private float duration;

		public bool Timed { get; private set; }

        public float Angle { get; set; }

        public float SegmentLength { get; set; }

        public float Threshold { get; set; }

        public IDictionary<string, string> Rules { get; set; }

        public IDictionary<string, ILSysCommand> Commands { get; set; }

        public float Decrease { get; set; }

        public string Root { get; set; }

        public IDrawContext Ctx { get; set; }

        public LSystem()
        {
            this.Decrease = 0.7f;
			this.Threshold = 0.1f;
            this.Root = "1";

            this.Rules = new Dictionary<string, string> { { "1", "FF-1" } };
			this.Commands = new Dictionary<string, ILSysCommand>();
			this.Ctx = new DrawContext();
        }

		public void AddCommand(ILSysCommand cmd)
		{
			foreach (var c in cmd.CommandConstants)
			{
				this.Commands[c] = cmd;
			}

			Debug.Log(string.Format("Commands = {0}", string.Join(",", this.Commands.Keys.ToArray())));
		}

        public void Reset()
        {
			// TODO: reset commands
//            this.segments = 0;
            this.duration = 0;
        }
		
        public void DrawGeneration(int generation, string rule, GenerationState genState, bool draw)
        {
            if (generation == 0)
            {
                this.duration = 1 + (LSYS_DURATION_MAX - genState.time);
            }

			if (genState.length <= this.Threshold)
            {
				this.duration = 1 + (LSYS_DURATION_MAX - genState.time);
            }

			if (!this.ExpandRule(rule, out rule))
				return;

        	for (var i = 0; i < rule.Length; i++)
        	{
				var c = Convert.ToString(rule[i]);

				ILSysCommand cmd;
				if (this.Commands.TryGetValue(c, out cmd))
				{
//					Debug.Log(string.Format("running cmd {0}", rule));
					cmd.Run(this, this.Ctx, generation, c, ref genState);
				}

				if (generation > 0 && genState.time > 0)
				{
					this.DrawGeneration(generation - 1, c, new GenerationState { angle = genState.angle, length = genState.length * this.Decrease, time = genState.time }, draw);
				}
        	}
        }

        public float Duration(int generation)
        {
            this.Ctx.Push();
            this.Reset();
			this.DrawGeneration(generation, this.Root, new GenerationState { angle = this.Angle, length = this.SegmentLength, time = LSYS_DURATION_MAX }, false);
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
        
            this.Timed = true;
            if (time == -1)
            {
                this.Timed = false;
                time = LSYS_DURATION_MAX;
            }

            this.Ctx.Push();
            this.Ctx.Translate(pos);
            this.Reset();
			this.DrawGeneration(generation, this.Root, new GenerationState { angle = angleToUse, length = this.SegmentLength, time = time }, true);
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
