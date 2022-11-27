using Braintree;

namespace Store.Utilities.Braintree
{
    public interface IBraintreeGate
    {
        public IBraintreeGateway CreateGateway();
        public IBraintreeGateway GetGateway();
    }
}