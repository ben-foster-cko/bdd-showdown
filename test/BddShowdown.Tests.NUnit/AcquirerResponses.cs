using System;

namespace BddShowdown.Tests.NUnit
{
    public class AcquirerResponses
    {
        public static IAcquirer Approved => new TestAcquirer(true, "10000");
        public static IAcquirer Declined => new TestAcquirer(false, "20054");
        public static IAcquirer DNH => new TestAcquirer(false, ResponseCodes.DoNotHonour);
        public static IAcquirer Throwing => new ThrowingAcquirer();
    }

    public class TestAcquirer : IAcquirer
    {
        public TestAcquirer(bool approved, string responseCode = null)
        {
            Approved = approved;
            ResponseCode = responseCode;
        }

        public bool Approved { get; }
        public string ResponseCode { get; }

        public Transaction Process(AuthorizationRequest authorizationRequest)
        {
            return new Transaction { Approved = Approved, ResponseCode = ResponseCode };
        }
    }

    public class ThrowingAcquirer : IAcquirer
    {
        public Transaction Process(AuthorizationRequest authorizationRequest)
        {
            throw new Exception("I always throw");
        }
    }
}