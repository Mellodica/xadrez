using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez.xadrez
{
    class Cavalo : Peca
    {

        public Cavalo(Tabuleiro tab, Cor cor) : base(tab, cor)
        {

        }


        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
        }
        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);


            //NO
            pos.definirValores(posicao.Linha - 1, posicao.Coluna - 2);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {
                
                    mat[pos.Linha, pos.Coluna] = true;
           
                }

            //NE
            pos.definirValores(posicao.Linha - 2, posicao.Coluna - 1);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }

            //SE
            pos.definirValores(posicao.Linha - 2, posicao.Coluna - 1);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }


            //SO
            pos.definirValores(posicao.Linha - 2, posicao.Coluna + 1);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }

            pos.definirValores(posicao.Linha - 1, posicao.Coluna + 2);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }

            pos.definirValores(posicao.Linha + 1, posicao.Coluna + 2);
            if (tab.posicaoValida(pos) && podeMover(pos))
            {

                mat[pos.Linha, pos.Coluna] = true;

            }

            pos.definirValores(posicao.Linha + 2, posicao.Coluna + 1);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }

            pos.definirValores(posicao.Linha + 2, posicao.Coluna - 1);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }


            pos.definirValores(posicao.Linha + 1, posicao.Coluna - 2);
                if (tab.posicaoValida(pos) && podeMover(pos))
                {

                    mat[pos.Linha, pos.Coluna] = true;

                }


            return mat;
        }

        public override string ToString()
        {
            return "C";
        }
    }
}
