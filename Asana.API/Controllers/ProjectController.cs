using Asana.API.Enterprise;
using Asana.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Project>? Get() {
            return new ProjectEC().Get();
        }

        [HttpGet("Expand")]
        public IEnumerable<Project>? GetExpand()
        {
            return new ProjectEC().ExpandProjects();
        }
        [HttpGet("{id}")]
        public Project? GetById(int id)
        {
            return new ProjectEC().GetById(id);
        }

        [HttpDelete("{id}")]
        public int? Delete(int id)
        {
            return new ProjectEC().Delete(id);
        }

        [HttpPost]
        public Project? AddOrUpdate([FromBody] Project? project)
        {
            return new ProjectEC().AddOrUpdate(project);
        }
    }
}
