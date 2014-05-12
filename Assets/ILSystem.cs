namespace LSystem
{
	using UnityEngine;

	public struct GenerationState
	{
		public float angle;
		public float length;
		public float time;
	}

    public interface ILSystem
    {
		void AddCommand(ILSysCommand cmd);

        void Reset();

        void DrawGeneration(int generation, string rule, GenerationState genState, bool draw);

        void Draw(Vector3 pos, int generation);

        void Draw(Vector3 pos, int generation, float time, float ease);
    }
}