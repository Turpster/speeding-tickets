using System.IO;
using System.Net;
using System.Xml.Linq;

namespace Main
{
    /*
     * CONFIG FORMAT
     *
     * <regnum>
     * <name>
     * <e-mail>
     * 
     */
    
    public class RegistratorDatabase
    {
        private string fileloc; 
        public RegistratorDatabase(string fileloc)
        {
            this.fileloc = fileloc;
        }

        public Registrator GetRegistratorRecord(int index)
        {
            // TODO Catch Index Out Of Range Exception
            
            FileStream fileStream = new FileStream(fileloc, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fileStream);
            
            for (int it = 0; it<3*index; it++) reader.ReadLine();

            string[] record = new string[3];

            for (int i = 0; i < 3; i++)
            {
                record[i] = reader.ReadLine();
            }
            reader.Close();
            return new Registrator(record[0], record[1], record[2]);
        }
        
        public void SaveRegistrator(Registrator registrator)
        {
            
            FileStream fileStream = new FileStream(fileloc, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fileStream);
     
            writer.Write(registrator.StructuredData);
            writer.Flush();
            
            writer.Close();
        }

        public int NumberOfRegistrators()
        {
            FileStream fileStream = new FileStream(fileloc, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(fileStream);

            int numOfNextLine = 0; 
            foreach (char character in reader.ReadToEnd())
            {
                if (character == '\n')
                {
                    numOfNextLine++;
                }
            }
            
            reader.Close();
            return numOfNextLine / 3;
        }

        public Registrator[] GetRegistrators()
        {
            int numberOfRegs = NumberOfRegistrators();
            Registrator[] registrators = new Registrator[numberOfRegs];

            for (int i = 0; i < numberOfRegs; i++)
            {
                registrators[i] = GetRegistratorRecord(i);
            }

            return registrators;
        }
    }
}