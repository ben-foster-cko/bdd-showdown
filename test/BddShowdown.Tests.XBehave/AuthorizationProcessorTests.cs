using System;
using System.Collections.Generic;
using Shouldly;
using Xbehave;
using Xunit;

namespace BddShowdown.Tests
{
    public class AuthorizationProcessorTests : NSpecShim
    {
        AuthorizationRequest authorizationRequest;
        AuthorizationResponse authorizationResponse;
        AuthorizationProcessor processor;
        AuthorizationProcessor authorizationProcessor;
        List<IAcquirer> acquirers;
        Exception ex;

        public AuthorizationProcessorTests()
        {
            authorizationProcessor = new AuthorizationProcessor();
            authorizationRequest = new AuthorizationRequest(10_99, "USD");
            acquirers = new List<IAcquirer>();
            processor = new AuthorizationProcessor();
        }

        [Scenario(DisplayName = "Authorization request is null")]
        public void NullRequest()
        {
            Given("authorization request is null", () => authorizationRequest = null);
            WhenProcessing();
            Then($"it throws {nameof(ArgumentNullException)}", () => ex.ShouldBeOfType<ArgumentNullException>());
        }

        [Scenario(DisplayName = "Acquirers null")]
        public void NullAcquirers()
        {
            Given("acquirers are null", () => acquirers = null);
            WhenProcessing();
            Then($"it throws {nameof(ArgumentNullException)}", () => ex.ShouldBeOfType<ArgumentNullException>());
        }

        [Scenario(DisplayName = "Acquirers empty")]
        public void EmptyAcquirers()
        {
            Given("no acquirers are provided", () => acquirers = new List<IAcquirer>());
            WhenProcessing();
            Then($"it throws {nameof(ArgumentException)}", () => ex.ShouldBeOfType<ArgumentException>());
        }

        [Scenario(DisplayName = "Single acquirer that throws")]
        public void SingleAcquirerThatThrows()
        {
            Given("a single acquirer that throws", () => acquirers.Add(new ThrowingAcquirer()));
            WhenProcessing();
            ThenItIsNotApproved();
        }

        [Scenario(DisplayName = "Single acquirer that declines")]
        public void SingleAcquirerThatDeclined()
        {
            Given("a single acquirer that declines the payment", () => acquirers.Add(new TestAcquirer(false)));
            WhenProcessing();
            ThenItIsNotApproved();
        }

        [Scenario(DisplayName = "Single acquirer that approves")]
        public void SingleAcquirerThatApproves()
        {
            Given("a single acquirer that approves the payment", () => acquirers.Add(new TestAcquirer(true)));
            WhenProcessing();
            ThenItIsApproved();
        }

        [Scenario(DisplayName = "Multiple acquirers with no cascade")]
        public void MultipleAcquirersApproval()
        {
            Given("multiple acquirers and the first approves the payment", () 
                => acquirers.AddRange(new[] { new TestAcquirer(true), new TestAcquirer(false) }));
            WhenProcessing();
            ThenItIsApproved();
            Then("it does not cascade", () => authorizationResponse.Transactions.Count.ShouldBe(1));
        }

        [Scenario(DisplayName = "Multiple acquirers with decline no cascade")]
        public void MultipleAcquirerWithDeclineNoCascade()
        {
            Given("multiple acquirers and the first declines the payment and the response code does not allow cascading", () 
                => acquirers.AddRange(new[] { new TestAcquirer(false), new TestAcquirer(false, "20054") }));
            WhenProcessing();
            ThenItIsNotApproved();
            Then("it does not cascade", () => authorizationResponse.Transactions.Count.ShouldBe(1));
        }

       [Scenario(DisplayName = "Multiple acquirers with decline and cascade")]
        public void MultipleAcquirerWithDeclineCascade()
        {
            Given("multiple acquirers and the first declines the payment and the response code does allow cascading", () 
                => acquirers.AddRange(new[] { new TestAcquirer(false, ResponseCodes.DoNotHonour), new TestAcquirer(true) }));
            WhenProcessing();
            ThenItIsApproved();
            Then("it cascades", () => authorizationResponse.Transactions.Count.ShouldBe(2));
        }

        void WhenProcessing()
            => When("processing", () => ex = Record.Exception(() => authorizationResponse = processor.AuthorizePayment(authorizationRequest, acquirers)));

        void ThenItIsNotApproved()
        {
            Then("it is not approved", () => authorizationResponse.Approved.ShouldBeFalse());
            ThenItReturnsTransactions();
        }

        void ThenItIsApproved()
        {
            Then("it is approved", () => authorizationResponse.Approved.ShouldBeTrue());
            ThenItReturnsTransactions();
        }

        void ThenItReturnsTransactions()
            => Then("it returns the transactions", () => authorizationResponse.Transactions.ShouldNotBeNull());

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