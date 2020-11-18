using System;
using System.Threading.Tasks;

namespace GLASSIX_JuniorBackend_AlmogAmos
{
    class Program
    {
        public static void Main(string[] args)
        {
            StartTest();
        }

        public static void StartTest()
        {
            /*
             * StartTest is a simple function that bind all the functionality together.
             * EmailValidationRes.Opening() -> is a static function that show us the opening screen,
             *                                 and validate the email address the user inserted.
             * DadJoke.GetJokeAsync(); -> is a static async function that fetch a random dad joke.
             * EmailValidationRes.ConnectToSmpt(DadJoke, string) -> static function that connect to the SMPT server, 
             *                                                      this function get an object of DadJoke and an email address (string).
             */
            string emailAddress = EmailValidationRes.Opening();
            
            if (!emailAddress.Equals("Error"))
            {
                Task<DadJoke> joke = DadJoke.GetJokeAsync();
                EmailValidationRes.ConnectToSmpt(joke.Result, emailAddress);
            }
            else
                Console.WriteLine("Your email is not valid please try again later...");
        }
    }
}
