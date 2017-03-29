using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Hash_Reversal_Functions
{
    public class Hashes
    {
        public static string SHA1_Managed_Hash(List<char> c)
        {
            string hash = "";
            string output = "";
            StringBuilder sb = new StringBuilder();
            foreach (char literal in c)
            {
                sb.Append(literal);
            }
            hash = sb.ToString();
            var hasher = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(hash));
            output = string.Join("", hasher.Select(b => b.ToString("x2")).ToArray());
            return output;
        }
        // Sha256 function for later use.
        public static string SHA256_Managed_Hash(List<char> c)
        {
            string hash = "";
            string output = "";
            StringBuilder sb = new StringBuilder();
            foreach (char literal in c)
            {
                sb.Append(literal);
            }
            hash = sb.ToString();
            var hasher = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(hash));
            output = string.Join("", hasher.Select(b => b.ToString("x2")).ToArray());
            return output;
        }
    }
}