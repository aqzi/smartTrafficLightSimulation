using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using UnityEngine;

public class DecisionService : MonoBehaviour
{
	[Serializable]
	class TestObj
	{
		public string Test { get; set; }
		public int Value { get; set; }
	}

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    
    void Start()
    {
        ConnectToTcpServer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {             
			SendMessage();         
		}
    }

    void OnApplicationQuit()
    {
        if(clientReceiveThread != null) clientReceiveThread.Abort();
        if(socketConnection != null) socketConnection.Close();
    }

    private void ConnectToTcpServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}

    private void ListenForData() { 		
		try { 			
			socketConnection = new TcpClient("127.0.0.1", 65432);  			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 						
						Debug.Log("server message received as: " + serverMessage); 					
					} 				
				} 			
			}       
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}

    private void SendMessage() {         
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
				string clientMessage = "This is a message from one of your clients."; 				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = ObjectToByteArray(new TestObj {
					Test = "hey",
					Value = 7
				}); 				
				// Write byte array to socketConnection stream.                 
				//stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);  
				stream.Write(BitConverter.GetBytes(1), 0, BitConverter.GetBytes(1).Length);                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}

	public void sendImage(byte[] image)
	{
		if (socketConnection == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                               
				stream.Write(image, 0, image.Length);                        
			}
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}  
	}

	private byte[] ObjectToByteArray(object obj)
	{
		if(obj == null)
			return null;
		BinaryFormatter bf = new BinaryFormatter();
		using (MemoryStream ms = new MemoryStream())
		{
			bf.Serialize(ms, obj);
			return ms.ToArray();
		}
	}
}
