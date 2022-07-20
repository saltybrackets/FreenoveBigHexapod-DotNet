namespace FreenoveBigHexapod.Client
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net.Sockets;
    
    
    [Serializable]
    public class HexapodVideoStream
    {
        public string Host = "192.168.1.108";
        public int Port = 8002;
        private BinaryReader binaryReader;
        private ConcurrentQueue<byte[]> dataQueue = new ConcurrentQueue<byte[]>();
        private NetworkStream networkStream;

        private TcpClient tcpClient;


        #region Properties
        public bool SocketReady { get; private set; }  = false;
        #endregion


        public byte[] GetFeedData()
        {
            byte[] data;
            if (this.dataQueue.Count > 0
                && this.dataQueue.TryDequeue(out data))
            {
                return data;
            }

            return null;
        }


        public bool StartStreaming()
        {
            try
            {
                this.tcpClient = new TcpClient(this.Host, this.Port);
                this.networkStream = this.tcpClient.GetStream();
                this.binaryReader = new BinaryReader(this.networkStream);
                this.SocketReady = true;
                
                return true;
            }
            
            catch (Exception e)
            {
            }

            return false;
        }


        public void StopStreaming()
        {
            if (!this.SocketReady)
                return;

            
            this.tcpClient.Close();
            this.SocketReady = false;
        }


        public void StreamData()
        {
            if (!this.SocketReady
                || !this.networkStream.DataAvailable)
            {
                return;
            }
            
            try
            {
                if (this.networkStream.CanRead)
                {
                    int length = this.binaryReader.ReadInt32();
                    byte[] data = this.binaryReader.ReadBytes(length);
                    this.dataQueue.Enqueue(data);
                }
            }
            catch
            {
            }
        }
    }
}