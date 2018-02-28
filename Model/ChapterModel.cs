using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
    public class ChapterModel : ViewModelBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

    }
}
