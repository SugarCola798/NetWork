using MobaServer.Net;
using System;

namespace MobaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("启动服务器... ...");
            NetSystemInit();

            Console.ReadLine();//读取用户的一行输入
        }

        public static USocket uSocket;
        static void NetSystemInit() {
            uSocket = new USocket(DispatchNetEvent);
            Debug.Log("网络系统初始化完成!");
        }

        static void DispatchNetEvent(BufferEntity buffer) { 
            //进行报文分发
            
            //给客户端回复信息
        }
    }
}
