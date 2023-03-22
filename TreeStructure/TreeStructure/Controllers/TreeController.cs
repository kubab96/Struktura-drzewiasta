using Microsoft.AspNetCore.Mvc;
using TreeStructure.Models;
using TreeStructure.Services;

namespace TreeStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private readonly ITreeService _service;

        public TreeController(ITreeService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<NodeDTO>> GetAll([FromQuery] NodeQuery query)
        {
            var nodes = _service.GetAll(query);
            if (nodes == null)
            {
                return NotFound();
            }
            return Ok(nodes);
        }

        //[HttpGet("{id}")]
        //public ActionResult<List<NodeDTO>> GetById(int id, [FromQuery] NodeQuery query)
        //{
        //    var node = _service.GetById(id, query);
        //    if (node.Count() == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(node);
        //}

        [HttpPost]
        public ActionResult<Node> Create(NodeUpsertDTO nodeUpsertDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var node = _service.Create(nodeUpsertDTO);
            return Created($"api/tree/{node.Id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult<Node> Update(int id, NodeUpsertDTO nodeUpsertDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _service.Update(id, nodeUpsertDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}/move/{newParentId}")]
        public IActionResult MoveTreeNode(int id, int newParentId)
        {
            _service.MoveTreeNode(id, newParentId);

            return NoContent();
        }
    }
}
