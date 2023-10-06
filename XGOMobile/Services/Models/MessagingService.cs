using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGOMobile.Services.Models
{
    public class MessagingService
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public MessagingService() { }
        #endregion

        #region Methods
        public async Task ShowMessage(string message)
        {
            await Application.Current.MainPage.DisplayAlert("", message, "Cancel");
        }
        #endregion
    }
}
