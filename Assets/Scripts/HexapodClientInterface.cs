namespace FreenoveBigHexapod.Client.Unity
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;


    public class HexapodClientInterface : MonoBehaviour
    {
        /// <summary>
        /// Raised when attempting connection.
        /// </summary>
        public UnityEvent Connecting;
        
        /// <summary>
        /// Raised once successfully connected.
        /// </summary>
        public UnityEvent Connected;
        
        /// <summary>
        /// Raised when disconnected.
        /// </summary>
        public UnityEvent Disconnected;
        
        /// <summary>
        /// Raised when data is received from server.
        /// </summary>
        public HexapodDataEvent DataReceived;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float commandLatency;

        [SerializeField]
        private HexapodClient client;

        [SerializeField]
        private HexapodMovement movement;

        [SerializeField]
        private HexapodHead head;

        [SerializeField]
        private HexapodPosing posing;

        private float commandLatencyTimer;

        /// <summary>
        /// Returns true if the hexapod client is connected to a hexapod server.
        /// </summary>
        public bool IsReady
        {
            get { return this.client.SocketReady; }
        }


        public void Awake()
        {
            this.client = new HexapodClient();
            this.movement = new HexapodMovement(this.client);
            this.head = new HexapodHead(this.client);
            this.posing = new HexapodPosing(this.client);
        }


        public void Update()
        {
            this.commandLatencyTimer += Time.fixedDeltaTime;
        }


        /// <summary>
        /// Move head up and down by a given amount.
        /// Negative moves head down.
        /// Positive moves head up.
        /// </summary>
        /// <param name="amount">Amount to move head in degrees.</param>
        public void PitchHead(int amount)
        {
            DispatchCommand(() => this.head.Pitch(amount));
        }


        /// <summary>
        /// Move head left and right.
        /// Negative moves head right.
        /// Positive moves head left.
        /// </summary>
        /// <param name="amount">Amount to move head in degrees.</param>
        public void RollHead(int amount)
        {
            DispatchCommand(() => this.head.Roll(amount));
        }


        /// <summary>
        /// Rotate the bot's body.
        /// </summary>
        /// <param name="rotationDirection">Direction of rotation.</param>
        public void Rotate(Rotation rotationDirection)
        {
            DispatchCommand(() => this.movement.Rotate(rotationDirection));
        }


        /// <summary>
        /// Start moving the bot in a given direction.
        /// Values should be clamped to -1, 0, or 1.
        /// </summary>
        /// <param name="x">Left(-1) or right (1).</param>
        /// <param name="y">Backwards (-1) or forward(1).(</param>
        public void Move(int x, int y)
        {
            DispatchCommand(() => this.movement.Move(x, y));
        }


        /// <summary>
        /// If the bot is in locomotion, stop it.
        /// Alias for Move(0, 0)
        /// </summary>
        public void Stop()
        {
            DispatchCommand(() => this.movement.Stop());
        }


        public void SetPosition(
            int x,
            int y,
            int z)
        {
            DispatchCommand(() => this.posing.SetPosition(x, y, z));
        }


        /// <summary>
        /// Connect if currently disconnected, or disconnect if currently connected.
        /// </summary>
        public void ToggleConnection()
        {
            if (!this.client.SocketReady)
                Connect();
            else
                Disconnect();
        }


        // Used to impose a delay between commands.
        private void DispatchCommand(Action action)
        {
            if (this.commandLatencyTimer < this.commandLatency)
                return;

            this.commandLatencyTimer = 0;
            action();
        }


        private void OnConnected()
        {
            // TODO
        }


        private void Connect()
        {
            if (this.client.SocketReady)
                return;
            
            this.Connecting.Invoke();
            
            if (this.client.OpenSocket())
            {
                OnConnected();
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