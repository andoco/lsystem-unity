namespace LSystem
{
    public interface ILSysCommand
    {
        void Run(ILSystem lSystem, int generation, string rule, float angle, float length, float time);
    }
}