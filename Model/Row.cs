using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.Model
{
   public class Row
    {
        private List<IBlock> _members=new List<IBlock>();

        public List<IBlock> Members
        {
            get { return _members; }
            set { _members = value; }
        }

    }
}
