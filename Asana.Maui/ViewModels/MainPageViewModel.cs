using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ToDoServiceProxy _toDoSvc;
        private ProjectServiceProxy _projectSvc;

        public MainPageViewModel()
        {
            _toDoSvc = ToDoServiceProxy.Current;
            _projectSvc = ProjectServiceProxy.Current;
        }

        public ToDoDetailViewModel SelectedToDo { get; set; }

        public ProjectViewModel SelectedProject { get; set; }
        public ObservableCollection<ToDoDetailViewModel> ToDos
        {
            get
            {
                var toDos = _toDoSvc.ToDos
                        .Select(t => new ToDoDetailViewModel(t));
                if (!IsShowCompleted)
                {
                    toDos = toDos.Where(t => !t?.Model?.IsCompleted ?? false);
                }
                return new ObservableCollection<ToDoDetailViewModel>(toDos);
            }
        }

        public ObservableCollection<ProjectViewModel> Projects
        {
            get
            {
                var projects = _projectSvc.Projects
                        .Select(t => new ProjectViewModel(t));
                if (!IsShowCompletedProject)
                {
                    projects = projects.Where(t => t?.Model?.CompletePercent < 100);
                }
                return new ObservableCollection<ProjectViewModel>(projects);
            }
        }




        public int SelectedToDoId => SelectedToDo?.Model?.Id ?? 0;
        public int SelectedProjectId => SelectedProject?.Model?.Id ?? 0;

        private bool isShowCompletedProject;

        public bool IsShowCompletedProject
        {
            get
            {
                return isShowCompletedProject;
            }
            set
            {
                if (isShowCompletedProject != value)
                {
                    isShowCompletedProject = value;
                    NotifyPropertyChanged(nameof(Projects));
                }
            }
        }
        private bool isShowCompleted;
        public bool IsShowCompleted
        { 
            get
            {
                return isShowCompleted;
            }

            set
            {
                if (isShowCompleted != value)
                {
                    isShowCompleted = value;
                    NotifyPropertyChanged(nameof(ToDos));
                }
            }
        }

        public void DeleteProject()
        {
            if (SelectedProject == null)
            {
                return;
            }
            ProjectServiceProxy.Current.DeleteProject(SelectedProject.Model);
            NotifyPropertyChanged(nameof(Projects));
        }
        public void DeleteToDo()
        {
            if (SelectedToDo == null)
            {
                return;
            }

            ToDoServiceProxy.Current.DeleteToDo(SelectedToDo.Model);
            NotifyPropertyChanged(nameof(ToDos));
        }

        public void RefreshPage()
        {
            NotifyPropertyChanged(nameof(ToDos));
            NotifyPropertyChanged(nameof(Projects));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
