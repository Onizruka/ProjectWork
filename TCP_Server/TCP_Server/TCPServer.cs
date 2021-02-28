using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Server
{
    class TCPServer
    {
        private static TcpListener TcpListener;
        private static Thread Thread;
        public static List<ClientObject> clientObjects = new List<ClientObject>();
        public static List<ClientAnswerObject> clientAnswerObjects = new List<ClientAnswerObject>();
        public static void Start_TCPServer()
        {
            TcpListener = new TcpListener(IPAddress.Any, 1470);
            TcpListener.Start();
            ThreadStart threadStart = new ThreadStart(AcceptClient);
            Thread = new Thread(threadStart);
            Thread.Start();
        }

        private static void AcceptClient()
        {
            while (true)
            {
                if (TcpListener.Pending())
                {

                }
                else
                {
                    TcpClient tcpClient = TcpListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ServerDoSomething, tcpClient);
                }
            }
        }

        private static void ServerDoSomething(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            string ip = tcpClient.Client.RemoteEndPoint.ToString();
            if (tcpClient == null)
            {
                return;
            }
            ClientObject clientObject = new ClientObject()
            {
                ip = ip,
                timeconnect = DateTime.Now
            };
            clientObjects.Add(clientObject);
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                byte[] vs1 = Encoding.UTF8.GetBytes("Hello World");
                networkStream.Write(vs1, 0, vs1.Length);
                int byteRead;
                while (true)
                {
                    byteRead = 0;
                    try
                    {
                        byte[] vs = new byte[1024];
                        byteRead = networkStream.Read(vs, 0, vs.Length);
                        if (byteRead > 0)
                        {
                            string s = Encoding.UTF8.GetString(vs);
                            s = s.Substring(0, s.IndexOf('\0'));
                            Console.WriteLine(s);
                            if (s.Contains("313233343536"))
                            {
                                vs1 = Encoding.UTF8.GetBytes("ปลาหยุด");
                                networkStream.Write(vs1, 0, vs1.Length);
                            }
                            if (s.Contains("363534333231"))
                            {
                                vs1 = Encoding.UTF8.GetBytes("ปลาวิด");
                                networkStream.Write(vs1, 0, vs1.Length);
                            }
                            else
                            {
                                vs1 = Encoding.UTF8.GetBytes("Answer wrong");
                                networkStream.Write(vs1, 0, vs1.Length);
                            }
                            ClientAnswerObject clientAnswerObject = new ClientAnswerObject()
                            {
                                ip = ip,
                                time = DateTime.Now,
                                data = s
                            };
                            clientAnswerObjects.Insert(0, clientAnswerObject);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        break;
                    }
                    if (byteRead == 0)
                    {
                        break;
                    }
                }
                ClientObject clientObject1 = clientObjects.Find(x => x.ip == tcpClient.Client.RemoteEndPoint.ToString());
                if (clientObject1 != null)
                {
                    clientObjects.Remove(clientObject1);
                }
            }
        }

        public static void StopThread()
        {
            TcpListener.Stop();
            Thread.Abort();
        }
    }
}
