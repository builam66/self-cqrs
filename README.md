# self-cqrs
Self-implement simple CQRS in C#.

```
src/
├── CQRS/
│   ├── Command/
│   │   ├── ICommand.cs
│   │   ├── ICommandHandler.cs
│   │   └── ICommandDispatcher.cs
│   ├── Query/
│   │   ├── IQuery.cs
│   │   ├── IQueryHandler.cs
│   │   └── IQueryDispatcher.cs
│   ├── Dispatchers/
│   │   ├── Dispatcher.cs
│   │   ├── CommandDispatcher.cs
│   │   └── QueryDispatcher.cs
│   ├── Exceptions/
│   │    └── NotFoundException.cs
│   └── DependencyInjection.cs
└── Examples/
    ├── Endpoints/
    │   └── UserEndpoint.cs
    ├── User/
    │   ├── Commands/CreateUser.cs
    |   └── Queries/GetUserById.cs
    └── Program.cs
```

## Examples

### Program Setup

```csharp
using Carter;
using CQRS;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

// Add CQRS
builder.Services.AddCQRS(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapCarter();

app.Run();
```

### Query

```csharp
using CQRS.Query;

namespace Examples.User.Queries
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class GetUserByIdQuery : IQuery<UserDto>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        public Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
        {
            // Simulate retrieving user from database
            var user = new UserDto
            {
                Id = query.Id,
                Name = $"User {query.Id}",
                Email = $"user{query.Id}@example.com"
            };

            return Task.FromResult(user);
        }
    }
}
```

### Command

```csharp
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
```
