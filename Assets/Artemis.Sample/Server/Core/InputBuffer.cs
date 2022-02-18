using System;
using System.Collections.Generic;
using System.Linq;
using Artemis.Sample.Packets;
using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Server.Core
{
    public class InputBuffer
    {
        private readonly Dictionary<Address, List<PlayerCommand>> _registry = new();

        private readonly Dictionary<Address, PlayerCommand> _lastConsumedCommand = new();

        public void Enqueue(Message<PlayerCommand> message)
        {
            EnsureAddressInsertion(message.Sender);
            message.Payload.EnqueuedAt = DateTime.UtcNow;
            _registry[message.Sender].Add(message.Payload);
        }
        
        public PlayerCommand Get(int tick, Address owner)
        {
            EnsureAddressInsertion(owner);
            var playerCommand = _registry[owner].FirstOrDefault(pc => pc.Tick == tick);

            if (playerCommand != null)
            {
                var sittingFor = DateTime.UtcNow - playerCommand.EnqueuedAt;
                Debug.LogWarning($"Input {tick} was consumed after sitting on buffer for {sittingFor.TotalMilliseconds}ms");
                _registry[owner].Remove(playerCommand);
                _lastConsumedCommand[owner] = playerCommand;
                return playerCommand;
            }

            if (_lastConsumedCommand[owner] != default)
            {
                Debug.LogError("Returning last command!");
                return _lastConsumedCommand[owner];
            }

            Debug.LogError("Returning default command!");
            return new PlayerCommand(tick, default);
        }

        private void EnsureAddressInsertion(Address address)
        {
            if (!_registry.ContainsKey(address))
            {
                _registry.Add(address, new List<PlayerCommand>());
            }

            if (!_lastConsumedCommand.ContainsKey(address))
            {
                _lastConsumedCommand.Add(address, null);
            }
        }
    }
}