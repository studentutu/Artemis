using Artemis.Sample.Packets;
using UnityEngine;

namespace Artemis.Sample.Player
{
    public static class MovePlayer
    {
        public static Vector2 Move(Vector2 position, PlayerCommand command)
        {
            var clampedMovement = Vector2.ClampMagnitude(command.Movement, 1f);
            var motion = clampedMovement * Configuration.PlayerMovementSpeed * (float) Configuration.TickInterval.TotalSeconds;
            return position + motion;
        }
    }
}