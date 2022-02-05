using System;

namespace Artemis.Exceptions
{
    internal class ObjectTypeUnhandledException : Exception
    {
        internal ObjectTypeUnhandledException(object obj) : base(GenerateMessage(obj))
        {
        }

        private static string GenerateMessage(object obj)
        {
            return $"Unhandled object type {obj.GetType().FullName}";
        }
    }
}