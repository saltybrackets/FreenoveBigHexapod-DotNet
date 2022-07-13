namespace FreenoveBigHexapod.Client
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Net.Sockets;
    using UnityEngine;
    using UnityEngine.UI;


    public class HexapodVideoStream
    {
        public string Host = "192.168.1.108";
        public int Port = 8002;

        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private BinaryReader binaryReader;
        private ConcurrentQueue<byte[]> dataQueue = new ConcurrentQueue<byte[]>();

        #region Properties
        public bool SocketReady { get; private set; }  = false;
        #endregion


        public bool StartStreaming()
        {
            try
            {
                this.tcpClient = new TcpClient(this.Host, this.Port);
                this.networkStream = this.tcpClient.GetStream();
                this.binaryReader = new BinaryReader(this.networkStream);
                this.SocketReady = true;
                
                Debug.Log("Video stream connected");
                
                return true;
            }
            
            catch (Exception e)
            {
                Debug.LogError($"Video socket error: {e}");
            }

            return false;
        }


        public void StopStreaming()
        {
            if (!this.SocketReady)
                return;

            
            this.tcpClient.Close();
            this.SocketReady = false;
            
            Debug.Log("Video stream disconnected");
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

        public Texture2D GetPicture()
        {
            Texture2D texture2D = new Texture2D(1, 1);
            byte[] data;
            if (this.dataQueue.Count > 0
                && this.dataQueue.TryDequeue(out data))
            {
                texture2D.LoadImage(data);
                texture2D.Apply();
                return texture2D;
            }

            return null;
        }
    }
}