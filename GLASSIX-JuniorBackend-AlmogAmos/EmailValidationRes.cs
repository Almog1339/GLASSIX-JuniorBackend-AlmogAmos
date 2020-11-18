using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GLASSIX_JuniorBackend_AlmogAmos
{
    class EmailValidationRes
    {
        public bool Format { get; set; }
        public string Domain { get; set; }
        public bool Disposable { get; set; }
        public bool Dns { get; set; }
        public bool Whitelist { get; set; }

        public static async Task<EmailValidationRes> GetEmailValidationAsync(string email)
        {
            /*
             * This is an async function that get a string (represent an email address)
             * and check if the string is a valid email using the disify API.
             * Once we got a JSON back we deserialize the result to an EmailValidationRel class object.
             */
            EmailValidationRes validationRes = new EmailValidationRes();
            _ =await Task.Run(async() =>
            {
                string emailValidatorApi = "https://disify.com/api/email/";
                emailValidatorApi += email;
                HttpClient _httpClient = new HttpClient();
                string httpMessage = await _httpClient.GetStringAsync(emailValidatorApi);
                validationRes = JsonConvert.DeserializeObject<EmailValidationRes>(httpMessage);
                return validationRes;
            });
            return validationRes;
        }


        public static string Opening()
        {
            /*
             * This function is made to show the opening of the application.
             * It get a user email and move it to the GetEmailValidationAsync function.
             * Once our answer back from the GetEmailValidationAsync function, 
             * we check if the Format parameter is true if so we return the email (since its valid) if not we return an Error.
             */
            Console.WriteLine("---------------------------------------Glassix----------------------------------------");
            Console.Write("Please enter your email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Checking for a valid email...");
            Task<EmailValidationRes> emailValidation = GetEmailValidationAsync(email);
            return (emailValidation.Result.Format == true) ? email : "Error";
        }

        public static void ConnectToSmpt(DadJoke joke, string email)
        {
            /*
             * This function get an DadJoke object and an email address, 
             * we first check the joke status -> we only contiue if the joke status is equel to 200
             * then we try to connect to the SMTP server, due to security reason the user need to enter the username and the password.
             * if the username and the password is correct we compose the email and send our dad joke.
             */
            if (joke.status == 200)
            {
                try
                {
                    Console.WriteLine("------------------------------------SMTP--------------------------------------");
                    Console.WriteLine("Connecting to SMTP server");
                    SmtpClient SmtpServer = new SmtpClient("glassix-hmail.westeurope.cloudapp.azure.com");
                    SmtpServer.Port = 587;
                    Console.WriteLine("--------------------------------Credentials-----------------------------------");
                    Console.Write("Please enter your userName for the SMTP server: ");
                    string userName = Console.ReadLine();
                    Console.Write("Please enter your password: ");
                    string passwd = Console.ReadLine();
                    Console.WriteLine("------------------------------------------------------------------------------");
                    SmtpServer.Credentials = new NetworkCredential(userName, passwd);

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(userName)
                    };

                    mail.To.Add(email);
                    mail.Subject = "Dad Joke";
                    mail.Body = joke.joke;

                    SmtpServer.Send(mail);
                    Console.WriteLine("Done, you may want to check your spam email folder");

                    Console.WriteLine("Thank you for evaluating me,\n" + "Have an awesome day! :) ");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Almog Amos~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~052-950-4350~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~Almog1339@Gmail.com~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Thread.Sleep(5000);
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went worng with the SMTP server....\nPlease try again later");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Opps... Something went worng with the joke");
                Console.WriteLine("Please try again later");
                Thread.Sleep(3000);
                Console.Clear();
            }
        }

    }
}
