using BookStore.Models;
using BookStore.Repository.iRepository;

namespace BookStore.Repository
{
    public class UserRepository : Repository<User>, iUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
