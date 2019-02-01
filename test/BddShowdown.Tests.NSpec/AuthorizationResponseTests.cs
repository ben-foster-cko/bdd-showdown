using System.Collections.Generic;
using NSpec;
using Shouldly;

namespace BddShowdown.Tests
{
    class describe_AuthorizationResponse : nspec
    {
        void given_all_transactions_were_declined()
        {
            var transactions = new[] {
                new Transaction { Approved = false },
                new Transaction { Approved = false }
            };

            var authResponse = new AuthorizationResponse();
            authResponse.Transactions.AddRange(transactions);

            it["is not approved"] = () => authResponse.Approved.ShouldBeFalse();
        }

        void given_any_transactions_was_approved()
        {
            var transactions = new[] {
                new Transaction { Approved = false },
                new Transaction { Approved = true }
            };

            var authResponse = new AuthorizationResponse();
            authResponse.Transactions.AddRange(transactions);

            it["is approved"] = () => authResponse.Approved.ShouldBeTrue();
        }

        // or

        void defined_approved()
        {
            List<Transaction> transactions = null;
            bool approved = false;

            before = () => transactions = new List<Transaction>();

            act = () =>
            {
                var authorizationResponse = new AuthorizationResponse();
                authorizationResponse.Transactions.AddRange(transactions);
                approved = authorizationResponse.Approved;
            };

            context["given all transactions were declined"] = () =>
            {
                before = () => transactions.AddRange(new[] {
                    new Transaction { Approved = false },
                    new Transaction { Approved = false }
                });

                it["is not approved"] = () => approved.ShouldBeFalse();
            };

            context["given any transaction was declined"] = () =>
            {
                before = () => transactions.AddRange(new[] {
                    new Transaction { Approved = false },
                    new Transaction { Approved = true }
                });

                it["is approved"] = () => approved.ShouldBeTrue();
            };
        }

        //

        void it_is_approved_when_any_transaction_is_approved()
        {
            var response = new AuthorizationResponse();
            response.Transactions.Add(new Transaction { Approved = true });
            response.Approved.ShouldBeTrue();
        }
    }
}