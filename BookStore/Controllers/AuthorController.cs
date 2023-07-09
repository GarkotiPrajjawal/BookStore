using BookStore.Models;
using BookStore.Repository;
using BookStore.Repository.iRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly iAuthorRepository _AuthorRepository;
        public AuthorController(iAuthorRepository AuthorRepository)
        {
            _AuthorRepository = AuthorRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Getpublishers()
        {
            var authorlist = await _AuthorRepository.GetAllAsync();
            if (authorlist == null) 
            {
                return BadRequest();
            }
            return Ok(authorlist);
        }

        [HttpGet("{name}", Name = "GetAuthorByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Getpublisherbyid(string name)
        {
            if (name == null) { return BadRequest(); }

            Author author = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] Author author)
        {
            Author authorcheck = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == author.Name.ToLower());
            if (author == null || authorcheck != null) { return BadRequest(); }
            await _AuthorRepository.CreateAsync(author);
            return StatusCode(201);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{name}", Name = "DeleteAuthor")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePublisher(String name)
        {
            if (name == null) { return BadRequest(); }
            Author authorcheck = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if (authorcheck == null) { return NotFound(); }
            await _AuthorRepository.RemoveAsync(authorcheck);
            return Ok();
        }

        [HttpPut("{name}", Name = "UpdateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePublisher(String name, [FromBody] Author author)
        {
            if (author == null || (name.ToLower() != author.Name.ToLower())) { return BadRequest(); }
            Author authorcheck = await _AuthorRepository.GetAsync(u => u.Name.ToLower() == name.ToLower(),false);
            if (authorcheck == null) { return BadRequest(); }
            await _AuthorRepository.UpdateAsync(author);
            return Ok();
        }
    }
}

   
