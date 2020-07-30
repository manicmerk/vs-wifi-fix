using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

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
                Console.Write("\n Please input On, Off, Log, Help or Quit? ");

                string outcome = GetUserInput();

                Console.WriteLine("\n " + outcome + "? Press ENTER to continue.");
                Console.ReadLine();


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
                        if (useRecent == "YES" && outcome == "ON")
                        {
                            string adapter = mostRecentWifi;
                            Console.WriteLine("You can now detect wireless routers.");
                            ToggleOn(adapter);
                            // Pretty sure I could make a method for all of these log statements, and possibly log in a simpler fashion... but this works, not DRY, but it works.
                            string log = DateTime.Now.ToString() + " Turned WiFi Connectivity On for " + adapter + "\r\n";
                            File.AppendAllText("ActionLog.txt", log);
                        }
                        else if (useRecent == "YES" && outcome == "OFF")
                        {
                            string adapter = mostRecentWifi;
                            Console.WriteLine("You are connected to wireless but unable to detect new routers.");
                            ToggleOff(adapter);
                            string log = DateTime.Now.ToString() + " Turned WiFi Connectivity Off for " + adapter + "\r\n";
                            File.AppendAllText("ActionLog.txt", log);
                        }
                        else if (useRecent == "NO")
                        {
                            Console.Write("\n Enter your Wi-Fi adapter name here: ");
                            string adapter = GetUserInput();
                            File.WriteAllText("MostRecentWifiLog.txt", adapter);
                            if (outcome == "ON")
                            {
                                Console.WriteLine("You can now detect wireless routers.");
                                ToggleOn(adapter);
                                string log = DateTime.Now.ToString() + " Turned WiFi Connectivity On for " + adapter + "\r\n";
                                File.AppendAllText("ActionLog.txt", log);
                            }
                            else if (outcome == "OFF")
                            {
                                ToggleOff(adapter);
                                Console.WriteLine("You are connected to wireless but unable to detect new routers.");
                                string log = DateTime.Now.ToString() + " Turned WiFi Connectivity Off " + adapter + "\r\n";
                                File.AppendAllText("ActionLog.txt", log);
                            }
                        }
                    }
                    else
                    {
                        Console.Write("\n Enter your Wi-Fi adapter name here: ");
                        string adapter = GetUserInput();
                        File.WriteAllText("MostRecentWifiLog.txt", adapter);
                        if (outcome == "ON")
                        {
                            Console.WriteLine("You can now detect wireless routers.");
                            ToggleOn(adapter);
                            string log = DateTime.Now.ToString() + " Turned WiFi Connectivity On for " + adapter + "\r\n";
                            File.AppendAllText("ActionLog.txt", log);
                        }
                        else if (outcome == "OFF")
                        {
                            ToggleOff(adapter);
                            Console.WriteLine("You are connected to wireless but unable to detect new routers.");
                            string log = DateTime.Now.ToString() + " Turned WiFi Connectivity Off " + adapter + "\r\n";
                            File.AppendAllText("ActionLog.txt", log);
                        }
                    }



                    // Currently stuck with the adapter variable being outside of the scope of my preceeding if/else statement but having trouble identifying how to bring them within the correct scope, or moving the adapter variable effectively outside of the if/else block?



                }

                else if (outcome == "HELP")
                {

                    string currentDirectory = Directory.GetCurrentDirectory();
                    DirectoryInfo directory = new DirectoryInfo(currentDirectory);
                    // \" required to encapsulate the directory because /c was seeing a space in the directory as end of command and was failing. \" allows directory to be read as full string and fires code correctly.
                    string img = "/c \"" + directory.FullName + "\\Help.JPG\"";
                    Process.Start("CMD.exe", img);

                    string log = DateTime.Now.ToString() + " You consulted the HELP file. \r\n";
                    File.AppendAllText("ActionLog.txt", log);
                }

                else if (outcome == "LOG")
                {
                    string actionLog = File.ReadAllText("ActionLog.txt");
                    Console.WriteLine(actionLog);
                }

                else if (outcome == "QUIT")
                {
                    Console.WriteLine("\n Goodbye!");
                    Environment.Exit(0);
                }

                // Better placement for log. Give timestamp and log outcome (option).

                Console.ReadLine();
            }
        }


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
