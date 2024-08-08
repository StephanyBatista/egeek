namespace EGeek.Common.CustomException;

public class BadException(string message) : ArgumentException(message)
{
    const string FieldEmpty = "The field {0} cannot be empty";
    const string FieldInvalid = "The field {0} is invalid";

    public static void ThrowIfNullOrEmpt(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
            throw new BadException(string.Format(FieldEmpty, name));
    }
    
    public static void ThrowIf(bool isInvalid, string name)
    {
        if (isInvalid)
            throw new BadException(string.Format(FieldInvalid, name));
    }
    
    public static void ThrowIfWithMessage(bool isInvalid, string message)
    {
        if (isInvalid)
            throw new BadException(message);
    }
    
    public static void ThrowIfEmailInvalid(string email, string name)
    {
        ThrowIf(!email.Contains('@') || !email.Contains('.'), name);
    }
}