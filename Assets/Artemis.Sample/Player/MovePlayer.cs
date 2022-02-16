using System;
using Artemis.Sample.Packets;
using UnityEngine;

namespace Artemis.Sample.Player
{
    public static class MovePlayer
    {
        public static Vector2 Move(Vector2 position, PlayerCommand command, TimeSpan deltaTime)
        {
            var clampedMovement = Vector2.ClampMagnitude(command.Movement, 1f);
            var framedSpeed = (float) (Configuration.PlayerMovementSpeed * deltaTime.TotalSeconds);
            var motion = clampedMovement * framedSpeed;
            return position + motion;
        }
    }
}