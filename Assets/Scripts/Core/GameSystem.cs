namespace Core
{
    public abstract class GameSystem
    {
        public bool IsRun { get; private set; }

        protected GameSystem(SystemConfig inConfig = null)
        {
            IsRun = true;
        }

        public abstract void Start();

        public virtual void Resume() => IsRun = true;
        public virtual void Pause() => IsRun = false;
    }
}
