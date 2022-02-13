using Artemis.Sample.Server.Core;

namespace Artemis.Sample.Server.States
{
    public abstract class AServerState
    {
        public virtual void OnStateEntered(DapperServer dapperServer)
        {
        }

        public virtual void OnGUI(DapperServer dapperServer)
        {
        }

        public virtual void OnDestroy(DapperServer dapperServer)
        {
        }
    }
}