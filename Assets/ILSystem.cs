namespace LSystem
{
	using UnityEngine;

    public interface ILSystem
    {
        void Reset();

        void DrawGeneration(int generation, string rule, float angle, float length, float time, bool draw);

        void Draw(Vector3 pos, int generation);

        void Draw(Vector3 pos, int generation, float time, float ease);
    }
}