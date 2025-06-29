using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Windows.Input;

namespace Asana.Maui.ViewModels;

public class ProjectViewModel
{
	public ProjectViewModel()
	{
		Model = new Project();
		DeleteCommand = new Command(DoDelete);
	}

	public ProjectViewModel(int id)
	{
		Model = ProjectServiceProxy.Current.GetById(id) ?? new Project();
		DeleteCommand = new Command(DoDelete);
	}

	public ProjectViewModel(Project? model)
	{
		Model = model ?? new Project();
		DeleteCommand = new Command(DoDelete);
	}

	public Project? Model { get; set; }
	public ICommand? DeleteCommand { get; set; }

	public void DoDelete()
	{
		ProjectServiceProxy.Current.DeleteProject(Model);
	}

	public void AddOrUpdateProject()
    {
		ProjectServiceProxy.Current.AddOrUpdate(Model);
    }
}

