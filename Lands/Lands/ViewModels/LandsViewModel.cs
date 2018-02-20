namespace Lands.ViewModels
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Services;
    using Xamarin.Forms;

    public class LandsViewModel:BaseViewModel
    {
        #region Services
        private ApiService apiServices;
        #endregion


        #region Attributes
        private ObservableCollection<Land> lands;

        #endregion

        #region Properties

        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { this.SetValue(ref this.lands, value); }
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
            var connection = await this.apiServices.CheckConnection();

            if (!connection.IsSuccess)

            {

                //this.IsRefreshing = false;

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
                //this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
               }
            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);
        }


        #endregion


    }
}
