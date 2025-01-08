using Dapper;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly DapperDbContext _context;

        public UsersRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            user.UserId = Guid.NewGuid();

            var query = "INSERT INTO public.\"Users\" " +
                "(\"UserId\", \"Email\", \"Password\", \"PersonName\", \"Gender\") " +
                "VALUES (@UserId, @Email, @Password, @PersonName, @Gender)";

            var rowCountAffected = await _context.DbConnection.ExecuteAsync(query, user);

            if (rowCountAffected > 0)
            {
                return user;
            }

            return null;
        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
        {
            var query = "SELECT * FROM public.\"Users\" WHERE \"Email\"=@Email AND \"Password\"=@Password";

            var parameters = new { Email = email, Password = password };

            var user = await _context.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

            return user;
        }
    }
}
