using Artemis.UserInterface;
using UnityEngine;

public partial class Client
{
    private void HandleServerClosingMessage(Message<ServerClosingMessage> message)
    {
        Debug.Log("<b>[C]</b> Server was closed");
        Disconnect();
    }
}