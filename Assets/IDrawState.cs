namespace LSystem
{
	using UnityEngine;

    public interface IDrawState
    {
        Vector3 Translation { get; set; } 

		Vector3 Rotation { get; set; }

		Vector3 Scale { get; set; }
    }
}