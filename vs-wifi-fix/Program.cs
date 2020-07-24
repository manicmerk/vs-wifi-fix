using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace Wi_Fi_Drop_Fix
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {// Initiate CMD prompt to display autoconfig setting causing lag bombs.
                string destination = "/c netsh wlan show settings";
                Process.Start("CMD.exe", destination);
                Console.ReadLine();

                Console.WriteLine("\n Do you want to toggle AutoConfig setting for your Wi-Fi Adapter?");
                Console.Write("\n Please input On, Off, Help or Quit? ");

                string outcome = GetUserInput();

                Console.WriteLine("\n " + outcome + "? Press ENTER to continue.");
                Console.ReadLine();

                // Code I was attempting to use to read and write a file. Simplified solution was File.WriteAllText()/File.ReadAllText()
                //string currentDirectory = Directory.GetCurrentDirectory();
                //DirectoryInfo directory = new DirectoryInfo(currentDirectory);
                //var fileName = Path.Combine(directory.FullName, "MostRecentWifiLog.txt");
                //var mostRecentWifi = ReadFile(fileName);
                //Console.WriteLine(mostRecentWifi);

                //string mostRecentWifi = File.ReadAllText("MostRecentWifiLog.txt");
                //Console.WriteLine(mostRecentWifi);



                if (outcome == "ON" || outcome == "OFF")
                {
                    if (File.Exists("MostRecentWifiLog.txt"))

                    {
                        var mostRecentWifi = File.ReadAllText("MostRecentWifiLog.txt");
                        Console.WriteLine("\n Your most recently used WiFi was " + mostRecentWifi + "? Would you like to use it again?");
                        Console.Write("\n Yes or No?: ");
                        string useRecent = GetUserInput();
                        Console.WriteLine("\n " + useRecent + "? Hit ENTER to confirm.");
                        Console.ReadLine();
                        string adapter = mostRecentWifi;
                    }
                    else
                    {
                        Console.Write("\n Enter your Wi-Fi adapter name here: ");
                        string adapter = GetUserInput();
                        File.WriteAllText("MostRecentWifiLog.txt", adapter);
                    }                  

                    if (outcome == "ON")
                    {
                        Console.WriteLine("You can now detect wireless routers.");
                        ToggleOn(adapter);
                        // Log event to log file.
                    }
                    else if (outcome == "OFF")
                    {
                        ToggleOff(adapter);
                        Console.WriteLine("You are connected to wireless but unable to detect new routers.");
                        // Log event to log file.
                    }
                    

                }

                // Can't figure out a way to make the console open the picture in image viewer?? Non essential to completion, fix if time permits.
                //else if (outcome == "HELP")
                //{
                //    string currentDirectory = Directory.GetCurrentDirectory();
                //    DirectoryInfo directory = new DirectoryInfo(currentDirectory);
                //    string helpFile = Path.Combine(directory + "\\Capture.JPG");
                //    Process.Start("CMD.exe", helpFile);

                //    File.Open("capture.jpg", FileMode.Open, FileAccess.ReadWrite);
                //}

                else if (outcome == "QUIT") 
                {
                    Console.WriteLine("\n Goodbye!");
                    Environment.Exit(0);
                }

                // Better placement for log. Give timestamp and log outcome (option).

                Console.ReadLine();
            }
        }

        // Method to read most recent wi-fi name. Not necessary due to File.WriteAllText()
        //public static string ReadFile(string fileName)
        //{
        //    using(var reader = new StreamReader(fileName))
        //    {
        //        return reader.ReadLine();
        //    }
        //}

        // Method to return answer for user outcome interface.
        static public string GetUserInput()
        {
            string outcome = Console.ReadLine();
            return outcome.ToUpper();
        }

        // Toggle Autoconfig setting on or off.
        static void ToggleOn(string adapter)
        {
            string toggleOn = "/c netsh wlan set autoconfig enabled=yes interface=\"" + adapter + "\"";
            Process.Start("CMD.exe", toggleOn);
        }

        static void ToggleOff(string adapter)
        {
            string toggleOff = "/c netsh wlan set autoconfig enabled=no interface=\"" + adapter + "\"";
            Process.Start("CMD.exe", toggleOff);
        }

    }
}
