using FluentValidation;
using Pragmatest.Wallets.Web.Models;

namespace Pragmatest.Wallets.Web.Validators
{
    public class WithdrawalRequestValidator : AbstractValidator<WithdrawalRequest> {
    
        public WithdrawalRequestValidator()
        {
            RuleFor(withdrawalRequest => withdrawalRequest.Amount).GreaterThanOrEqualTo(0);
        }
    }
}
