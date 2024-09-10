import java.net.*;
import java.util.*;


public class Server{
    private static ServerSocket serverSocket = null;
    private static Map<Integer, Queue<String>> clientes = new HashMap<>();

    public static void main(String[] args) {
        int PUERTO = 60001;
        System.out.println("Iniciando servidor...");    

        try {
            serverSocket = new ServerSocket(PUERTO);
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

        while (true) {
            Socket cliente = null;
            try {
                cliente = serverSocket.accept();
            } catch (Exception e) {
                System.out.println(e.getMessage());
            }

            ServidorEscucha nuevoCliente = new ServidorEscucha(cliente, clientes);
            Thread hilo = new Thread(nuevoCliente); 
            hilo.start();
        }

    }
}