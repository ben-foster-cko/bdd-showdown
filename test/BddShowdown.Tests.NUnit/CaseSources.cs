using System;
using System.Collections.Generic;

namespace BddShowdown.Tests.NUnit
{
    public static class CaseSources
    {
        public static IEnumerable<object[]> AuthorizationProcessorParameterCases()
        {
            yield return new object[]
            {
                null,
                new List<IAcquirer> { AcquirerResponses.Approved },
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                new AuthorizationRequest(100, "gbp"),
                null,
                typeof(ArgumentNullException)
            };

            yield return new object[]
            {
                new AuthorizationRequest(100, "gbp"),
                new List<IAcquirer>(),
                typeof(ArgumentException)
            };
        }
    }
}