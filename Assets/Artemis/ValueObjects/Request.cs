using Artemis.Clients;
using Artemis.ValueObjects;

public class Request<T> : Message<T>
{
    private readonly string _id;
    private readonly ArtemisClient _means;

    public Request(string id, T payload, Address sender, ArtemisClient means) : base(payload, sender)
    {
        _id = id;
        _means = means;
    }

    public void Reply<T>(T obj)
    {
        _means.SendMessage(new Response(_id, obj), Sender, DeliveryMethod.Reliable);
    }
}