using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Asana.Maui.ViewModels;

public class ToDosInProjectViewModel
{
	public ToDosInProjectViewModel(int projectId)
	{
		Model = ProjectServiceProxy.Current.GetById(projectId) ?? new Project();
		ToDos = new ObservableCollection<ToDoDetailViewModel>(
			Model.ToDos?.Select(t => new ToDoDetailViewModel(t)) ?? new List<ToDoDetailViewModel>()
		);

	}

	public Project? Model { get; set; }
	public ObservableCollection<ToDoDetailViewModel> ToDos { get; set; }

	public List<int> Priorities { get; } = new List<int> { 0, 1, 2, 3, 4 };

	private int newToDoPriority = 4;
        public int NewToDoPriority { 
            get
            {
                return newToDoPriority;
            }
            set
            {
                if (Model != null && newToDoPriority != value)
                {
                   newToDoPriority = value;
                }
            }
        }

	private DateTime newToDoDueDate = DateTime.Today;
	public DateTime NewToDoDueDate
	{
		get => newToDoDueDate;
		set
		{
			if (newToDoDueDate != value)
			{
				newToDoDueDate = value;
				NotifyPropertyChanged();
			}
		}
	}

	public int ProjectId => Model?.Id ?? 0;
	public string ProjectName => Model?.Name ?? string.Empty;
	public string ProjectDescription => Model?.Description ?? string.Empty;
	private string newToDoName = "";
	public string NewToDoName
	{
		get => newToDoName;
		set
		{
			if (newToDoName != value)
			{
				newToDoName = value;
				NotifyPropertyChanged();
			}
		}
	}

	private string newToDoDescription = "";
	public string NewToDoDescription
	{
		get => newToDoDescription;
		set
		{
			if (newToDoDescription != value)
			{
				newToDoDescription = value;
				NotifyPropertyChanged();
			}
		}
	}



	public void AddToDo()
	{
		var newToDo = new ToDo
		{
			Name = NewToDoName,
			Description = NewToDoDescription,
			Priority = NewToDoPriority,
			DueDate = NewToDoDueDate,
			Id = 0,
			IsCompleted = false
		};

		var addedToDo = ToDoServiceProxy.Current.AddOrUpdate(newToDo);

		if (addedToDo != null && !Model.ToDos.Any(t => t.Id == addedToDo.Id))
		{
			Model.ToDos.Add(addedToDo);
			ToDos.Add(new ToDoDetailViewModel(addedToDo));
		}

		NewToDoName = string.Empty;
		NewToDoDescription = string.Empty;
		NewToDoPriority = 4;
		NewToDoDueDate = DateTime.Today;
	}

	public void RefreshPage()
	{
		NotifyPropertyChanged(nameof(ToDos));
		NotifyPropertyChanged(nameof(Model));
	}
	public event PropertyChangedEventHandler? PropertyChanged;

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
