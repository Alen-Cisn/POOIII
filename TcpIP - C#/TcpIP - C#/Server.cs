using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpIP
{
  public class Server
  {
    private static TcpListener serverSocket = null;
    private static readonly Dictionary<int, Queue<string>> clientes = [];

    public static void Main(string[] args)
    {
      int PORT = 60001;
      Console.WriteLine("Inicializando servidor...");

      try
      {
        serverSocket = new TcpListener(IPAddress.Any, PORT);
        serverSocket.Start();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      while (true)
      {
        TcpClient cliente = null;
        try
        {
          cliente = serverSocket.AcceptTcpClient();
        }
        catch (Exception e)
        {

          Console.WriteLine(e.Message);
        }

        ServidorEscucha nuevoCliente = new ServidorEscucha(cliente, clientes);
        Thread hilo = new Thread(new ThreadStart(nuevoCliente.Run));
        hilo.Start();
      }
    }
  }

  public class ServidorEscucha(TcpClient cliente, Dictionary<int, Queue<string>> clientes) : IDisposable
  {
    private readonly TcpClient client = cliente;
    private StreamReader entrada;
    private StreamWriter salida;
    private string line;
    private Dictionary<int, Queue<string>> clientes = clientes;

    public void Run()
    {
      int clientID = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
      Console.WriteLine("Entro: [" + clientID + "] IP: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

      try
      {
        NetworkStream stream = client.GetStream();
        salida = new StreamWriter(stream);
        entrada = new StreamReader(stream);
        salida.AutoFlush = true;

        salida.WriteLine("Conexion establecida");
        Thread.Sleep(5000);

        lock (clientes)
        {
          clientes[clientID] = new Queue<string>();
        }

        while (true)
        {
          Console.WriteLine("Esperando...");
          salida.WriteLine("Esperando...");

          line = entrada.ReadLine();

          if (line == null)
          {
            Console.WriteLine("El cliente cerro la conexion");
            break;
          }

          if (line.StartsWith("/send"))
          {
            string[] sections = line.Split(' ', 3);
            if (sections.Length < 3)
            {
              salida.WriteLine("Error de sintaxis: pruebe usando /send <ID_destinatario> <mensaje>");
              continue;
            }

            string mensaje = sections[2];

            if (sections[1].Equals("all"))
            {
              if (clientes.Count == 1)
              {
                salida.WriteLine("No hay mas clientes conectados");
              }
              else
              {
                lock (clientes)
                {
                  foreach (var id in clientes)
                  {
                    if (id.Key != clientID)
                    {
                      id.Value.Enqueue("Mensaje de " + clientID + ": " + mensaje);
                    }
                  }
                }
                salida.WriteLine("Mensaje enviado a todos los conectados");
              }
            }
            else
            {
              if (int.TryParse(sections[1], out int destinatario))
              {
                lock (clientes)
                {
                  if (clientes.TryGetValue(destinatario, out Queue<string>? value))
                  {
                    value.Enqueue("Mensaje de " + clientID + ": " + mensaje);
                    salida.WriteLine("Mensaje enviado a " + destinatario);
                  }
                  else
                  {
                    salida.WriteLine("El destinatario " + destinatario + " no se encuentra");
                  }
                }
              }
              else
              {
                salida.WriteLine("Error: el formato del destinatario no es correcto");
              }
            }
          }
          else if (line.Equals("/check"))
          {
            lock (clientes)
            {
              if (clientes[clientID].Count == 0)
              {
                salida.WriteLine("No hay nuevos mensajes");
              }
              else
              {
                salida.WriteLine(clientes[clientID].Dequeue());
              }
            }
          }
          else if (line.Equals("/close"))
          {
            salida.WriteLine("Cerrando conexion...");
            break;
          }
          else if (line.Equals("/list"))
          {
            salida.WriteLine("Clientes conectados: [yo: " + clientID + "]");
            lock (clientes)
            {
              foreach (var cliente in clientes.Keys)
              {
                if (cliente != clientID)
                {
                  salida.WriteLine(" [" + cliente + "]");
                }
              }
              salida.WriteLine();
            }
          }
          else if (line.Equals("/help"))
          {
            salida.WriteLine("Lista de comandos:\n\r" +
                "* /help\n\r" +
                "* /send <ID_destinatario> <mensaje>\n\r" +
                "* /check\n\r" +
                "* /list\n\r" +
                "* /close");
          }
          else
          {
            salida.WriteLine("Leido " + line);
          }
        }

        lock (clientes)
        {
          clientes.Remove(clientID);
        }

        entrada.Close();
        salida.Close();
        client.Close();
        Console.WriteLine("Conexion cerrada con " + clientID);
      }
      catch (Exception e)
      {
        Console.WriteLine("Error: " + e.Message);
      }
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}
