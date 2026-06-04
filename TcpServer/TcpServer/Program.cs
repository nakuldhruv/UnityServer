using System.Net;
using System.Net.Sockets;
using System.Text;

var isRunning = true;
var port = 8888;

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();
Console.WriteLine($"TCP server started, listening on port {port}. Press Ctrl+C to stop.");

// Handle Ctrl+C signal to stop the server gracefully
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    isRunning = false;
    listener.Stop();
    Console.WriteLine("Server is stopping...");
};

// Main loop: continuously accept client connections
while (isRunning)
{
    try
    {
        var client = await listener.AcceptTcpClientAsync();
        // Start an independent task for each client to handle concurrency
        _ = Task.Run(() => HandleClientAsync(client));
    }
    catch (ObjectDisposedException) when (!isRunning)
    {
        // This exception is expected when the listener is stopped normally, ignore it
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while accepting connection: {ex.Message}");
    }
}

Console.WriteLine("Server has stopped.");

// Logic to handle a single client
async Task HandleClientAsync(TcpClient client)
{
    var clientEndpoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
    Console.WriteLine($"[{DateTime.Now}] Client connected: {clientEndpoint}");

    try
    {
        await using var stream = client.GetStream();
        var buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received from {clientEndpoint}: {receivedMessage}");

            // Echo: send back the received message with a prefix
            var response = $"Server echo: {receivedMessage}";
            var responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error handling client {clientEndpoint}: {ex.Message}");
    }
    finally
    {
        client.Close();
        Console.WriteLine($"[{DateTime.Now}] Client disconnected: {clientEndpoint}");
    }
}