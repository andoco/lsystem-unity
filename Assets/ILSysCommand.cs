namespace LSystem
{
    public interface ILSysCommand
    {
		string[] CommandConstants { get; }

        void Run(ILSystem lSystem, IDrawContext drawCtx, int generation, string rule, ref GenerationState genState);
    }
}