using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro
{
    class TabuleiroExcessao : Exception
    {
        public TabuleiroExcessao(string msg) : base(msg)
        {

        }
    }
}
