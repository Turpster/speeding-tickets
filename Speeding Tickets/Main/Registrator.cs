using System.Runtime.InteropServices.WindowsRuntime;

namespace Main
{
    public class Registrator
    {
        public readonly string RegNumber;
        public readonly string Name;
        public readonly string Email;

        public string StructuredData => RegNumber + "\n" + Name + "\n" + Email + "\n";

        public Registrator(string regNumber, string name, string email)
        {
            RegNumber = regNumber;
            Name = name;
            Email = email;
        }
    }
}