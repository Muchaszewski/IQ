using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creator
{
    public class DisplayNameWeightList
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _Weight;

        public int Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
    }
}
