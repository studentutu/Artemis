namespace Artemis.Sample.Core
{
    public interface IClientState
    {
        void OnStateEntered(Client client);
        void OnGUI(Client client);
        void OnDestroy(Client client);
    }
}