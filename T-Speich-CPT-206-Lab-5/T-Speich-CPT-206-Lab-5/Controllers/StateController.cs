using Microsoft.AspNetCore.Mvc;
using T_Speich_CPT_206_Lab_5.Models;

namespace T_Speich_CPT_206_Lab_5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository repo;

        public StateController(IStateRepository repo)
        {
            this.repo = repo;
        }

        // route: /state || /state?name={name}
        [HttpGet("ByName/{name}", Name = nameof(GetStates))]
        [ProducesResponseType(200, Type = typeof(State))]
        public async Task<State> GetStates(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            else //search for the valid name
            {
                name = name.ToUpper();
                return (await repo.RetrieveAllAsync())
                  .Where(customer => customer.State_Name.ToUpper() == name).SingleOrDefault();
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<State>))]
        public async Task<IEnumerable<State>> GetStates()
        {
            return await repo.RetrieveAllAsync();
        }

        // route: /state/{id} || /state?id={id}
        [HttpGet("{id}", Name = nameof(GetState))] 
        [ProducesResponseType(200, Type = typeof(State))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetState(int id)
        {
            State? state = await repo.RetrieveAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        // POST: api/state
        // BODY: State (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(State))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] State state)
        {
            if (state == null)
            {
                return BadRequest();

            }
            State? addedState = await repo.CreateAsync(state);
            if (addedState == null)
            {
                return BadRequest("Repository failed to create state");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetState),
                    routeValues: new { id = addedState.State_ID },
                    value: addedState);
            }
        }
        // PUT: api/state/[id]
        // BODY: State (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] State state)
        {

            if (state == null)
            {
                return BadRequest();

            }
            State? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            await repo.UpdateAsync(id, state);
            return new NoContentResult();
        }

        // DELETE: api/state/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            State? exisiting = await repo.RetrieveAsync(id);
            if (exisiting == null)
            {
                return NotFound();
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult();
            }
            else
            {
                return BadRequest($"State {id} was found but failed to delete!");
            }
        }
    }
}
