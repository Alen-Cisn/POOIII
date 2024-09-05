import java.net.*;
import java.util.LinkedList;
import java.util.Queue;


public class server{
    private static ServerSocket serverSocket = null;
    private static Queue<String> buffer = new LinkedList<String>();

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

            ServidorEscucha nuevoCliente = new ServidorEscucha(cliente, buffer);
            Thread hilo = new Thread(nuevoCliente); 
            hilo.start();
        }

    }
}