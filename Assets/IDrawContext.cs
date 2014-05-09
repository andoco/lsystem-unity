namespace LSystem
{
	using UnityEngine;

    public interface IDrawContext
    {
        IDrawState CurrentState { get; }

        void Translate(Vector3 delta);

        void Rotate(Vector3 axis, float angle);

        void Scale(Vector3 factor);

        void Push();

        void Pop();
    }
}