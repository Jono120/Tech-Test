using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tech_Test
{
    // Take the provided text file as an input file containing a collection of strings 
    // Parse the strings as follows: 
    // - Establish a parent/child relationship based on ID value (i.e. 1.1 is a child of 1, 23.4.2 is a child of 23.4, etc.) 

    // Each item should have at least the following properties: 
    // - ID(unique and mandatory)
    // - Name(mandatory)
    // - Location(mandatory)
    // - Children(optional)

    // If a Name or Location isn’t provided it should inherit from its parent 

    // Include whatever testing you deem appropriate 
    // Write the resulting object out to a single JSON file 
    // Handle and log any errors to a txt file 

    //  Inputs 
    // - Collection of delimited strings

    // Read the input.txt file and parse the data into a dictionary of Item objects
    // Run the program and check the output.json file is created
    // Check the output.json file contains the expected data

    //  Outputs
    // - JSON file containing a collection of objects

    public class Program
    {
        static void Main(string[] args)
        {
            
            string inputFile = "input-text.txt";
            string outputFile = "output.json";

            try
            {
                List<Item> items = ParseInputFile(inputFile);
                string jsonOutput = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(outputFile, jsonOutput);
                Console.WriteLine("Output file created successfully.");
            }
            catch (Exception ex)
            {
                string errorFile = "error.txt";
                File.WriteAllText(errorFile, ex.ToString());
                Console.WriteLine("An error occurred. Details have been logged to error.txt.");
            }
        }

       static List<Item> ParseInputFile(string inputFile)
        {
            List<Item> items = new List<Item>();
            Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

            foreach (string line in File.ReadAllLines(inputFile))
            {
                string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 2)
                {
                    throw new Exception($"Invalid input format: {line}");
                }

                string id = parts[0].Trim();
                string name = parts[1].Trim();
                string location = parts.Length > 2 ? parts[2].Trim() : null;

                Item item = new Item(id, name, location);
                itemDict[id] = item;

                if (id.Contains("."))
                {
                    string parentId = id.Substring(0, id.LastIndexOf('.'));
                    if (!itemDict.ContainsKey(parentId))
                    {
                        throw new Exception($"Invalid parent ID: {parentId}");
                    }
                    itemDict[parentId].Children.Add(item);
                }
                else
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }

    internal class Item
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Item> Children { get; set; }

        public Item(string id, string name, string location)
        {
            ID = id;
            Name = name;
            Location = location;
            Children = new List<Item>();
        }
    }
}