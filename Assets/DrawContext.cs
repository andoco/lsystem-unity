namespace LSystem
{
	using System.Collections.Generic;

	using UnityEngine;

	public class DrawContext : IDrawContext
	{
		private readonly Stack<IDrawState> states = new Stack<IDrawState>();

		public DrawContext()
		{
			this.states.Push(new DrawState());
		}

		public IDrawState CurrentState { get { return this.states.Peek(); } }

		public void Translate(Vector3 delta)
		{
			var state = this.CurrentState;

//			Debug.Log(string.Format("Delta before = {0}", delta));
			var rot = Quaternion.Euler(state.Rotation);
			delta = rot * delta;
//			Debug.Log(string.Format("Delta after = {0}", delta));

			state.Translation += delta;
		}

		public void Rotate(Vector3 axis, float angle)
		{
//			var oldRot = this.CurrentState.Rotation;
			var state = this.CurrentState;
			var rot = Quaternion.Euler(state.Rotation) * Quaternion.AngleAxis(angle, axis);
			state.Rotation = rot.eulerAngles;

//			Debug.Log(string.Format("ROTATE (axis={0}, a={1}) from {2} to {3}", axis, angle, oldRot, this.CurrentState.Rotation));
		}

		public void Scale(Vector3 factor)
		{
			this.CurrentState.Scale += factor;
		}

		public void Push()
		{
			this.states.Push(new DrawState(this.CurrentState));
		}

		public void Pop()
		{
			this.states.Pop();
		}
	}
}