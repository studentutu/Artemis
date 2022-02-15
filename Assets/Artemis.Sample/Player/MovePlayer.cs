using Artemis.Sample.Packets;
using UnityEngine;

namespace Artemis.Sample.Player
{
    public static class MovePlayer
    {
        public static Vector2 Move(Vector2 position, PlayerCommand command)
        {
            var input = new Vector2(command.Horizontal, command.Vertical);
            var motion = Vector2.ClampMagnitude(input, 1f) * Configuration.PlayerMovementSpeed * (float) Configuration.TickInterval.TotalSeconds;
            return position + motion;
        }
    }
}