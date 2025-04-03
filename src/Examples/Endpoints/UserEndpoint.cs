using Carter;
using CQRS.Dispatchers;
using Examples.User.Commands;
using Examples.User.Queries;

namespace Examples.Endpoints
{
    public class UserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/user/{id}", async (int id, IDispatcher dispatcher) =>
            {
                var query = new GetUserByIdQuery { Id = id };
                var result = await dispatcher.QueryAsync<GetUserByIdQuery, UserDto>(query);

                return Results.Ok(result);
            })
            .WithName("GetUserById");

            app.MapPost("/user", async (CreateUserCommand command, IDispatcher dispatcher) =>
            {
                var userId = await dispatcher.SendAsync<CreateUserCommand, int>(command);

                return Results.Created($"/user/{userId}", new { Id = userId });
            })
            .WithName("CreateUser");
        }
    }
}
