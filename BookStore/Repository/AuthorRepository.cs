using BookStore.Models;
using BookStore.Repository.iRepository;

namespace BookStore.Repository
{
    public class AuthorRepository : Repository<Author> ,iAuthorRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
