﻿using Asana.Maui.ViewModels;

namespace Asana.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private void AddNewProjectClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ProjectDetails");
        }

        private void EditProjectClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedProjectId ?? 0;
            Shell.Current.GoToAsync($"//ProjectDetails?projectId={selectedId}");  
        }

        private void DeleteProjectClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteProject();
        }

        private void OpenProjectClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedProjectId ?? 0;
            Shell.Current.GoToAsync($"//ToDosInProjectDetails?projectId={selectedId}");

        }
        private void AddNewClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ToDoDetails");
        }
        private void EditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainPageViewModel)?.SelectedToDoId ?? 0;
            Shell.Current.GoToAsync($"//ToDoDetails?toDoId={selectedId}");
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.DeleteToDo();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainPageViewModel)?.RefreshPage();
        }

        private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {

        }

        private void InLineDeleteClicked(object sender, EventArgs e)
        {
            (BindingContext as MainPageViewModel)?.RefreshPage();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
        }
    }

}
