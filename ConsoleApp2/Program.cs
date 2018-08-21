using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp2
{
    class Program 
    {
        static void Main(string[] args)
        {
            CreateXML("fuel.csv");
            QueryXML("fuel.xml");
            
        }

        private static void QueryXML(string path)
        {
            var document = XDocument.Load(path);
            var query =
                    document.Element("Cars").Elements("Car")
                            .Where(e => e.Attribute("Name").Value == "MUSTANG");

            foreach (var item in query)
            {
                Console.WriteLine(item.Attribute("Name").Value + "      :" + item.Attribute("Combined").Value);
            }

        }

        private static void CreateXML(string path)
        {
            var CsvElements = ProcessFile(path);
            var document = new XDocument();
            var cars = new XElement("Cars",
                from element in CsvElements
                select new XElement("Car",
                                         new XAttribute("Name", element.Name),
                                         new XAttribute("Combined", element.Combined))
                );
            document.Add(cars);
            document.Save("fuel.xml");

        }

        private static List<Car> ProcessFile(string path)
        {

            return
                File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .Select(Car.ParseFromCsv).ToList();
        }
    }
}
