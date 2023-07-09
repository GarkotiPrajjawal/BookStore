using BookStore.Models;
using BookStore.Repository.iRepository;


namespace BookStore.Repository
{
    public class PublisherRepository : Repository<Publisher>,iPublisherRepository
    {
        private readonly ApplicationDbContext _db;
        public PublisherRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
