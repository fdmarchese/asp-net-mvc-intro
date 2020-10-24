using System.Security.Cryptography;
using System.Text;

namespace usando_seguridad.Extensions
{
    public static class StringExtensions
    {
        public static byte[] Encriptar(this string data) =>
            new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(data));

        /*
         * Análogo a:
        
        public static byte[] Encriptar(this string data)
        {
            return new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(data));
        }
         */
    }
}