using CineTrackFE.AppServises;
using CineTrackFE.Common.Events;
using CineTrackFE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrackFE.ViewModels;

public class CatalogViewModel: BindableBase, INavigationAware
{
    private readonly IApiService _apiService;
    private readonly IEventAggregator _eventAggregator;


    private readonly AsyncDelegateCommand OnInitializeAsyncCommand;


    public CatalogViewModel(IApiService apiService, IEventAggregator eventAggregator)
    {
        _apiService = apiService;
        _eventAggregator = eventAggregator;


        OnInitializeAsyncCommand = new AsyncDelegateCommand(OnInitializeAsync);
        OnInitializeAsyncCommand.Execute();

        _eventAggregator.GetEvent<MainViewTitleEvent>().Publish("Catalog Page");
    }



    // I-NAVIGATION-AWARE //
    public bool IsNavigationTarget(NavigationContext navigationContext) => false;
    public void OnNavigatedTo(NavigationContext navigationContext) { }
    public void OnNavigatedFrom(NavigationContext navigationContext) { }



    // ON INITIALIZE //
    private async Task OnInitializeAsync()
    {
        ErrorMessage = null;

        try
        {
            var filmListDb = await _apiService.GetAsync<IEnumerable<Film>>("/api/FilmApi/AllFilms");
            if (filmListDb != null) FilmList = new ObservableCollection<Film>(filmListDb);

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            // chyba
        }




    }


    private ObservableCollection<Film> filmList = [];
    public ObservableCollection<Film> FilmList
    {
        get { return filmList; }
        set { SetProperty(ref filmList, value); }
    }



    private string? errorMessage;
    public string? ErrorMessage
    {
        get { return errorMessage; }
        set { SetProperty(ref errorMessage, value); }
    }



}
