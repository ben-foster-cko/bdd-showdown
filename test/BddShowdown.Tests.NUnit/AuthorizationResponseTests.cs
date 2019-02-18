using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace BddShowdown.Tests.NUnit
{
    [TestFixture]
    public class AuthorizationResponseTests
    {
        private List<Transaction> _transactions;

        private Transaction _approved => new Transaction { Approved = true };
        private Transaction _declined => new Transaction { Approved = false };

        [SetUp]
        public void SetUp()
        {
            _transactions = new List<Transaction>();
        }

        [Test]
        public void All_Transactions_Where_Declined()
        {
            _transactions.Add(_declined);
            _transactions.Add(_declined);

            var sut = new AuthorizationResponse();
            sut.Transactions.AddRange(_transactions);

            sut.Approved.ShouldBe(false);
        }

        [Test]
        public void Any_Transaction_Is_Approved()
        {
            _transactions.Add(_declined);
            _transactions.Add(_approved);

            var sut = new AuthorizationResponse();
            sut.Transactions.AddRange(_transactions);

            sut.Approved.ShouldBe(true);
        }
    }
}