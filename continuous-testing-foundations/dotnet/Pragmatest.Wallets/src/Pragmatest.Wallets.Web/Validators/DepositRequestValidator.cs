using FluentValidation;
using Pragmatest.Wallets.Web.Models;

namespace Pragmatest.Wallets.Web.Validators
{
    public class DepositRequestValidator : AbstractValidator<DepositRequest> {
    
        public DepositRequestValidator()
        {
            RuleFor(depositRequest => depositRequest.Amount).GreaterThanOrEqualTo(0);
        }
    }
}
