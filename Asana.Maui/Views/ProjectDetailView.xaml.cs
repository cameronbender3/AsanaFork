using Asana.Library.Models;
using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(projectId), "projectId")]
public partial class ProjectDetailView : ContentPage
{
	public ProjectDetailView()
	{
		InitializeComponent();
	}

    public int projectId { get; set; }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ProjectViewModel)?.AddOrUpdateProject();
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ProjectViewModel(projectId);
    }
}