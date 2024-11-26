using CommunityToolkit.Mvvm.Input;
using CYD.Models;

namespace CYD.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}