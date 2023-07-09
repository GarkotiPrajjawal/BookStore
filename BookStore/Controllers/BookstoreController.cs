using AutoMapper;
using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Repository.iRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("Books")]
    public class BookstoreController : ControllerBase
    {
        private readonly iBooksRepository _BooksRepository;
        private readonly iAuthorRepository _AuthorRepository;
        private readonly iPublisherRepository _PublisherRepository;
        private readonly IMapper _mapper;
        public BookstoreController(iBooksRepository BooksRepository, IMapper mapper, iAuthorRepository authorRepository, iPublisherRepository publisherRepository)
        {
            _BooksRepository = BooksRepository;
            _mapper = mapper;
            _AuthorRepository = authorRepository;
            _PublisherRepository = publisherRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBooks()
        {
            var Bookslist = await _BooksRepository.GetAllAsync();
            if (Bookslist == null)
            {
                return BadRequest();
            }
            return Ok(Bookslist);
        }
        [HttpGet("{id:int}", Name = "GetBookByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookbyid(int id)
        {
            if (id == 0) { return BadRequest(); }

            Books book = await _BooksRepository.GetAsync(u => u.id == id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Createbook([FromBody] BooksDto book)
        {
            if (book == null) { return BadRequest(); }
            Author Authorcheck = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == book.Author.ToLower());
            Publisher Publishercheck = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == book.Publisher.ToLower());
            if (Authorcheck == null || Publishercheck == null) { return BadRequest(); }
            Books bookinsert=_mapper.Map<Books>(book);
            await _BooksRepository.CreateAsync(bookinsert);
            return StatusCode(201);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "Deletebookcheck")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Deletebookcheck(int id)
        {
            if (id == 0) { return BadRequest(); }
            Books bookcheck = await _BooksRepository.GetAsync(u => u.id == id);
            if (bookcheck == null) { return NotFound(); }
            await _BooksRepository.RemoveAsync(bookcheck);
            return Ok();
        }

        [HttpPut("{id:int}", Name = "UpdateBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Books book)
        {
            if (book == null || (id != book.id)) { return BadRequest(); }
            Books bookcheck = await _BooksRepository.GetAsync(u => u.id == id, false);
            if (bookcheck == null) { return BadRequest(); }
            Author Authorcheck = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == book.Author.ToLower());
            Publisher Publishercheck = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == book.Publisher.ToLower());
            if (Authorcheck == null || Publishercheck == null) { return BadRequest(); }
            await _BooksRepository.UpdateAsync(book);
            return Ok();
        }
    }
}
