namespace Caml
{
    public interface IMatcher
    {
        bool Match(string literalValue);

        bool Match(string functionName, params string[] arguments);
    }
}