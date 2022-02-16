using Artemis.Sample.ValueObjects;

namespace Artemis.Sample.Input
{
    public static class DapperInput
    {
        public static Int2 GetMovementInput()
        {
            var horizontal = Keyboard.GetAxis(Keyboard.Key.D, Keyboard.Key.A);
            var vertical = Keyboard.GetAxis(Keyboard.Key.W, Keyboard.Key.S);
            return new Int2(horizontal, vertical);
        }
    }
}