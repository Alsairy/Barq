using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Attributes;

/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EncryptedFieldAttribute : ValidationAttribute
{
    /// <summary>
    /// </summary>
    public bool IsSearchable { get; set; } = false;
    
    /// <summary>
    /// </summary>
    public string? KeyId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequireDecryption { get; set; } = true;

    /// <summary>
    /// </summary>
    public EncryptedFieldAttribute()
    {
        ErrorMessage = "Encrypted field validation failed";
    }

    /// <summary>
    /// </summary>
    public EncryptedFieldAttribute(bool isSearchable) : this()
    {
        IsSearchable = isSearchable;
    }

    /// <summary>
    /// </summary>
    public EncryptedFieldAttribute(string keyId) : this()
    {
        KeyId = keyId;
    }

    /// <summary>
    /// </summary>
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is not string stringValue)
            return false;

        if (string.IsNullOrEmpty(stringValue))
            return true;

        return true;
    }
}

/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SearchableEncryptedAttribute : EncryptedFieldAttribute
{
    /// <summary>
    /// </summary>
    public SearchableEncryptedAttribute() : base(true)
    {
    }
}

/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PiiFieldAttribute : EncryptedFieldAttribute
{
    /// <summary>
    /// </summary>
    public PiiFieldAttribute() : base()
    {
        RequireDecryption = true;
    }
}
