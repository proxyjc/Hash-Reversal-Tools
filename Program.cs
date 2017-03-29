using System;
using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Hash_Reversal_Functions;

namespace Hash_Reversal_Tool_v2
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            // Classes

            // Lists
            List<bool> char_position_determined = new List<bool>();
            List<char> test_string = new List<char> { };
            List<int> indexer = new List<int> { };

            // Variables

            // Integers
            int beginAt;
            int times_operation_completed = 0;

            // Strings
            string input;
            string inputHash = "";
            string hash = "";
            string hashOutput = "";
            string questionInput = "";
            string formatted_hash = "";
            // Introduction
            Console.Write("Hello there! Welcome to Hash Reversal Tool v2. First and foremost you should know that this program was released online free of charge. " +
                "This program is meant for either A) educational use or B) to detect collisions between two or more strings. " +
                "Usage of this program for mal intent is", Console.ForegroundColor = ConsoleColor.DarkGreen);
            Console.Write(" NOT ", Console.ForegroundColor = ConsoleColor.Red);
            Console.Write("condoned by it's creator nor his affiliates.", Console.ForegroundColor = ConsoleColor.DarkGreen);
            Console.ReadLine();
            Console.Clear();
            while (formatted_hash.Length != 40)
            {
                Console.WriteLine("Enter in your hash:", Console.ForegroundColor = ConsoleColor.White);
                inputHash = Console.ReadLine();
                formatted_hash = hashPrep(inputHash, formatted_hash);
                if (inputHash.Length == 40) continue;
                else Console.WriteLine("Sorry! The string you inputted is not the length of a SHA1 hash! please try again...\n");
            }

            while (!(questionInput.ToLower().Equals("n") || questionInput.ToLower().Equals("y")))
            {
                Console.WriteLine("Do you know any of the letters in your target phrase? (y/n)");
                questionInput = Console.ReadLine();
                if (questionInput == "y")
                {
                    Console.WriteLine("Perfect! Type in the letters you know in the correct spaces. Fill in any letters you do not know with a \"?\"");
                    input = Console.ReadLine();
                    foreach (char individual_character in input.ToCharArray())
                    {
                        test_string.Add(individual_character);
                    }
                    test_string = hashCharSetup(test_string, indexer);
                }
                else if (questionInput == "n")
                {
                    Console.WriteLine("This process could take you a few centuries to complete. I'll save you the time and exit.");

                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                    Console.Clear();
                }
            }
            Console.WriteLine("String Length = " + test_string.Count);
            beginAt = indexer[0];
            while (hashOutput != formatted_hash)
            {
                test_string[beginAt]++;
                StringBuilder sb = new StringBuilder();
                if (test_string[beginAt] == '{')
                {
                    test_string[beginAt] = 'a';
                    test_string[indexer[1]]++;
                    if (!(indexer.Count >= 3))
                    {
                        for (int i = 2; i < indexer.Count; i++)
                        {
                            if ((test_string[i] > 'z') && (test_string[i] != (indexer.Count - 1)))
                            {
                                test_string[indexer[i]]++;
                            }
                        }
                    }
                }
                hashOutput = Hashes.SHA1_Managed_Hash(test_string);
                foreach (char character in test_string)
                {
                    sb.Append(character);
                }
                hash = sb.ToString();
                Console.WriteLine(hash);
                times_operation_completed++;
            }
            Console.Clear();
            string[] lines = { "--- " + times_operation_completed + " Total Operations ---", "--- Your hash was: " + hashOutput + " ---", "String: " + hash };
            foreach (string message in lines)
            {
                Console.WriteLine(message);
                Thread.Sleep(50);
            }
            Console.WriteLine("\nWrite results to .txt file? (y/n)");
            questionInput = Console.ReadLine().ToLower();
            while (questionInput != "n" && questionInput != "y")
            {
                Console.WriteLine("Invalid Input");
                questionInput = Console.ReadLine().ToLower();
                Console.Clear();
            }
            if (questionInput == "y")
            {
                file_writer(lines);
                Console.WriteLine("All done!");
            }
            Console.WriteLine("Thank you for using this program! Any feedback is appreciated.");
            Console.ReadLine();
        }

        // Helper Functions:
        static void file_writer(string[] lines)
        {
            string sUsername; // Username as string.
            char[] username = WindowsIdentity.GetCurrent().Name.ToCharArray(); // Returns NetworkName\username
            int[] name_index = { 0, 0 }; // { start int, finish int}
            for (int i = 0; i < username.Length; i++) // This for loop gets the position of the \, and makes the start position one after.
            {
                if (username[i] == '\\')
                {
                    name_index[0] = ++i;
                    name_index[1] = username.Length;
                    break;
                }
            }
            StringBuilder name_builder = new StringBuilder();
            for (int i = name_index[0]; i < name_index[1]; i++) // This for loop takes everything after that \.
            {
                name_builder.Append(username[i]);
            }
            sUsername = name_builder.ToString(); // Joins it all together
            string file_location = "";
            try
            {
                file_location = "C:\\Users\\" + sUsername + "\\Desktop\\hashresults.txt"; // user is always c:\users\username\ and the rest will make a text file for the hash results.
            }
            catch (Exception)
            {
                Console.WriteLine("Save error. Quitting application...");
                Environment.Exit(0);
            }
            Console.ReadLine();
            File.WriteAllLines(@file_location, lines); // Will write to that location.
        }
        public static List<char> hashCharSetup(List<char> c, List<int> indexer)
        {
            for (int i = (c.Count - 1); i > -1; i--)
            {
                if (c[i] == '?')
                {
                    indexer.Add(i);
                    c[i] = 'a';
                }
            }
            return c;
        }
        public static string hashPrep(string inputString, string outputHash)
        {
            // The purpose of this function is to make the hash lowercase and remove any unwanted characters.
            // All of this converts the string to a char[].
            char[] hashArray = inputString.ToCharArray();
            // Removes anything thats not a letter or number. Mainly used for getting rid of dashes and underscores.
            for (int i = 0; i < hashArray.Length; i++)
            {
                if (!((hashArray[i] > 64 && hashArray[i] < 91) || (hashArray[i] > 96 && hashArray[i] < 123) || (hashArray[i] > 46 && hashArray[i] < 58)))
                // The above line checks to see whether the character in the current index is within A to Z or a to z in decimal value.
                {
                    hashArray[i] = ' ';
                    // If it isn't, the program makes it a space.
                }
            }
            // The next bit of code removes the spaces.
            StringBuilder sb = new StringBuilder();
            foreach (char current_char in hashArray)
            {
                if (current_char != ' ')
                {
                    sb.Append(current_char);
                }
            }
            // This takes the output of the foreach loop and gives it as output:
            outputHash = sb.ToString().ToLower();

            if (inputString != outputHash)
            {
                Console.WriteLine("Formatted Hash: " + outputHash);
                Console.ReadLine();
            }
            return outputHash;
        }
    }
}
/*
    whitealice:
    1636ac52c8c5b926255ffa09e72522649f1fa6e4
*/
