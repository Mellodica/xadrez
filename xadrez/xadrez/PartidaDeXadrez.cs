﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez;


namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public bool xeque { get; private set; }

        public Peca vulneravelEnPassant { get; private set; }


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimenta(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            
                //Jogada RoquePequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            //Jogada RoqueGrande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            //Jogada Especial En Pasant

            if(p is xadrez.Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(p.cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }

                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);

                }
            }


            return pecaCapturada;

        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQteMovimentos();
            if(pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }

            tab.colocarPeca(p, origem);

            //Jogada RoquePequeno DESFAZER
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            //Jogada RoqueGrande DESFAZER
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            //Jogada en Passant

            if(p is xadrez.Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if(p.cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);

                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }

        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimenta(origem, destino);
            Peca p = tab.peca(destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExcecao("Você não pode se colocar em Xeque!");
            }

            //JOgadaEspecial Promocao

            if(p is xadrez.Peao)
            {
                if((p.cor == Cor.Branca && destino.Linha == 0) || (p.cor == Cor.Preta && destino.Linha == 7))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new xadrez.Dama(tab, p.cor);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;

            }
            else
            {
                xeque = false;
            }

            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {

                turno++;
                mudaJogador();
            }

            //Peca p = tab.peca(destino);
            //Jogada Especial en passant

            if(p is xadrez.Peao && (destino.Linha == origem.Linha -2 || destino.Linha == origem.Linha + 2)){
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }


        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroExcecao("Não Existe Peça na Posição de Origem Escolhida!");
            }
            if(jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroExcecao("A peça de origem escolhida não é a sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroExcecao("Não Há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroExcecao("Posição de destino inválida");
            }
        }

        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }


        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca X in capturadas)
            {
                if ( X.cor == cor)
                {
                    aux.Add(X);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca X in pecas)
            {
                if(X.cor == cor)
                {
                    aux.Add(X);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }


        private Cor adversaria(Cor cor)
        {
            if(cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca X in pecasEmJogo(cor))
            {
                if (X is Rei)
                {
                    return X;
                }
                
            }
            return null;
        }


        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroExcecao("Não tem Rei da cor" + cor + " no tabuleiro!");
            }
            foreach (Peca X in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = X.movimentosPossiveis();
                if (mat[R.posicao.Linha, R.posicao.Coluna])
                {
                    return true;
                }
                
            }
            return false;
        }


        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca X in pecasEmJogo(cor))
            {
                bool[,] mat = X.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if(mat[i, j])
                        {
                            Posicao origem = X.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimenta(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void colocarPecas()
        {
           
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new xadrez.Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new xadrez.Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new xadrez.Dama(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new xadrez.Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new xadrez.Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('a', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('b', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('c', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('d', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('e', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('f', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('g', 2, new xadrez.Peao(tab, Cor.Branca, this));
            colocarNovaPeca('h', 2, new xadrez.Peao(tab, Cor.Branca, this));


            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('b', 8, new xadrez.Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new xadrez.Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new xadrez.Dama(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new xadrez.Bispo(tab, Cor.Preta));
            colocarNovaPeca('g', 8, new xadrez.Cavalo(tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('a', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('b', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('c', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('d', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('e', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('f', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('g', 7, new xadrez.Peao(tab, Cor.Preta, this));
            colocarNovaPeca('h', 7, new xadrez.Peao(tab, Cor.Preta, this));



        }

    }
}
