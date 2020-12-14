using System;

namespace Monochromator.App.Exceptions {
    /// <summary>
    /// Exception thrown when an unknown enumeration value is encountered
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    public class UnknownEnumValueException<T> : Exception where T : Enum {
        public UnknownEnumValueException(T value) : base($"Unknown {typeof(T).Name}: {value}") { }
    }
}