namespace LSystem
{
	using UnityEngine;

    public interface ISegmentDrawer
    {
		void DrawStart();

		void DrawEnd();

        void Segment(Vector3 from, Vector3 to, int generation, float time, int id);
    }
}