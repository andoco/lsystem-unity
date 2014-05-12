namespace LSystem
{
    public interface ILSysCommand
    {
		string[] CommandConstants { get; }

        void Run(ILSystem lSystem, IDrawContext drawCtx, int generation, string rule, float angle, float length, float time);
    }
}