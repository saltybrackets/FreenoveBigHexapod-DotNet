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

        [SerializeField]
        private HexapodHead head;

        public bool IsReady
        {
            get { return this.client.SocketReady; }
        }


        public void Awake()
        {
            this.client = new HexapodClient();
            this.movement = new HexapodMovement(this.client);
            this.head = new HexapodHead(this.client);
        }


        /// <summary>
        /// Move head up and down by a given amount.
        /// Negative moves head down.
        /// Positive moves head up.
        /// </summary>
        /// <param name="amount">Amount to move head in degrees.</param>
        public void PitchHead(int amount)
        {
            this.head.Pitch(amount);
        }


        /// <summary>
        /// Move head left and right.
        /// Negative moves head right.
        /// Positive moves head left.
        /// </summary>
        /// <param name="amount">Amount to move head in degrees.</param>
        public void RollHead(int amount)
        {
            this.head.Roll(amount);
        }


        public void Rotate(Rotation rotationDirection)
        {
            this.movement.Rotate(rotationDirection);
        }


        public void Move(int x, int y)
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