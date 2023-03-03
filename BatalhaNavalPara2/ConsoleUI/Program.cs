using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatalhaNavalLibrary;
using BatalhaNavalLibrary.Models;


namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            MensagemInicial();

            PlayerModel playerAtual = CriarJogador("Player1");
            ExibirInformacoes(playerAtual);

            PlayerModel oponente = CriarJogador("Player2");
            ExibirInformacoes(oponente);

            PlayerModel vencedor = null;

            do
            {
                Console.Clear();
                Console.WriteLine($"Começando o round do jogador {playerAtual.NomeDoUsuario}! Os pinos serão mostrados !");
                // Mostrar o tabuleiro de pinos
                MostrarPinos(playerAtual);

                GravarTiroDesteJogador(playerAtual, oponente);

                // Determinar se o jogo ja acabou, alguem vira vencedor
                bool inimigoAindaVivo = LogicaDoGame.FrotaInimigaAfundou(oponente);

                Console.WriteLine("Navios do oponente ->");
                foreach (var navio in oponente.LocalidadeNavios)
                {
                    Console.WriteLine(navio.LetraDaPosicao + navio.NumeroDaPosicao + navio.Status);
                }

                if (inimigoAindaVivo)
                {
                    Console.WriteLine($"O inimigo {oponente.NomeDoUsuario} ainda está vivo, o jogo continua !");
                    (playerAtual, oponente) = (oponente, playerAtual);
                }
                else
                {
                    // Usando tupla !
                    // Os valores sao swapados ao mesmo tempo sem risco de um sumir !
                    vencedor = playerAtual;
                }
                Console.WriteLine("Enter para continuar !");
                Console.ReadLine();
            } while (vencedor == null);

            IdentificarVencedor(vencedor);

            Console.ReadLine();
        }

        private static void IdentificarVencedor(PlayerModel vencedor)
        {
            Console.WriteLine($"{vencedor.NomeDoUsuario} venceu o jogo !");
            Console.WriteLine($"{vencedor.NomeDoUsuario} precisou de {LogicaDoGame.GetContaTiro(vencedor)} tiros para vencer !");
        }

        private static void GravarTiroDesteJogador(PlayerModel playerAtivo, PlayerModel oponente)
        {
            bool tiroValido = false;
            string letra = string.Empty;
            int numero = 0;

            do
            {
                string tiro = PedirUmTiro(playerAtivo);
                bool entradaValida = LogicaDoGame.VerificarEntradaValida(tiro);

                if (!entradaValida)
                {
                    Console.WriteLine("ENTRADA INVALIDA SEU MONGOLOIDE ! Enter para continuar");
                    Console.ReadLine();
                    continue;
                }

                (letra, numero) = LogicaDoGame.DividirTiro(tiro);

                tiroValido = LogicaDoGame.ValidarTiro(playerAtivo, letra, numero);

                if (!tiroValido)
                {
                    Console.WriteLine("Tiro invalido !");
                }
                else
                {
                    Console.WriteLine("O Tiro e valido ! Checaremos se acertou algum navio");
                }

            } while (!tiroValido);

            bool acertouTiro = LogicaDoGame.DeterminarResultadoTiro(oponente, letra, numero);

            if (!acertouTiro)
            {
                Console.WriteLine("VOCÊ ERROU ! NAO TINHA NAVIO NESSE PINO TROXAO !");
                Console.WriteLine("Enter para continuar !");
                Console.ReadLine();
            }

            LogicaDoGame.MarcarPonto(playerAtivo, letra, numero, acertouTiro);
        }

        private static string PedirUmTiro(PlayerModel playerAtivo)
        {
            Console.WriteLine($"{playerAtivo.NomeDoUsuario}, aonde você quer atirar ?");
            string output = Console.ReadLine();
            return output;
        }

        private static void MostrarPinos(PlayerModel jogadorAtual)
        {
            string linhaAtual = jogadorAtual.Pinos[0].LetraDaPosicao;

            // Vai mostrar no console, por isso nao fica na ClassLibrary !
            foreach (LocalPinoModel pino in jogadorAtual.Pinos)
            {

                if (linhaAtual != pino.LetraDaPosicao)
                {
                    Console.WriteLine();
                    linhaAtual = pino.LetraDaPosicao;
                }

                if (pino.Status == StatusPino.Vazio)
                {
                    Console.Write($" { pino.LetraDaPosicao }{ pino.NumeroDaPosicao } ");
                }
                else if (pino.Status == StatusPino.Acertou)
                {
                    Console.Write(" X  ");
                }
                else if (pino.Status == StatusPino.Errou)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void ExibirInformacoes(PlayerModel player)
        {
            Console.WriteLine(player.NomeDoUsuario);
            foreach (var pino in player.Pinos)
            {
                Console.Write($"{pino.LetraDaPosicao}{pino.NumeroDaPosicao} ");
            }
            Console.WriteLine();
            foreach (var item in player.LocalidadeNavios)
            {
                Console.WriteLine($" Navio colocados: {item.LetraDaPosicao} {item.NumeroDaPosicao}");
            }
        }

        public static void MensagemInicial()
        {
            Console.WriteLine("Bem vindo ao Batalha Naval para 2 jogadores !");
            Console.WriteLine("Criado por PFMELLO");
            Console.WriteLine();
        }

        public static string PedirInformacao(string mensagem)
        {
            Console.WriteLine(mensagem);
            string output = Console.ReadLine();

            return output;
        }

        public static string PedirNomeUsuario()
        {
            string output = "";
            do
            {
                output = PedirInformacao("Qual o seu nome ?");
            } while (output.Length < 3);


            return output;
        }

        public static PlayerModel CriarJogador(string qualPlayer)
        {
            PlayerModel output = new PlayerModel();
            Console.WriteLine($"Inserindo informacoes do {qualPlayer} !");
            // Pede nome do usuario
            output.NomeDoUsuario = PedirNomeUsuario();

            LogicaDoGame.InicializarPinos(output);

            ColocarNaviosDoJogador(output);

            Console.Clear();

            return output;
        }

        private static void ColocarNaviosDoJogador(PlayerModel jogador)
        {
            do
            {
                Console.WriteLine($"Aonde você quer colocar o navio {jogador.LocalidadeNavios.Count + 1} ?");
                string local = Console.ReadLine();

                if (local.Length != 2)
                {
                    Console.WriteLine("APENAS 2 CARACTERES POR FAVOR ! Enter para continuar");
                    Console.ReadLine();
                    continue;
                }

                bool verificarLocalizacaoValida = LogicaDoGame.GuardarNavio(jogador, local);

                if (!verificarLocalizacaoValida)
                {
                    Console.WriteLine("Essa não é uma localização valida, tenta novamente !");
                }

            } while (jogador.LocalidadeNavios.Count < 5);
        }
    }


}
