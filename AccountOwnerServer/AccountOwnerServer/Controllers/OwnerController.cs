using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    public class OwnerController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _repository.Owner.GetAllOwnersAsync();

            _logger.LogInfo($"Returned all owners from database.");

            return Ok(owners);
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById(Guid id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);


            if (owner.IsEmptyObject())
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            _logger.LogInfo($"Returned owner with id: {id}");
            return Ok(owner);
        }

        [HttpGet("{id}/account")]
        public async Task<IActionResult> GetOwnerWithDetails(Guid id)
        {
            var owner = await _repository.Owner.GetOwnerWithDetailsAsync(id);


            if (owner.IsEmptyObject())
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            _logger.LogInfo($"Returned owner with details for id: {id}");
            return Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody] Owner owner)
        {
            if (owner.IsObjectNull())
            {
                _logger.LogError("Owner object sent from client is null.");
                return BadRequest("Owner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid owner object sent from client.");
                return BadRequest("Invalid model object");
            }

            await _repository.Owner.CreateOwnerAsync(owner);

            return CreatedAtRoute("OwnerById", new {id = owner.Id}, owner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] Owner owner)
        {
            if (owner.IsObjectNull())
            {
                _logger.LogError("Owner object sent from client is null.");
                return BadRequest("Owner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid owner object sent from client.");
                return BadRequest("Invalid model object");
            }

            var dbOwner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (dbOwner.IsEmptyObject())
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.Owner.UpdateOwnerAsync(dbOwner, owner);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);
            if (owner.IsEmptyObject())
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                return NotFound();
            }

            await _repository.Owner.DeleteOwnerAsync(owner);

            return NoContent();
        }
    }
}