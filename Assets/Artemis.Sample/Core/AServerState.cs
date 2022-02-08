namespace Artemis.Sample.Core
{
    public abstract class AServerState
    {
        public virtual void OnStateEntered(Server server)
        {
        }

        public virtual void OnGUI(Server server)
        {
        }

        public virtual void OnDestroy(Server server)
        {
        }
    }
}