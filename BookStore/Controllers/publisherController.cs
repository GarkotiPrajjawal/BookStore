using BookStore.Models;
using BookStore.Repository.iRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("Publisher")]
    public class publisherController : ControllerBase
    {
        private readonly iPublisherRepository _PublisherRepository;
        public publisherController(iPublisherRepository PublisherRepository)
        {
            _PublisherRepository = PublisherRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Getpublishers()
        {
            var publisherslist = await _PublisherRepository.GetAllAsync();
            if (publisherslist == null)
            {
                return BadRequest();
            }
            return Ok(publisherslist);
        }

        [HttpGet("{name}", Name = "GetPublisherByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Getpublisherbyid(string name)
        {
            if (name == null) { return BadRequest(); }
           
            Publisher publisher = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] Publisher publisher)
        {
            Publisher publishercheck = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == publisher.Name.ToLower());
            if (publisher == null || publishercheck!=null) { return BadRequest(); }
            await _PublisherRepository.CreateAsync(publisher);
            return StatusCode(201);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{name}", Name = "DeletePublisher")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePublisher(String name)
        {
            if (name == null) { return BadRequest(); }
            Publisher publishercheck = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if(publishercheck==null) { return NotFound();}
            await _PublisherRepository.RemoveAsync(publishercheck);
            return Ok();
        }

        //[HttpPut("{name}", Name = "UpdatePublisher")]
        [HttpPut("{name}", Name = "UpdatePublisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePublisher(String name, [FromBody] Publisher publisher)
        {
            if (publisher == null || (name.ToLower() != publisher.Name.ToLower())) { return BadRequest(); }
            Publisher publishercheck = await _PublisherRepository.GetAsync(u => u.Name.ToLower() == name.ToLower(), false);
            if(publishercheck==null) { return BadRequest(); }
            await _PublisherRepository.UpdateAsync(publisher);
            return Ok();
        }
    }
    }
