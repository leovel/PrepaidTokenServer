namespace MultiLsTokenServer.Domain.Validation.Rules;

/// <summary>
/// A named rule containing an error to be used if the rule fails.
/// </summary>
/// <typeparam name="T">The type of the object the rule applies to.</typeparam>
public abstract class Rule<T>
{
    private readonly string propertyName;
    private readonly object error;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rule<T>"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property this instance applies to.</param>
    /// <param name="error">The error message if the rules fails.</param>
    protected Rule(string propertyName, object error)
    {
        this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        this.error = error ?? throw new ArgumentNullException(nameof(error));
    }

    /// <summary>
    /// Gets the name of the property this instance applies to.
    /// </summary>
    /// <value>The name of the property this instance applies to.</value>
    public string PropertyName => propertyName;

    /// <summary>
    /// Gets the error message if the rules fails.
    /// </summary>
    /// <value>The error message if the rules fails.</value>
    public object Error => error;

    /// <summary>
    /// Applies the rule to the specified object.
    /// </summary>
    /// <param name="obj">The object to apply the rule to.</param>
    /// <returns>
    /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
    /// </returns>
    public abstract bool Apply(T obj);
}
