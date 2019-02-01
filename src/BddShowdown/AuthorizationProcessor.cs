using System;
using System.Collections.Generic;
using System.Linq;

namespace BddShowdown
{
    public class AuthorizationProcessor
    {       
        public AuthorizationResponse AuthorizePayment(AuthorizationRequest authorizationRequest, IEnumerable<IAcquirer> acquirers)
        {
            if (authorizationRequest == null) throw new ArgumentNullException(nameof(authorizationRequest));
            if (acquirers == null) throw new ArgumentNullException(nameof(acquirers));
            if (!acquirers.Any()) throw new ArgumentException("At least one acquirer must be provided", nameof(acquirers));

            var response = new AuthorizationResponse();

            foreach (var acquirer in acquirers)
            {
                var transaction = Process(acquirer, authorizationRequest);
                response.Transactions.Add(transaction);
                if (transaction.Approved || !ResponseCodes.CanCascade(transaction.ResponseCode))
                    break;
            }

            return response;
        }

        private Transaction Process(IAcquirer acquirer, AuthorizationRequest authorizationRequest)
        {
            try
            {
                return acquirer.Process(authorizationRequest);
            }
            catch
            {
                return new Transaction { Approved = false };
            }
        }
    }

    public class AuthorizationRequest
    {
        public AuthorizationRequest(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public string Currency { get; }
    }

    public class AuthorizationResponse
    {
        public AuthorizationResponse()
        {
            Transactions = new List<Transaction>();
        }

        public List<Transaction> Transactions { get; }
        public bool Approved => Transactions.Any(t => t.Approved);
    }

    public class Transaction
    {
        public bool Approved { get; set; }
        public string ResponseCode { get; set; }
    }

    public interface IAcquirer
    {
        Transaction Process(AuthorizationRequest authorizationRequest);
    }

    public class ResponseCodes 
    {
        public const string DoNotHonour = "20005";

        public static bool CanCascade(string responseCode)
        {
            return DoNotHonour == responseCode;
        }
    }
}


