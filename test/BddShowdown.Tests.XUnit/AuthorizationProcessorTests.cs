using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace BddShowdown.Tests.XUnit
{
    public class AuthorizationProcessorTests
    {
        private AuthorizationProcessor _processor;

        public AuthorizationProcessorTests()
        {
            _processor = new AuthorizationProcessor();
        }

        public class Constructor : AuthorizationProcessorTests
        {
            [Fact]
            public void ShouldConstruct()
            {
                new AuthorizationProcessor().ShouldNotBeNull();
            }
        }

        public class AuthorizePayment : AuthorizationProcessorTests
        {
            private AuthorizationRequest _validAuthorizeRequest;

            public AuthorizePayment()
            {
                _validAuthorizeRequest = new AuthorizationRequest(10, "USD");
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenAuthorizationRequestIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => _processor.AuthorizePayment(null, new[] { new TestAcquirer(true, ResponseCodes.DoNotHonour) }));
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenAcquirersIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => _processor.AuthorizePayment(_validAuthorizeRequest, null));
            }


            [Fact]
            public void ShouldThrowArgumentExceptionWhenAcquirersIsEmpty()
            {
                Assert.Throws<ArgumentException>(() => _processor.AuthorizePayment(_validAuthorizeRequest, Enumerable.Empty<IAcquirer>()));
            }

            [Fact]
            public void ShouldApproveTransaction()
            {
                var _acquirer = new TestAcquirer(true, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _acquirer });

                result.Approved.ShouldBeTrue();
            }

            [Fact]
            public void ShouldRefuseTransaction()
            {
                var _acquirer = new TestAcquirer(false, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _acquirer });

                result.Approved.ShouldBe(_acquirer.Approved);
            }

            [Fact]
            public void ShouldReturnTransaction()
            {
                var _acquirer = new TestAcquirer(true, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _acquirer });

                result.Transactions.Count.ShouldBe(1);
            }

            [Fact]
            public void ShouldCatchAcquirerExceptionAndRefuseTransaction()
            {
                var _acquirer = new ThrowingAcquirer();

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _acquirer });

                result.Approved.ShouldBeFalse();
            }

            [Fact]
            public void ShouldCascadeAcquirers()
            {
                var _refusingAcquirer = new TestAcquirer(false, ResponseCodes.DoNotHonour);
                var _approvingAcquirer = new TestAcquirer(true, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _refusingAcquirer, _approvingAcquirer });

                result.Approved.ShouldBeTrue();
            }

            [Fact]
            public void ShouldNotCascadeAcquirers()
            {
                var _refusingAcquirer = new TestAcquirer(false, "10001");
                var _approvingAcquirer = new TestAcquirer(true, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _refusingAcquirer, _approvingAcquirer });

                result.Approved.ShouldBeFalse();
            }

            [Fact]
            public void ShouldReturnCascadedTransaction()
            {
                var _refusingAcquirer = new TestAcquirer(false, ResponseCodes.DoNotHonour);
                var _approvingAcquirer = new TestAcquirer(true, "10000");

                var result = _processor.AuthorizePayment(_validAuthorizeRequest, new[] { _refusingAcquirer, _approvingAcquirer });

                result.Transactions.Count.ShouldBe(2);
            }
        }
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