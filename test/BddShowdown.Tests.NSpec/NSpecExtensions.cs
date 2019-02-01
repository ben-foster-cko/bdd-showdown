using System;
using NSpec;
using NSpec.Domain;

namespace BddShowdown.Tests
{
    public static class NSpecExtensions
    {
        public static void ItThrows<TException>(this nspec spec) where TException : Exception
        {
            spec.it[$"throws {typeof(TException).Name}"] = spec.expect<TException>();
        }
    }
}