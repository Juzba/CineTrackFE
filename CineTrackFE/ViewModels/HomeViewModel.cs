using CineTrackFE.AppServises;
using System.Threading.Tasks;

namespace CineTrackFE.ViewModels;

public class HomeViewModel : BindableBase
{
    private readonly IApiService _apiService;
    public AsyncDelegateCommand ClickCommand { get; }



    public HomeViewModel(IApiService apiService)
    {
        ClickCommand = new AsyncDelegateCommand(Click);
        _apiService = apiService;
    }




    private async Task Click()
    {
        //await _apiService.GetAsync();
    }









}
