using Braintree;
using Microsoft.Extensions.Options;

namespace Store.Utilities.Braintree
{
    public class BraintreeGate:IBraintreeGate
    {
        public BraintreeSettings BraintreeOptions { get; set; }
        public IBraintreeGateway? BraintreeGetWay { get; set; }

        public BraintreeGate(IOptions<BraintreeSettings> options)
        {
            BraintreeOptions=options.Value;
        }

        public IBraintreeGateway CreateGateway() =>
            new BraintreeGateway(
                BraintreeOptions.Environment,
                BraintreeOptions.MerchantId,
                BraintreeOptions.PublicKey,
                BraintreeOptions.PrivateKey);

        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGetWay==null)
            {
                BraintreeGetWay=CreateGateway();
                return BraintreeGetWay;
            }
            else
            {
                return BraintreeGetWay;
            }
        }
    }
}