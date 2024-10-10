using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Print sth. to identify app
        Console.WriteLine("TCP Server:");

        // Instantiate new object of TcpListener class, with port 7
        TcpListener listener = new TcpListener(IPAddress.Any, 7);

        // Start listening for connections
        listener.Start();
        Console.WriteLine("Server is listening on port 7...");

        // Main server loop - this will keep the server running indefinitely
        while (true)
        {
            // Accept a new client connection
            TcpClient socket = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Handle the client in a separate task/thread to allow multiple connections simultaneously
            Task.Run(() => HandleClient(socket));
        }
    }

    // Handle client connections in a separate method
    static void HandleClient(TcpClient socket)
    {
        try
        {
            // Read and write connections (streams)
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message;

            // Keep reading messages until the client disconnects
            while ((message = reader.ReadLine()) != null)
            {

                //Randomizer
                if (message.Contains("Random", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Lets generate random numbers. Input numbers: ");
                    writer.Flush();

                    //Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    //Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');
                    //get elements by index and parse to integer
                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    //generate random number        
                    Random rand = new Random();
                    int randomNumber = rand.Next(Math.Min(num1, num2), Math.Max(num1, num2) + 1);

                    Console.WriteLine(randomNumber);

                }

                // Add
                if (message.Contains("Add", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Lets add two numbers. Input numbers: ");
                    writer.Flush();

                    //Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    //Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');

                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    //addition
                    int result = num1 + num2;

                    Console.WriteLine(result);

                }

                // Subtract
                if (message.Contains("Subtract", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Great. Lets subtract. Input numbers: ");
                    writer.Flush();

                    //Wait for the client to send two numbers
                    string numbers = reader.ReadLine();

                    //Split numbers into two elements in a list
                    string[] splitNumbers = numbers.Split(' ');

                    int num1 = Int32.Parse(splitNumbers[0]);
                    int num2 = Int32.Parse(splitNumbers[1]);

                    //addition
                    int result = num1 - num2;

                    Console.WriteLine(result);

                }

                // If the client sends "Stop", close the connection and break the loop
                if (message.Equals("Stop", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Stop command received. Closing connection.");
                    writer.WriteLine("Goodbye!"); //Send a message back to the client
                    writer.Flush();
                    break; // Exit the loop to close the connection
                }

                // Echo the message back to the client
                //writer.WriteLine(message); // Send the received message back to the client

                // Flush to ensure data is sent immediately
                writer.Flush();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            // Clean up and close the connection
            socket.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}