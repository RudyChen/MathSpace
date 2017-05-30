using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
   public class Page
    {
        private List<Row> _rows=new List<Row>();

        public List<Row> Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

    }
}
