using MultiLsTokenServer.Domain.Validation.Rules;

namespace MultiLsTokenServer.Domain.Validation;

/// <summary>
/// Provides functionality to provide errors for the object if it is in an invalid state.
/// </summary>
/// <typeparam name="T">The type of this instance.</typeparam>
public abstract class NotifyDataErrorInfo<T>
    where T : NotifyDataErrorInfo<T>
{
    private const string HasErrorsPropertyName = "HasErrors";

    private static readonly RuleCollection<T> rules = [];

    private Dictionary<string, List<object>> errors = null!;

    /// <summary>
    /// Gets a value indicating whether the object has validation errors. 
    /// </summary>
    /// <value><c>true</c> if this instance has errors, otherwise <c>false</c>.</value>
    public virtual bool HasErrors
    {
        get
        {
            InitializeErrors();
            return errors.Count > 0;
        }
    }

    /// <summary>
    /// Gets the rules which provide the errors.
    /// </summary>
    /// <value>The rules this instance must satisfy.</value>
    protected static RuleCollection<T> Rules => rules;



    /// <summary>
    /// Gets the validation errors for a specified property or for the entire object.
    /// </summary>
    /// <param name="propertyName">Name of the property to retrieve errors for. <c>null</c> to 
    /// retrieve all errors for this instance.</param>
    /// <returns>A collection of errors.</returns>
    public ICollection<object> GetErrors(string? propertyName = null)
    {
        InitializeErrors();

        ICollection<object> result;
        if (string.IsNullOrEmpty(propertyName))
        {
            List<object> allErrors = [];

            foreach (var keyValuePair in errors)
            {
                allErrors.AddRange(keyValuePair.Value);
            }

            result = allErrors;
        }
        else
        {
            if (errors.TryGetValue(propertyName, out List<object>? value))
            {
                result = value;
            }
            else
            {
                result = [];
            }
        }

        return result;
    }


    /// <summary>
    /// Applies all rules to this instance.
    /// </summary>
    private void ApplyRules()
    {
        InitializeErrors();

        foreach (string propertyName in rules.Select(x => x.PropertyName))
        {
            ApplyRules(propertyName);
        }
    }

    /// <summary>
    /// Applies the rules to this instance for the specified property.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void ApplyRules(string propertyName)
    {
        InitializeErrors();

        List<object> propertyErrors = rules.Apply((T)this, propertyName).ToList();

        if (propertyErrors.Count > 0)
        {
            if (errors.TryGetValue(propertyName, out List<object>? value))
            {
                value.Clear();
            }
            else
            {
                errors[propertyName] = [];
            }

            errors[propertyName].AddRange(propertyErrors);
        }
        else
        {
            errors.Remove(propertyName);
        }
    }

    /// <summary>
    /// Initializes the errors and applies the rules if not initialized.
    /// </summary>
    private void InitializeErrors()
    {
        if (errors == null)
        {
            errors = [];

            ApplyRules();
        }
    }
}
