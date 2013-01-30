using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderView.ProtocolBuffer;
using System.IO;
namespace PBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("test.proto"))
            {
                Protocol c = new Protocol(sr.ReadToEnd());
                foreach (MessageType mt in c.MessageTypes)
                {
                    Console.WriteLine(" message: " + mt.Name);
                    foreach (Field field in mt.rules)
                    {
                        Console.WriteLine();
                        foreach (String prop in field.Properties)
                        {
                            Console.Write(prop + " ");
                        }

                    }

                    Console.WriteLine("== END==");
                }
                Console.ReadLine();
            }
        }
    }
}
