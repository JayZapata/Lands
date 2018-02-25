using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.ViewModels
{
    using Models;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class LandViewModel:BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Borders> borders;
        
        #endregion


        #region Properties
        public Land Land
        {
            get;
            set;
        }
        #endregion

        public ObservableCollection<Borders> Borders
        {
            get { return this.borders; }
            set { this.SetValue(ref this.borders, value); }
        }

        #region Constructors

        public LandViewModel(Land land)
        {
            this.Land = land;
            this.LoadBorders();
        }

        #endregion

        #region Methods
        private void LoadBorders()
        {
            this.Borders = new ObservableCollection<Borders>();
            foreach (var border in this.Land.Borders)
            {
                var land = MainViewModel.GetInstance().LandsList.
                                         Where(l => l.Alpha3Code == border).
                                         FirstOrDefault();

                if (land != null)
                {
                    this.Borders.Add(new Borders
                    {
                        Code = land.Alpha3Code,
                        Name = land.Name,
                    });
                }
            }
        }
        #endregion
    }
}
