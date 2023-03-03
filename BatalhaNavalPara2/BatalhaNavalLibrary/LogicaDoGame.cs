using BatalhaNavalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatalhaNavalLibrary
{
    // Sera uma classe de METODOS, nao de instancias, logo sera static para sempre existir na memoria do programa
    public static class LogicaDoGame
    {
        public static void InicializarPinos(PlayerModel modelo)
        {
            List<string> listaDeletras = new List<string> { "A", "B", "C", "D", "E" };
            List<int> listaDeNumeros = new List<int> { 1, 2, 3, 4, 5 };

            foreach (string letra in listaDeletras)
            {
                foreach (int numero in listaDeNumeros)
                {
                    AdicionarSpotPino(modelo, letra, numero);
                }
            }
        }

        private static void AdicionarSpotPino(PlayerModel modelo, string letra, int numero)
        {
            LocalPinoModel espaco = new LocalPinoModel
            {
                LetraDaPosicao = letra,
                NumeroDaPosicao = numero,
                Status = StatusPino.Vazio
            };

            modelo.Pinos.Add(espaco);
        }

        public static bool FrotaInimigaAfundou(PlayerModel oponente)
        {
            bool jogadorVivo = false;

            foreach (var pino in oponente.LocalidadeNavios)
            {
                if (pino.Status != StatusPino.Afundou)
                {
                    jogadorVivo = true;
                }
            }

            return jogadorVivo;
        }

        public static bool GuardarNavio(PlayerModel jogador, string local)
        {
            //// Supondo que ele digitou A5
            //char[] caracteres = local.ToCharArray();

            //if (caracteres.Length < 2)
            //{
            //    return false;
            //}

            //string letraDoPino = caracteres[0].ToString().ToUpper();
            //string numeroDoPino = caracteres[1].ToString();

            //bool verificaNumero = int.TryParse(numeroDoPino, out int numeroValidoDoPino);

            //if (!verificaNumero)
            //{
            //    return false;
            //}

            //// Agora eu tenho uma letra e um numero valido !

            //foreach (LocalPinoModel pino in jogador.Pinos)
            //{
            //    if (pino.LetraDaPosicao == letraDoPino)
            //    {
            //        if (pino.NumeroDaPosicao == numeroValidoDoPino)
            //        {
            //            // Agora ele acha um pino valido !
            //            switch (pino.Status)
            //            {
            //                case StatusPino.Vazio:
            //                    Console.WriteLine($"Um navio foi colocado em {local} !");
            //                    pino.Status = StatusPino.Navio;
            //                    jogador.LocalidadeNavios.Add(pino);
            //                    break;
            //                default:
            //                    Console.WriteLine("Ja tem navio aqui caralho !");
            //                    return false;
            //            }
            //        }
            //    }
            //}

            //// Limpando os pinos, ja que eles estao salvos em NaviosGuardados !

            //foreach (var pino in jogador.Pinos)
            //{
            //    pino.Status = StatusPino.Vazio;
            //}

            //return true;

            bool output = false;

            (string letra, int numero) = DividirTiro(local);

            bool verificaLocalValido = ValidarPino(jogador, letra, numero);
            bool pinoComVaga = ValidarLocalNavio(jogador, letra, numero);

            if (verificaLocalValido && pinoComVaga)
            {
                jogador.LocalidadeNavios.Add(new LocalPinoModel
                {
                    LetraDaPosicao = letra.ToUpper(),
                    NumeroDaPosicao = numero,
                    Status = StatusPino.Navio
                });

                output = true;
            }

            return output;
        }

        public static bool VerificarEntradaValida(string tiro)
        {
            bool output = true;

            if (tiro.Length != 2)
            {
                output = false;
            }

            return output;
        }

        private static bool ValidarLocalNavio(PlayerModel jogador, string letra, int numero)
        {
            bool localValido = true;

            foreach (var navio in jogador.LocalidadeNavios)
            {
                if (navio.LetraDaPosicao == letra.ToUpper() && navio.NumeroDaPosicao == numero)
                {
                    localValido = false;
                    Console.WriteLine("JA TEM NAVIO AQUI CARALHO !");
                }
            }

            return localValido;
        }

        private static bool ValidarPino(PlayerModel jogador, string letra, int numero)
        {
            bool localValido = false;

            foreach (var navio in jogador.Pinos)
            {
                if (navio.LetraDaPosicao == letra.ToUpper() && navio.NumeroDaPosicao == numero)
                {
                    localValido = true;
                    Console.WriteLine("Local encontrado nos pinos !");
                }
            }

            return localValido;
        }

        public static void MarcarPonto(PlayerModel playerAtivo, string letra, int numero, bool acertouTiro)
        {
            foreach (var pino in playerAtivo.Pinos)
            {
                if (pino.LetraDaPosicao == letra && pino.NumeroDaPosicao == numero)
                {
                    if (acertouTiro)
                    {
                        pino.Status = StatusPino.Acertou;
                    }
                    else
                    {
                        pino.Status = StatusPino.Errou;
                    }
                }
            }
        }

        public static bool DeterminarResultadoTiro(PlayerModel oponente, string letra, int numero)
        {
            bool acertouTiro = false;

            foreach (var navio in oponente.LocalidadeNavios)
            {
                if (navio.LetraDaPosicao == letra.ToUpper() && navio.NumeroDaPosicao == numero)
                {
                    acertouTiro = true;
                    navio.Status = StatusPino.Afundou;
                    Console.WriteLine($"Voce afundou {navio.LetraDaPosicao}{navio.NumeroDaPosicao} ! Enter p continuar");
                    Console.ReadLine();
                }
            }

            return acertouTiro;
        }

        public static bool ValidarTiro(PlayerModel playerAtivo, string letra, int numero)
        {
            bool tiroValido = false;

            foreach (var pino in playerAtivo.Pinos)
            {
                if (pino.LetraDaPosicao == letra.ToUpper() && pino.NumeroDaPosicao == numero)
                {
                    if (pino.Status == StatusPino.Vazio)
                    {
                        return true;
                    }
                }
            }

            return tiroValido;
        }

        public static (string letra, int numero) DividirTiro(string tiro)
        {
            string letraRetorno = string.Empty;
            int numeroRetorno = 0;

            char[] tiroArray = tiro.ToCharArray();

            letraRetorno = tiroArray[0].ToString().ToUpper();
            numeroRetorno = int.Parse(tiroArray[1].ToString());

            return (letraRetorno, numeroRetorno);
        }

        public static int GetContaTiro(PlayerModel player)
        {
            int numeroTiros = 0;

            foreach (var tiro in player.Pinos)
            {
                if (tiro.Status != StatusPino.Vazio)
                {
                    numeroTiros++;
                }
            }

            return numeroTiros;
        }
    }
}
