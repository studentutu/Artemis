namespace Artemis.Sample.Core
{
    public abstract class AClientState
    {
        public virtual void OnStateEntered(Client client)
        {
        }

        public virtual void OnGUI(Client client)
        {
        }

        public virtual void OnDestroy(Client client)
        {
        }
    }
}