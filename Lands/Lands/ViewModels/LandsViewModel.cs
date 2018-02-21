namespace Lands.ViewModels
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Services;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;

    public class LandsViewModel:BaseViewModel
    {
        #region Services
        private ApiService apiServices;
        #endregion

        #region Attributes
        private ObservableCollection<Land> lands;
        private bool isRefreshing;
        private string filter;
        private List<Land> LandsList;
        #endregion

        #region Properties

        public string Filter
        {
            get { return this.filter; }
            set { this.SetValue(ref this.filter, value); }
        }

        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { this.SetValue(ref this.lands, value); }
        }


        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors

        public LandsViewModel()
        {
            this.apiServices = new ApiService();
            this.LoadLands();
        }

        #endregion

        #region Methods

        private async void LoadLands()
        {
            this.IsRefreshing = true;
            var connection = await this.apiServices.CheckConnection();

            if (!connection.IsSuccess)

            {

                this.IsRefreshing = false;

                await Application.Current.MainPage.DisplayAlert(

                    "Error",
                    connection.Message,
                    "Accept");

                await Application.Current.MainPage.Navigation.PopToRootAsync();

                return;

            }

            var response=await this.apiServices.GetList<Land>(
                "http://restcountries.eu/rest/v2/all",
                "/rest",
                "/v2/all");

            if (!response.IsSuccess)
                {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
               }
            this.LandsList = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(this.LandsList);
            this.IsRefreshing = false;
        }

        #endregion

        #region Commands

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            this.IsRefreshing = true;
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<Land>(this.LandsList);
            }
            else
            {
                this.Lands = new ObservableCollection<Land>(
                    this.LandsList.Where(l => l.Name.ToLower().Contains(this.Filter.ToLower())));

            }
            IsRefreshing = false;
        }

        #endregion


    }
}
