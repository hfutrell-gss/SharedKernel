namespace Shared.Abstractions.Validation;

public interface IValidationRuleService
{
    public IReadOnlyCollection<IValidationRule<T>> GetRules<T>() where T : IValidationRule;
}