using System;

namespace CRUDConsoleApp
{
    public abstract class AppCommand : IAppCommand
    {
        public abstract string Name { get; }
        public abstract string Usage { get; }
        public abstract string Description { get; }

        protected string[] _arguments;

        public virtual void Initialize(string[] arguments)
        {
            _arguments = arguments;
        }

        public abstract void Execute();
    }
}
