namespace FreenoveBigHexapod.Client
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using UnityEngine;

    [Serializable]
    public class HexapodClient
    {
        public string Host = "192.168.1.108";
        public int Port = 5002;
        
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private StreamWriter streamWriter;
        private StreamReader streamReader;

        
        public bool SocketReady { get; private set; }  = false;


        public bool OpenSocket()
        {
            try
            {
                this.tcpClient = new TcpClient(this.Host, this.Port);
                this.networkStream = this.tcpClient.GetStream();
                this.streamWriter = new StreamWriter(this.networkStream);
                this.streamReader = new StreamReader(this.networkStream);
                this.SocketReady = true;
                
                Debug.Log("Connected");
                
                return true;
            }
            
            catch (Exception e)
            {
                Debug.LogError($"Socket error: {e}");
            }

            return false;
        }


        public string ReadFromSocket()
        {
            if (!this.SocketReady)
                return string.Empty;

            return this.networkStream.DataAvailable 
                       ? this.streamReader.ReadLine() 
                       : null;
        }


        public void WriteToSocket(string input)
        {
            if (!this.SocketReady)
                return;

            Debug.Log(input);

            var inputBytes = Encoding.Default.GetBytes($"{input}\n");
            
            this.streamWriter.Write(Encoding.UTF8.GetString(inputBytes));
            this.streamWriter.Flush();
        }


        

        public void CloseSocket()
        {
            if (!this.SocketReady)
                return;

            this.streamWriter.Close();
            this.streamReader.Close();
            this.tcpClient.Close();
            this.SocketReady = false;
            
            Debug.Log("Disconnected");
        }
    } 
}