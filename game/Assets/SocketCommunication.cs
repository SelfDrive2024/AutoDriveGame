using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketCommunication : MonoBehaviour
{
    // Socket Communication
    bool isRunning;
    Thread pythonCommThread;
    public string pythonCommIP = "127.0.0.1";
    public int pythonCommPort = 25001;
    TcpListener tcpListener;
    TcpClient tcpClient;
    public string directionReceived;

    // Start is called before the first frame update
    void Start()
    {
        ThreadStart threadStart = new ThreadStart(GetCommInfo);
        pythonCommThread = new Thread(threadStart);
        pythonCommThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendAndReceiveData(bool isSending = false, string dataToBeSent = null)
    {
        NetworkStream networkStream = tcpClient.GetStream();
        byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

        // RECEIVING data from the Python process
        int bytesRead = networkStream.Read(buffer, 0, tcpClient.ReceiveBufferSize); // getting data in bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); // converting data into string

        if (dataReceived != null)
        {
            directionReceived = dataReceived;
        }

        if (isSending)
        {
            // SENDING data to Python process
            byte[] writeBuffer = Encoding.ASCII.GetBytes(dataToBeSent); // creating a write buffer
            networkStream.Write(writeBuffer, 0, writeBuffer.Length); // sending the byte array to Python
        }
    }

    void GetCommInfo()
    {
        var localAddress = IPAddress.Parse(pythonCommIP);
        tcpListener = new TcpListener(IPAddress.Any, pythonCommPort);

        tcpListener.Start();
        tcpClient = tcpListener.AcceptTcpClient();

        isRunning = true;

        while (isRunning)
        {
            SendAndReceiveData();
        }

        tcpListener.Stop();
    }
}
