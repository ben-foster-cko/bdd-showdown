using System;
using System.Collections.Generic;
using NSpec;
using Shouldly;

namespace BddShowdown.Tests
{
    public class describe_AuthorizationProcessor : nspec
    {
        AuthorizationRequest authorizationRequest;
        AuthorizationResponse authorizationResponse;
        List<IAcquirer> acquirers;
        AuthorizationProcessor processor;

        void before_all() => processor = new AuthorizationProcessor();

        void before_each()
        {
            authorizationRequest = new AuthorizationRequest(10_99, "USD");
            acquirers = new List<IAcquirer>();
        }

        void when_processing()
        {
            act = () => authorizationResponse = processor.AuthorizePayment(authorizationRequest, acquirers);

            context["given authorization request is null"] = () =>
            {
                before = () => authorizationRequest = null;
                it[$"throws {nameof(ArgumentNullException)}"] = expect<ArgumentNullException>();
            };

            context["given acquirers is null"] = () =>
            {
                before = () => acquirers = null;
                it[$"throws {nameof(ArgumentNullException)}"] = expect<ArgumentNullException>();
            };

            context["given no acquirers are provided"] = () =>
            {
                before = () => acquirers = new List<IAcquirer>();
                it[$"throws {nameof(ArgumentException)}"] = expect<ArgumentException>();
            };

            context["given one acquirer is provided"] = () =>
            {
                context["and the acquirer declines the payment"] = () =>
                {
                    before = () => acquirers.Add(new TestAcquirer(false));
                    it["is not approved"] = () => authorizationResponse.Approved.ShouldBeFalse();
                    it["returns the acquirer transaction"] = () => authorizationResponse.Transactions.Count.ShouldBe(1);
                };

                context["and the acquirer throws"] = () =>
                {
                    before = () => acquirers.Add(new ThrowingAcquirer());
                    it["is not approved"] = () => authorizationResponse.Approved.ShouldBeFalse();
                };

                context["and the acquirer approves the payment"] = () =>
                {
                    before = () => acquirers.Add(new TestAcquirer(true));
                    it["is approved"] = () => authorizationResponse.Approved.ShouldBeTrue();
                    it["returns the acquirer transaction"] = () => authorizationResponse.Transactions.Count.ShouldBe(1);
                };
            };

            context["given multiple acquirers are provided"] = () =>
            {
                context["and the first acquirer approves the payment"] = () =>
                {
                    before = () => acquirers.AddRange(new[] { new TestAcquirer(true), new TestAcquirer(false) });
                    it["is does not cascade"] = () => authorizationResponse.Approved.ShouldBeTrue();
                    it["returns the acquirer transaction"] = () => authorizationResponse.Transactions.Count.ShouldBe(1);
                };

                context["and the first acquirer declines the payment"] = () =>
                {
                    context["and the response code can not be cascaded"] = () =>
                    {
                        before = () => acquirers.AddRange(new[] { new TestAcquirer(false, "20054"), new TestAcquirer(true) });
                        it["is does not cascade"] = () => authorizationResponse.Approved.ShouldBeFalse();
                        it["returns the transaction"] = () => authorizationResponse.Transactions.Count.ShouldBe(1);
                        it["returns the acquirer transaction"] = () => authorizationResponse.Transactions.Count.ShouldBe(1);
                    };

                    context["and the response code can be cascaded"] = () =>
                    {
                        before = () => acquirers.AddRange(new[] { new TestAcquirer(false, ResponseCodes.DoNotHonour), new TestAcquirer(true) });
                        it["cascades to the next acquirer"] = () => authorizationResponse.Approved.ShouldBeTrue();
                        it["returns both acquirer transactions"] = () => authorizationResponse.Transactions.Count.ShouldBe(2);
                    };
                };
            };
        }

        class ThrowingAcquirer : IAcquirer
        {
            public Transaction Process(AuthorizationRequest authorizationRequest)
            {
                throw new Exception("I always throw");
            }
        }

        class TestAcquirer : IAcquirer
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
    }
}