using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro
{
    class TabuleiroExcecao : Exception
    {
        public TabuleiroExcecao(string msg) : base(msg)
        {

        }
    }
}
