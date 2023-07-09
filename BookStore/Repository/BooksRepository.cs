using BookStore.Models;
using BookStore.Repository.iRepository;

namespace BookStore.Repository
{
    public class BooksRepository : Repository<Books>, iBooksRepository
    {
        private readonly ApplicationDbContext _db;
        public BooksRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
