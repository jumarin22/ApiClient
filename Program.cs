using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleTables;

namespace ApiClient
{
    class Program
    {
        class Fact
        {

            // Example GET output:
            // "type": "cat",
            // "deleted": false,
            // "_id": "5ff06cea9aae76001727849a",
            // "user": "5ff06cd49aae760017278498",
            // "text": "This is a fact, Jack.",
            // "createdAt": "2021-01-02T12:54:02.559Z",
            // "updatedAt": "2021-01-02T12:54:02.559Z",
            // "__v": 0

            [JsonPropertyName("type")]
            public string Type { get; set; }
            [JsonPropertyName("deleted")]
            public bool Deleted { get; set; }
            [JsonPropertyName("_id")]
            public string FactId { get; set; }
            [JsonPropertyName("user")]
            public string UserId { get; set; }
            [JsonPropertyName("text")]
            public string Text { get; set; }
            [JsonPropertyName("createdAt")]
            public DateTime CreatedAt { get; set; }
            [JsonPropertyName("updatedAt")]
            public DateTime UpdatedAt { get; set; }
            [JsonPropertyName("__v")]
            public int Version { get; set; }

        }


        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Create client to send and receive data.
            var client = new HttpClient();

            // Make a `GET` request to the API and get back a *stream* of data.

            string animalString = "";
            string amountString = "";
            string httpString;

            string userChoice;


            //Main menu. 
            var keepRunning = true;
            var firstChoice = true;
            var secondChoice = true;
            while (keepRunning)
            {
                // Ask what animal.
                while (firstChoice)
                {
                    Console.WriteLine("Welcome to Fast Animal Facts!");
                    Console.WriteLine("Choose an animal to receive facts about:");
                    Console.WriteLine("(C)at\n(D)og\n(H)orse\n(S)nail\n(Q)uit");
                    userChoice = Console.ReadLine().ToLower();
                    switch (userChoice)
                    {
                        case "c":
                            animalString = "cat";
                            firstChoice = false;
                            secondChoice = true;
                            break;
                        case "d":
                            animalString = "dog";
                            firstChoice = false;
                            secondChoice = true;
                            break;
                        case "h":
                            animalString = "horse";
                            firstChoice = false;
                            secondChoice = true;
                            break;
                        case "s":
                            animalString = "snail";
                            firstChoice = false;
                            secondChoice = true;
                            break;
                        case "q":
                            keepRunning = false;
                            firstChoice = false;
                            secondChoice = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Sorry, I don't understand.");
                            secondChoice = true;
                            break;
                    }
                }

                while (secondChoice)
                {
                    Console.WriteLine($"Enter the integer number of facts that you like to see about {animalString}s (2 to 500)");
                    amountString = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine($"Here are your {amountString} facts about {animalString}s:");
                    Console.WriteLine();

                    httpString = $"https://cat-fact.herokuapp.com/facts/random?animal_type={animalString}&amount={amountString}";

                    var responseAsStream = await client.GetStreamAsync($"{httpString}");
                    var facts = await JsonSerializer.DeserializeAsync<List<Fact>>(responseAsStream);

                    // Note: Some facts too long to neatly appear in the ConsoleTable.
                    var factCount = 1;

                    foreach (var fact in facts)
                    {
                        Console.WriteLine($"{factCount}: {fact.Text}");
                        Console.WriteLine();
                        factCount++;
                    }

                    secondChoice = false;
                    firstChoice = true;
                }
            }
        }
    }
}
