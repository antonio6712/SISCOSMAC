using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SISCOSMAC.AlgoritmoSeguridad
{
    public class Hash
    {
        private static readonly int TamañoSal = 32;

        public static byte[] GenerarSal()
        {
            using (var NumeroGeneracionAleatoria = new RNGCryptoServiceProvider())
            {
                var NumeroAleatorio = new byte[TamañoSal];
                NumeroGeneracionAleatoria.GetBytes(NumeroAleatorio);
                return NumeroAleatorio;
            }
        }

        public static byte[] Combinar(byte[] primero, byte[] segundo)
        {
            var resultado = new byte[primero.Length + segundo.Length];

            Buffer.BlockCopy(primero, 0, resultado, 0, primero.Length);
            Buffer.BlockCopy(segundo, 0, resultado, primero.Length, segundo.Length);

            return resultado;
        }

        public static byte[] HashPasswordConSal(byte[] CadenaASerHashed, byte[] sal)
        {
            using (var Sha256 = SHA256.Create())
            {
                return Sha256.ComputeHash(Combinar(CadenaASerHashed, sal));
            }
        }

        public static byte[] PasswordABytes(byte[] password, byte[] sal)
        {
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(Combinar(password, sal));
        }

        public static string ObtenerMD5(string password)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();

            ASCIIEncoding enconding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(enconding.GetBytes(password));

            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }
            return sb.ToString();
        }
    }
}
