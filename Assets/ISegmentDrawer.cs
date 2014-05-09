namespace LSystem
{
	using UnityEngine;

    public interface ISegmentDrawer
    {
        void Segment(Vector3 from, Vector3 to, int generation, float time, int id);
    }
}