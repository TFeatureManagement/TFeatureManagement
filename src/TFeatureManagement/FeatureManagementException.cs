namespace TFeatureManagement;

public class FeatureManagementException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureManagementException"/> class.
    /// </summary>
    /// <param name="errorType">The feature management error type.</param>
    /// <param name="message">The exception message.</param>
    public FeatureManagementException(FeatureManagementErrorType errorType, string message)
        : base(message)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public FeatureManagementErrorType ErrorType { get; }
}
