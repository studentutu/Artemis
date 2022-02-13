namespace Artemis.Sample.Core
{
    public abstract class AClientState
    {
        public virtual void OnStateEntered(DapperClient dapperClient)
        {
        }

        public virtual void OnGUI(DapperClient dapperClient)
        {
        }

        public virtual void OnDestroy(DapperClient dapperClient)
        {
        }
    }
}