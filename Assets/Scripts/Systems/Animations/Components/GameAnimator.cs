using Core;
using Interfaces;

namespace Systems.Animations.Components
{
    public struct GameAnimator : IComponent
    {
        public string Id { get; }
        public event IComponentEvents onDestroyed;
        public GameView View { get; }
        public void Attach(GameView inView)
        {
            
        }

        public void Destroy()
        {
           // Main.GetSystem<AnimationSystem>().DetachComponent(GetType(), this);
        }
    }
}