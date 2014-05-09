namespace LSystem
{
	using UnityEngine;

	public class DrawState : IDrawState
	{
		public DrawState()
		{
			this.Translation = Vector3.zero;
			this.Rotation = Vector3.right;
			this.Scale = new Vector3(1f, 1f, 1f);
		}

		public DrawState(IDrawState state)
		{
			this.Translation = state.Translation;
			this.Rotation = state.Rotation;
			this.Scale = state.Scale;
		}

		public Vector3 Translation { get; set; } 
		
		public Vector3 Rotation { get; set; }
		
		public Vector3 Scale { get; set; }
	}
}
