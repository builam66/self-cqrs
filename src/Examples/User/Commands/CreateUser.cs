using CQRS.Command;

namespace Examples.User.Commands
{
    public class CreateUserCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
    {
        public Task<int> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            // Simulate creating a user and returning ID
            return Task.FromResult(new Random().Next(1, 1000));
        }
    }
}
