using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace RealEstate.Application.Attributes;

public class ObjectIdAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is not string stringValue)
            return false;

        return ObjectId.TryParse(stringValue, out _);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be a valid ObjectId.";
    }
}