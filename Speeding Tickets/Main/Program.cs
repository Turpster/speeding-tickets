using System;
using System.Globalization;
using System.IO;

/*
 * 1. Produce a test file with 5 Registration Numbers, 5 people’s names and 5 e-mail addresses.
 * 2. User enters a Registration number of a car
 * 3. User enters a time that the car entered an average speed limit zone of 50MPH
 * 4. User enters a time that the car leaves the average speed limit zone (2 miles later)
 * 5. The program works out the speed of the car
 * 6. If the car was speeding – it will query the test file to retrieve the customer’s name and e-mail address and 
 * produce an e-mail alerting them of how far above the speed limit they were. Outputting the contents of the 
 * e-mail to a new file.
 */

namespace Main
{
    public class Program
    {
        private RegistratorDatabase database;
        
        public static void Main(string[] args)
        {
            new Program();
        }

        public void WriteRecords()
        {
            for (int i = 0; i < 5; i++)
                database.SaveRegistrator(new Registrator(AskUserString("Regnum"), AskUserString("Name"), AskUserString("Email")));
        }

        public Program()
//        {
//            database = new RegistratorDatabase(@"./database.txt");
//
//            Registrator registrator = database.GetRegistratorRecord(0);
//            Console.WriteLine(registrator.RegNumber);
//            Console.WriteLine(registrator.Name);
//            Console.WriteLine(registrator.Email);
//            
//        }
//        public void sProgram()
        {   
            database = new RegistratorDatabase(@"./database.txt");
            
            string regnumber = AskUserString("Reg Num");
            DateTime exceededAbove = AskUserDateTime("Enter time your vehicle entered above 50mph");
            DateTime exceededBelow = AskUserDateTime("Enter time your vehicle entered below 50mph"); // 2miles

            TimeSpan span = exceededBelow - exceededAbove;

            // s = d/t
            double speedMPH = 2D / span.TotalHours;

            Console.WriteLine("{0} / {1} = {2}", 2D, span.TotalHours, speedMPH);
            
            if (speedMPH > 50)
            {
                Registrator[] registrators = database.GetRegistrators();

                Registrator targetRegistrator = null;
                
                foreach (Registrator registrator in registrators)
                {
                    Console.WriteLine(registrator.RegNumber);
                    Console.WriteLine(registrator.Name);
                    Console.WriteLine(registrator.Email);
                    Console.WriteLine('\n');
                    
                    if (string.Equals(registrator.RegNumber, regnumber, StringComparison.OrdinalIgnoreCase))
                    {
                        targetRegistrator = registrator;
                        break;
                    }
                }

                if (targetRegistrator != null)
                {
                    Directory.CreateDirectory(@"./emails/" + targetRegistrator.Email);
                    int id = 1;

                    while (File.Exists(@"./emails/" + targetRegistrator.Email + "/" + id + ".txt"))
                    {
                        id++;
                    }

                    StreamWriter fileStream = File.CreateText(@"./emails/" + targetRegistrator.Email + "/" + id + ".txt");
                    
                    fileStream.Write(String.Format("Hello {0},\n" +
                                                   "You were {1:0.00}MPH above the speed limit!", targetRegistrator.Name, speedMPH - 50));
                    
                    fileStream.Flush();
                    fileStream.Close();
                }
                else
                {
                    Console.WriteLine("Registration Number does not exist in our database.");
                }
            }
            else
            {
                Console.WriteLine("You're fine");
            }
        }
        
        public static DateTime AskUserDateTime(string demand = "Input a datetime")
        {
            demand += " (dd/MM/yyyy h:mm:ss tt eg. 21/10/2001 7:25:10 am)";
            while (true)
            {
                try
                {
                    string date = AskUserString(demand);
                    return DateTime.ParseExact(date, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format, make sure you are using a valid date format.");
                }
            }
        }
        
        public static string AskUserString(string demand = "Input")
        {
            bool running = true;
            string input = "";

            while (running)
            {
                Console.Write(demand + ": ");

                input = Console.ReadLine();
                if (input.Length == 0)
                {
                    Console.WriteLine("Please write letters...");
                }
                else running = false;
            }

            return input;
        }
    }
}