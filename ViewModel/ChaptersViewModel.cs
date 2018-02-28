using GalaSoft.MvvmLight;
using MathSpace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.ViewModel
{
    public class ChaptersViewModel : ViewModelBase
    {
        public ChaptersViewModel()
        {
            for (int i = 0; i < 12; i++)
            {
                allChapters.Add(new ChapterModel() {Name="Chapter"+i.ToString() });
            }
        }

        private List<ChapterModel> allChapters=new List<ChapterModel>();

        public List<ChapterModel> AllChapters
        {
            get { return allChapters; }
            set { Set(ref allChapters, value); }
        }
    }
}
