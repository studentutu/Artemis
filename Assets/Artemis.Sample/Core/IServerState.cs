namespace Artemis.Sample.Core
{
    public interface IServerState
    {
        void OnStateEntered(Server server);
        void OnGUI(Server server);
        void OnDestroy(Server server);
    }
}