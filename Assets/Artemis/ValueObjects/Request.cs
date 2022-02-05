using Artemis.Clients;
using Artemis.ValueObjects;

public readonly struct Request<TRequest>
{
    public readonly TRequest Payload;
    public readonly Address Sender;
    private readonly string _id;
    private readonly ArtemisClient _means;

    public Request(string id, TRequest request, Address sender, ArtemisClient means)
    {
        _id = id;
        Payload = request;
        Sender = sender;
        _means = means;
    }

    public void Reply<TResponse>(TResponse response)
    {
        _means.SendMessage(new ResponseContainer(_id, response), Sender, DeliveryMethod.Reliable);
    }
}