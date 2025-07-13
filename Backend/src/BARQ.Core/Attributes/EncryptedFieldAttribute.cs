using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EncryptedFieldAttribute : ValidationAttribute
{
    public bool IsSearchable { get; set; } = false;
    public string? KeyId { get; set; }
    public bool RequireDecryption { get; set; } = true;

    public EncryptedFieldAttribute()
    {
        ErrorMessage = "Encrypted field validation failed";
    }

    public EncryptedFieldAttribute(bool isSearchable) : this()
    {
        IsSearchable = isSearchable;
    }

    public EncryptedFieldAttribute(string keyId) : this()
    {
        KeyId = keyId;
    }

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

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SearchableEncryptedAttribute : EncryptedFieldAttribute
{
    public SearchableEncryptedAttribute() : base(true)
    {
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PiiFieldAttribute : EncryptedFieldAttribute
{
    public PiiFieldAttribute() : base()
    {
        RequireDecryption = true;
    }
}
