using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrackFE.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public DelegateCommand Btn1Command { get; }


        public MainViewModel()
        {
            Btn1Command = new DelegateCommand(Btn1);
        }




        private void Btn1()
        {
            // Your logic here
        }


    }
}
