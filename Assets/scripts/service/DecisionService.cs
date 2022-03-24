using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Proto = SmartTrafficLight.Protobuf.Traffic;

public class DecisionService : MonoBehaviour
{
	private TCP tcp;

    public Texture2D img;
    public List<byte[]> imgs = new List<byte[]>(); //4 images to send each time
    private int count = 0; //keep count of filled places
    private bool configure = true;


    // Start is called before the first frame update
    public async Task Start()
    {
        this.resetImgs();

		this.tcp = new TCP();
		await tcp.ConnectAsync("127.0.0.1", 5050);

		await Task.Delay(TimeSpan.FromSeconds(1));

        this.configure = false;

        if(GameEvents.current != null) 
        {
            GameEvents.current.onResultsRequestType2 += onResultsRequestType2;
        }
	}

    public void Update()
    {
        if(!configure)
        {
            if(this.count == 4)
            {
                var request = new Proto.ProcessImageRequest
                {
                    Image1 = Google.Protobuf.ByteString.CopyFrom(this.imgs[0]),
                    Image2 = Google.Protobuf.ByteString.CopyFrom(this.imgs[1]),
                    Image3 = Google.Protobuf.ByteString.CopyFrom(this.imgs[2]),
                    Image4 = Google.Protobuf.ByteString.CopyFrom(this.imgs[3])
                };
                
                tcp.Send(request.ToByteArray(), Convert.ToByte(1));
                tcp.Read();

                this.resetImgs();
            }
        }
    }

    public void addImage(byte[] img, int roadNr)
    {
        this.imgs[roadNr-1] = img;
        this.count++;
    }

    private void resetImgs()
    {
        this.imgs = new List<byte[]>()
        {
            new Byte[64],
            new Byte[64],
            new Byte[64],
            new Byte[64]
        };

        this.count = 0;
    }

    public void onResultsRequestType2()
    {
        GameEvents.current.resultsReceiveType2(this.tcp.getDecisions());
    }

	public class TCP
    {
        private const int HEADER_LENGTH_LENGTH = 4;
        private const int HEADER_TYPE_LENGTH = 1;
        private TcpClient socket;
        private NetworkStream stream;
        private byte[] dataBuffer;
        private List<string> decisions = new List<string>();

        public async Task ConnectAsync(string serverIpAddress, int serverPort)
        {
            socket = new TcpClient();
            await socket.ConnectAsync(serverIpAddress, serverPort);

            stream = socket.GetStream();
        }

        public void Send(byte[] message, byte type)
        {
            if (stream.CanWrite)
            {
                byte[] sendBuffer = new byte[message.Length + HEADER_LENGTH_LENGTH + HEADER_TYPE_LENGTH];
                byte[] msgLength = BitConverter.GetBytes(message.Length);
                
                msgLength.CopyTo(sendBuffer, 0);
                sendBuffer[HEADER_LENGTH_LENGTH] = type;
                message.CopyTo(sendBuffer, HEADER_LENGTH_LENGTH + HEADER_TYPE_LENGTH);
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }

        public void Read()
        {
            var msgLengthBuffer = new byte[HEADER_LENGTH_LENGTH];
            
            stream.Read(msgLengthBuffer, 0, HEADER_LENGTH_LENGTH);
            Array.Reverse(msgLengthBuffer);
            int msgLength = BitConverter.ToInt32(msgLengthBuffer, 0);

            int msgType = stream.ReadByte();

            var msgBuffer = new byte[msgLength];
            stream.Read(msgBuffer, 0, msgLength);

            if(msgType == 2)
            {
                var response = Proto.ProcessImageResponse.Parser.ParseFrom(msgBuffer);
                Debug.Log(response.Message);
                this.decisions.Add(response.Message);
                if(GameEvents.current != null) GameEvents.current.decision(response.Message);
            }
        }

        public string getDecisions()
        {
            return string.Join(", ", this.decisions.ToArray());
        }
    }
}
