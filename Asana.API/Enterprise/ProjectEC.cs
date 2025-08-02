using Asana.API.Database;
using Asana.Library.Models;

namespace Asana.API.Enterprise
{
    public class ProjectEC
    {
        public IEnumerable<Project>? Get()
        {
            return new MsSqlContext().Projects.Take(100);
        }

        public Project? GetById(int id)
        {
            return new MsSqlContext().ExpandSingle(id);
        }

        public IEnumerable<Project> ExpandProjects()
        {
            return new MsSqlContext().Expanded();
        }

        public Project? AddOrUpdate(Project? project)
        {
            if (project == null)
            {
                return project;
            }
            new MsSqlContext().AddOrUpdateProject(project);
            return project;
        }

        public int Delete(int id)
        {
            if (id > 0)
            {
                return new MsSqlContext().DeleteProject(id);
            }
            return -1;
        }
    }
}
