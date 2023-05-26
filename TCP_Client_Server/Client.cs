using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TCP_Client_Server
{
    public class Client
    {
        int port = 8888;
        string hostname = "192.168.0.14";

        Socket socket;

        bool connected = false;
        string errorString = "Нет ошибок";
        string resultString = "";
        int errorCode = 0;

        public Client(string _hostname, int _port)
        {
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ServerConnectAsync(_hostname, _port);
                connected = true;
                errorCode = 0;
                resultString = $"Подключено к {_hostname}:{_port}";
            }
            catch (SocketException)
            {
                connected = false;
                errorCode = 1;
                errorString = $"Не удалось установить подключение к {_hostname}. ErrorCode: {errorCode}";
            }
            Console.WriteLine(errorString);
        }

        public async void SendMessage(Socket socket)
        {
            using var stream = new NetworkStream(socket);
            // отправляем сообщение для отправки
            var message = $"GET / HTTP/1.1\r\nHost: {hostname}\r\nConnection: Close\r\n\r\n";
            // кодируем его в массив байт
            var data = Encoding.UTF8.GetBytes(message);
            // отправляем массив байт на сервер 
            await stream.WriteAsync(data);

            // буфер для получения данных
            var responseData = new byte[512];
            // получаем данные
            var bytes = await stream.ReadAsync(responseData);
            // преобразуем полученные данные в строку
            string response = Encoding.UTF8.GetString(responseData, 0, bytes);
            // выводим данные на консоль
            Console.WriteLine(response);

            Console.WriteLine($"Данные отправлены на сервер {hostname}");
            stream.Close();
        }

        private async void ServerConnectAsync(string _hostname, int _port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // пытаемся подключиться используя URL-адрес и порт
            await socket.ConnectAsync(_hostname, _port);

            Console.WriteLine($"Адрес подключения {socket.RemoteEndPoint}");
            Console.WriteLine($"Адрес приложения {socket.LocalEndPoint}");

        }
        
        private void ServerDisconnectAsync(Socket socket)
        {
            socket.DisconnectAsync(new SocketAsyncEventArgs(true)); // отключаемся
        }

        ~Client()
        {
            if (connected)
            {
                this.ServerDisconnectAsync(socket);
                connected = false;
            }
        }
    }
}
