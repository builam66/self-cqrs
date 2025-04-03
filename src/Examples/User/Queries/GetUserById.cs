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
