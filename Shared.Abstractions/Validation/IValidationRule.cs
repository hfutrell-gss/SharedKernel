namespace Shared.Abstractions.Validation;

public interface IValidationRule
{
}

public interface IValidationRule<in T> : IValidationRule
{
    public bool IsValid(T value);
}