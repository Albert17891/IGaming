namespace IGaming.Domain.Exceptions;
public class SameUserNameExceptions : Exception
{
    public SameUserNameExceptions(string message) : base(message) { }

}
