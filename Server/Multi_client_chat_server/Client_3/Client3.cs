//Step2. client with nickname

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
    private static int localPort = 0;              // 로컬 포트 (자동 할당)

    private static string nickname;  // 클라이언트의 닉네임

    public static async Task Main(string[] args)
    {
        udpClient = new UdpClient(localPort);
        Console.Write("UDP Chat Client started. Enter your nickname: ");

        // 닉네임 설정
        nickname = Console.ReadLine();
        if (string.IsNullOrEmpty(nickname))
        {
            nickname = "Anonymous";
        }

        await SendMessageAsync(nickname);

        Task receiveTask = ReceiveMessagesAsync();

        await SendMessagesAsync();
    }

    private static async Task SendMessagesAsync()
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

        while (true)
        {
            try
            {
                string message = Console.ReadLine();
                if (message == "/exit")
                {
                    byte[] disconnectMessage = Encoding.UTF8.GetBytes("disconnect");
                    await udpClient.SendAsync(disconnectMessage, disconnectMessage.Length, serverEndPoint);

                    Console.WriteLine("Exit chat");
                    udpClient.Close();
                    break;
                }
                if (!string.IsNullOrEmpty(message))
                {
                    await SendMessageAsync(message);
                    Console.WriteLine($"You: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
            }
        }
    }

    // 서버로부터 메시지를 비동기로 수신하는 메서드
    private static async Task ReceiveMessagesAsync()
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                UdpReceiveResult receivedResult = await udpClient.ReceiveAsync();
                string receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);
                Console.WriteLine(receivedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving message: " + ex.Message);
            }
        }
    }

    private static async Task SendMessageAsync(string message)
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        await udpClient.SendAsync(messageBytes, messageBytes.Length, serverEndPoint);
    }
}
