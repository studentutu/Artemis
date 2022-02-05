using System;

namespace Artemis.Exceptions
{
    public class ObjectTypeUnhandledException : Exception
    {
        public ObjectTypeUnhandledException(object obj) : base(GenerateMessage(obj))
        {
        }

        private static string GenerateMessage(object obj)
        {
            return $"Unhandled object type {obj.GetType().FullName}";
        }
    }
}