using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UDPHandler : MonoBehaviour
{
    private string udpAddress; // Target IP for sending
    private int udpSendPort;   // Port for sending
    private int udpReceivePort; // Port for receiving

    private UdpClient udpClient;
    private UdpClient udpReceiver;
    private CancellationTokenSource cancellationTokenSource;
    private Task receiveTask;

    public TextMeshProUGUI receivedText; // UI Text to display received messages (optional)

    // Event for received commands
    public delegate void OnCommandReceived(int command);
    public static event OnCommandReceived CommandReceived;

    private ConcurrentQueue<int> commandQueue = new ConcurrentQueue<int>();

    void Start()
    {
        // Load UDP settings from PersistentSettings
        LoadSettings();

        udpClient = new UdpClient();
        udpReceiver = new UdpClient(udpReceivePort);
        cancellationTokenSource = new CancellationTokenSource();

        // Start async receiving
        receiveTask = Task.Run(() => ReceiveUDPDataAsync(cancellationTokenSource.Token));
    }

    // Call this to load the persistent settings
    private void LoadSettings()
    {
        PersistentSettings settings = PersistentSettings.Instance;

        // Load UDP configuration from persistent settings
        udpAddress = settings.udpAddress;
        udpSendPort = settings.udpSendPort;
        udpReceivePort = settings.udpReceivePort;

        // Optionally, log the settings
        Debug.Log($"Loaded UDP settings: {udpAddress}:{udpSendPort}, Receive Port: {udpReceivePort}");
    }

    private async Task ReceiveUDPDataAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result = await udpReceiver.ReceiveAsync();

                // Convert first byte (uint8) to integer (command)
                int command = result.Buffer[0];
                commandQueue.Enqueue(command);

                Debug.Log($"Received UDP command: {command}");
            }
            catch (ObjectDisposedException)
            {
                break; // Exit loop if UdpClient is closed
            }
            catch (Exception e)
            {
                Debug.LogError($"UDP Receive Error: {e.Message}");
            }
        }
    }

    public void SendUDPCommand(int command)
    {
        byte[] data = new byte[] { (byte)command }; // Convert command to byte
        udpClient.Send(data, data.Length, udpAddress, udpSendPort);
        Debug.Log($"Sent UDP command: {command} to {udpAddress}:{udpSendPort}");
    }

    void Update()
    {
        // Dequeue and process any received commands
        while (commandQueue.TryDequeue(out int command))
        {
            CommandReceived?.Invoke(command);

            // Optionally, update the UI text
            if (receivedText != null)
            {
                receivedText.text = $"Received Command: {command}";
            }

            // Perform actions based on the command (example)
            switch (command)
            {
                case 1:
                    Debug.Log("Move Forward");
                    break;
                case 2:
                    Debug.Log("Move Backward");
                    break;
                case 3:
                    Debug.Log("Move Left");
                    break;
                case 4:
                    Debug.Log("Move Right");
                    break;
                default:
                    Debug.Log("Unknown command");
                    break;
            }
        }
    }

    void OnDestroy()
    {
        cancellationTokenSource.Cancel();
        udpReceiver?.Close();
        udpClient?.Close();

        if (receiveTask != null)
        {
            receiveTask.Wait();
            receiveTask.Dispose();
        }

        cancellationTokenSource.Dispose();
    }

    // Call SaveSettings when you leave the scene or if there are any changes
    void OnApplicationQuit()
    {
        PersistentSettings.Instance.SaveSettings();
    }
}
