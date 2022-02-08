<div align=center>   
<img src="Graphics/artemis.png">

# Artemis
### A layered rUDP client for Unity
  Artemis is an extension on top of the User Datagram Protocol (UDP), adding a layer of reliability in the delivery of packets when required.
</div>

<div align=center>
  
## APIs
```csharp
void SendUnreliableMessage<T>(T obj, Address recipient)
```
  
```csharp
void SendReliableMessage<T>(T obj, Address recipient, CancellationToken ct = default)
```
  
```csharp
Task<object> RequestAsync<T>(T obj, Address recipient, CancellationToken ct = default) 
```
  
```csharp
Task<object> RequestAsync<T>(T obj, Address recipient, TimeSpan timeout, CancellationToken ct = default)
```

</div>

<div align=center>
  
## Credits
<a href="https://www.flaticon.com/free-icons/artemis" title="artemis icons">Artemis icon created by max.icons - Flaticon</a>
</div>
