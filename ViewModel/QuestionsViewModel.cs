using GalaSoft.MvvmLight;
using MathSpace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.ViewModel
{
   public class QuestionsViewModel:ViewModelBase
    {
        public QuestionsViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                allQuestions.Add(new QuestionModel() { Name="Question"+i.ToString()});
            }
        }

        private List<QuestionModel> allQuestions=new List<QuestionModel>();

        public List<QuestionModel> AllQuestions
        {
            get { return allQuestions; }
            set { Set(ref allQuestions, value); }
        }

    }
}
