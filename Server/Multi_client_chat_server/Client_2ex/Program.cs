using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class UdpChatClient
{
    private static UdpClient udpClient;
    private static string serverIp = "127.0.0.1";  // 서버 IP
    private static int serverPort = 8080;          // 서버 포트
    private static int localPort = 0;              // 자동포트 할당
    private static string nickname = "Anonymous";  // 기본 닉네임

    public static async Task Main(string[] args)
    {
        // 닉네임 설정
        Console.Write("Enter your nickname: ");
        string inputNickname = Console.ReadLine();
        if (!string.IsNullOrEmpty(inputNickname))
        {
            nickname = inputNickname;
        }

        udpClient = new UdpClient(localPort);
        Console.WriteLine($"UDP Chat Client started as {nickname}... Type messages to send to the server...");

        await SendNicknameAsync();

        Task receiveTask = ReceiveMessagesAsync();

        await SendMessagesAsync();
    }

    private static async Task SendNicknameAsync()
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        byte[] nicknameBytes = Encoding.UTF8.GetBytes(nickname);
        await udpClient.SendAsync(nicknameBytes, nicknameBytes.Length, serverEndPoint);
        Console.WriteLine($"Sent nickname to server: {nickname}");
    }

    private static async Task SendMessagesAsync()
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

        while (true)
        {
            try
            {
                string message = Console.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await udpClient.SendAsync(messageBytes, messageBytes.Length, serverEndPoint);
                    Console.WriteLine($"Sent to server: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
            }
        }
    }

    private static async Task ReceiveMessagesAsync()
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                UdpReceiveResult receivedResult = await udpClient.ReceiveAsync();
                string receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);
                Console.WriteLine($"Server: {receivedMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving message: " + ex.Message);
            }
        }
    }
}
