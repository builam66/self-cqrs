namespace CQRS.Command
{
    public interface ICommand
    {
    }

    public interface ICommand<out TResult>
    {
    }
}
