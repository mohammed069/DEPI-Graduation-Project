using System;
using System.Collections.Generic;
using System.Text;

namespace QuizApp.Services {
    public static class MainServices {
        public static string ReadPassword() {
            string password = "";

            while(true) {
                var key = Console.ReadKey(intercept: true);

                if(key.Key == ConsoleKey.Enter) {
                    Console.WriteLine();
                    return password;
                }

                if(key.Key == ConsoleKey.Backspace) {
                    if(password.Length > 0)
                        password = password[..^1];

                    continue;
                }

                if(!char.IsControl(key.KeyChar))
                    password += key.KeyChar;
            }
        }
    }
}
