namespace LSystem
{
	using System.Collections.Generic;

	using UnityEngine;

    public interface ISegmentDrawer
    {
		void DrawStart();

		void DrawEnd();

//		void DrawSegment(int generation, float time, int id, IDrawContext ctx, IDictionary<string, object> generationState);

        void Segment(Vector3 from, Vector3 to, int generation, float time, int id);
    }
}