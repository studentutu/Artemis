using System;
using Artemis.UserInterface;

public class ServerClosingMessageHandler : IMessageHandler<ServerClosingMessage>
{
    private readonly Action _disconnectFunction;

    public ServerClosingMessageHandler(Action disconnectFunction)
    {
        _disconnectFunction = disconnectFunction;
    }
    
    public void Handle(Message<ServerClosingMessage> message)
    {
        _disconnectFunction.Invoke();
    }
}