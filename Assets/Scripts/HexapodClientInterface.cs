namespace FreenoveBigHexapod.Client.Unity
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;


    public class HexapodClientInterface : MonoBehaviour
    {
        public UnityEvent Connecting;
        public UnityEvent Connected;
        public UnityEvent Disconnected;
        public HexapodDataEvent DataReceived;

        
        [SerializeField]
        private HexapodClient client;
        
        [SerializeField]
        private HexapodMovement movement;

        public bool IsReady
        {
            get { return this.client.SocketReady; }
        }


        public void Awake()
        {
            this.client = new HexapodClient();
            this.movement = new HexapodMovement(this.client);
        }


        public void Move(float x, float y)
        {
            this.movement.Move(x, y);
        }


        public void Stop()
        {
            this.movement.Stop();
        }
        

        public void ToggleConnection()
        {
            if (!this.client.SocketReady)
                Connect();
            else
                Disconnect();
        }
        

        private void Connect()
        {
            if (this.client.SocketReady)
                return;
            
            this.Connecting.Invoke();
            
            if (this.client.OpenSocket())
            {
                this.Connected.Invoke();
            }
            else
            {
                this.Disconnected.Invoke();
            }
        }


        private void Disconnect()
        {
            this.client.CloseSocket();
            this.Disconnected.Invoke();
        }
    }
}