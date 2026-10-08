using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class PasswordManager
    {
        private string website;
        private string password;
        private string filePath;

        
        public PasswordManager(string path)
        {
            filePath = path;

           
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        
        public void SetWebsite(string w)
        {
            website = w;
        }

        public string GetWebsite()
        {
            return website;
        }

        public void SetPassword(string p)
        {
            password = p;
        }

        public string GetPassword()
        {
            return password;
        }

        
        public void CreatePassword()
        {
            Console.Write("Enter website/service name: ");
            website = Console.ReadLine();

            Console.Write("Do you want to enter your own password? (Y/N): ");
            string choice = Console.ReadLine().ToUpper();

            if (choice == "Y")
            {
                Console.Write("Enter your password: ");
                password = Console.ReadLine();

                if (password.Length < 12)
                {
                    Console.WriteLine("Warning: Your password is short. Consider using at least 12 characters");
                    return;
                }

            }
            else

            {
                password = GeneratePassword();
                Console.WriteLine("Generated password for " + website + ": " + password);
            }

            string encrypted = Encrypt(password);
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(website + "|" + encrypted);
            }
            
            
                Console.WriteLine("Password saved successfully!\n");
            
        }

  
        public void RetrievePassword()
        {
            Console.Write("Enter website/service name to find: ");
            string search = Console.ReadLine();
            bool found = false;

         
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2 && parts[0].Equals(search, StringComparison.OrdinalIgnoreCase))
                {
                    string decrypted = Decrypt(parts[1]);
                    Console.WriteLine("Password for " + search + ": " + decrypted);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("Service not found.");
            }
        }

        
        private string GeneratePassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@£$%^&*()_-+=<>?/'~#:;{}[].,";
            Random rnd = new Random();
            string result = "";

            for (int i = 0; i < 12; i++)
            {
                result += chars[rnd.Next(chars.Length)];
            }

            return result;
        }

        private string Encrypt(string text)
        {
            string result = "";
            foreach (char c in text)
            {
                result += (char)(c + 2);
            }
            return result;
        }

        private string Decrypt(string text)
        {
            string result = "";
            foreach (char c in text)
            {
                result += (char)(c - 2);
            }
            return result;
        }

        
        public void Menu()
        {
            int choice = 0;
            while (choice != 3)
            {
                Console.WriteLine("=== Password Manager ===");
                Console.WriteLine("1. Create Password");
                Console.WriteLine("2. Retrieve Password");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option(1-3): ");
                choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1)
                    CreatePassword();
                else if (choice == 2)
                    RetrievePassword();
                else if (choice == 3)
                    Console.WriteLine("Goodbye!");
                else
                    Console.WriteLine("Invalid choice, please try again.");
            }
        }
    }
}
