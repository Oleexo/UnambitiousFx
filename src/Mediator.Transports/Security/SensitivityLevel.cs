namespace UnambitiousFx.Mediator.Transports.Security;

/// <summary>
///     Defines the sensitivity level of data for classification and encryption purposes.
/// </summary>
public enum SensitivityLevel
{
    /// <summary>
    ///     Public data that can be freely shared without restrictions.
    /// </summary>
    Public = 0,

    /// <summary>
    ///     Internal data that should only be accessible within the organization.
    /// </summary>
    Internal = 1,

    /// <summary>
    ///     Confidential data that requires protection and limited access.
    /// </summary>
    Confidential = 2,

    /// <summary>
    ///     Restricted data with the highest level of protection, such as PII or financial information.
    /// </summary>
    Restricted = 3
}
