namespace CRUDConsoleApp
{
    public interface IAppCommand
    {
        string Name { get; }
        string Usage { get; }
        string Description { get; }

        void Initialize(string[] arguments);
        void Execute();
    }
}
