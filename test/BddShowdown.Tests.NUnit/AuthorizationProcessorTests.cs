using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace BddShowdown.Tests.NUnit
{
    [TestFixture]
    public class AuthorizationProcessorTests
    {
        private AuthorizationProcessor _sut = null;
        private AuthorizationRequest _authRequest = null;
        private List<IAcquirer> _acquirers = null;

        [SetUp]
        public void SetUp()
        {
            _sut = new AuthorizationProcessor();
            _acquirers = new List<IAcquirer>();
            _authRequest = new AuthorizationRequest(100, "gbp");
        }

        [Test]
        [TestCaseSource(nameof(AuthorizationProcessorParameterCases))]
        public void Violating_Params_Must_Throw_Excepted_Exception(AuthorizationRequest authRequest, List<IAcquirer> acquirers, Type expected)
        {
            Should.Throw(() => _sut.AuthorizePayment(authRequest, _acquirers), expected);
        }

        [Test]
        public void Single_Acquirer_Declines_The_Payment()
        {
            _acquirers.Add(AcquirerResponses.Declined);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(false);
            actual.Transactions.Count.ShouldBe(1);
        }

        [Test]
        public void Single_Acquirer_Approves_The_Payment()
        {
            _acquirers.Add(AcquirerResponses.Approved);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(true);
            actual.Transactions.Count.ShouldBe(1);
        }

        [Test]
        public void Single_Acquirer_Throws_Exception()
        {
            _acquirers.Add(AcquirerResponses.Throwing);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(false);
            actual.Transactions.Count.ShouldBe(1);
        }

        [Test]
        public void First_Acquirer_Approved_The_Payment()
        {
            _acquirers.Add(AcquirerResponses.Approved);
            _acquirers.Add(AcquirerResponses.Declined);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(true);
            actual.Transactions.Count.ShouldBe(1);
        }

        [Test]
        public void First_Acquirer_Declines_The_Payment_With_No_Cascade()
        {
            _acquirers.Add(AcquirerResponses.Declined);
            _acquirers.Add(AcquirerResponses.Approved);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(false);
            actual.Transactions.Count.ShouldBe(1);
        }

        [Test]
        public void Next_Acquirer_Is_Attempted_After_First_Acquirer_Declines()
        {
            _acquirers.Add(AcquirerResponses.DNH);
            _acquirers.Add(AcquirerResponses.Approved);

            var actual = _sut.AuthorizePayment(_authRequest, _acquirers);

            actual.Approved.ShouldBe(true);
            actual.Transactions.Count.ShouldBe(2);
        }

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
                typeof(ArgumentException)
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
