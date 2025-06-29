using Asana.Maui.ViewModels;

namespace Asana.Maui.Views;

[QueryProperty(nameof(projectId), "projectId")]
public partial class ToDosInProjectView : ContentPage
{
	public ToDosInProjectView()
	{
		InitializeComponent();
	}

	public int projectId { get; set; }
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

	private void AddToDoClicked(object sender, EventArgs e)
	{
		(BindingContext as ToDosInProjectViewModel)?.AddToDo();
     }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {

    }

	private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
	{
		BindingContext = new ToDosInProjectViewModel(projectId);
		
    }
}