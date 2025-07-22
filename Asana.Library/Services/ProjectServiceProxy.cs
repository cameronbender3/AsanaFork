using Asana.Library.Models;
using Asana.Maui.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private List<Project> _projectList;

        public List<Project> Projects
        {
            get
            {
                return _projectList.Take(100).ToList();
            }
            private set
            {
                if (value != _projectList)
                {
                    _projectList = value;
                }
            }
        }

        private ProjectServiceProxy()
        {
            // Projects = new List<Project>
            // {
            //     new Project{Id = 1, Name = "Hello", CompletePercent=50},
            //     new Project{Id = 2, Name = "Two", CompletePercent=100},
            //     new Project{Id = 3, Name = "Three", CompletePercent=25}
            // };
            var projectData = new WebRequestHandler().Get("/Project/Expand").Result;
            _projectList = JsonConvert.DeserializeObject<List<Project>>(projectData) ?? new List<Project>();
        }
        private int nextKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(p => p.Id).Max() + 1;
                }
                return 1;
            }
        }

        private static object _lock = new object();
        private static ProjectServiceProxy? instance;
        public static ProjectServiceProxy Current
        {
            get
            {
                lock (_lock) {
                    if (instance == null)
                    {
                        instance = new ProjectServiceProxy();
                    }
                }

                return instance;
            }
        }

        public void ToDosInProject(Project? project)
        {
            if (project != null && project.ToDos != null)
              {
                 project.ToDos.ForEach(Console.WriteLine);
              }
        }
        public void AddOrUpdate(Project? project)
        {
            if (project != null && project.Id == 0)
            {
                project.Id = nextKey;
                _projectList.Add(project);
            }
        }

        public Project? GetById(int id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }

        public void DeleteProject(Project? project)
        {
            if (project == null)
            {
                return;
            }
            _projectList.Remove(project);
        }

        public void DisplayProjects()
        {
            Projects.ForEach(Console.WriteLine);
        }
    }
}
