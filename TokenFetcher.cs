using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//read text file token.token


namespace LanguageLearnBot_.NET
{
    internal class TokenFetcher
    {
        public static string? Token { get; private set; }
        static public void OpenTokenFile()
        {
            string pathToToken = @"token.token";

            try
            {
                var token = System.IO.File.ReadAllText(pathToToken, Encoding.UTF8);
                //Console.WriteLine($"Token is: {token}");
                

                TokenFetcher.Token = token;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error token file is not found"); // Action after the exception is caught  
                Console.WriteLine(e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}

