﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    class Torre : Peca
    {

        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
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
            //Esquerda
            pos.definirValores(posicao.Linha, posicao.Coluna - 1);

            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha, pos.Coluna - 1);
            }

            //Direita
            pos.definirValores(posicao.Linha, posicao.Coluna + 1);

            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha, pos.Coluna + 1);
            }

            //Acima
            pos.definirValores(posicao.Linha - 1, posicao.Coluna);

            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha - 1, pos.Coluna);
            }

            //Abaixo
            pos.definirValores(posicao.Linha + 1, posicao.Coluna);

            while (tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha + 1, pos.Coluna);
            }
            return mat;
        }

        public override string ToString()
        {
            return "T";
        }


    }
}
