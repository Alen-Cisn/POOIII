import java.io.*;
import java.net.*;
import java.util.Queue;

public class ServidorEscucha implements Runnable{
    Socket client;
    DataOutputStream salida;
    BufferedReader entrada;
    String leido;
    Queue<String> buffer;

    ServidorEscucha(Socket cliente, Queue<String> buffer) {        
        this.client = cliente;
        this.buffer = buffer;
    }

    public void run(){
        System.out.println("Entro: Puerto: " + client.getPort() +
        "IP: " + client.getRemoteSocketAddress());

        try {
            salida = new DataOutputStream(client.getOutputStream());
            entrada = new BufferedReader(new InputStreamReader(client.getInputStream()));{}

            salida.writeBytes("Conexion establecida\n");
            Thread.sleep(5000);

            while (true) { 
                System.out.println("Esperando...");
                leido = entrada.readLine();

                if (leido == null) {
                    System.out.println("El cliente cerro la conexion"); break;
                }

                if (leido.startsWith("/put")) {
                    String mensaje = leido.substring(5); //elimina el /put
                    synchronized (buffer) {
                        buffer.add(mensaje); //carga mensaje en buffer
                    }
                    salida.writeBytes("Mensaje añadido al buffer\n");
                } else if (leido.equals("/get")) {
                    synchronized (buffer) {
                        if (buffer.isEmpty()) {
                            salida.writeBytes("No hay mensajes en el buffer, pruebe añadiendo uno con /put <mensaje>\n");
                        } else {
                            String mensaje = buffer.poll();
                            salida.writeBytes("Mensaje: " + mensaje + "\n");
                        }
                    }
                } else if (leido.equals("/close")) {
                    System.out.println("Cerrando conexion...");
                    salida.writeBytes("Cerrando conexion...\n");
                    break;
                } else if (leido.equals("/check")) {
                    salida.writeBytes("Hay " + buffer.size() + " mensajes pendientes\n");
                }else {
                    salida.writeBytes("Leido " + leido + "\n");
                    System.out.println("Leido " + leido + "\n");
                }
                

                
            }

            //cierre de recursos
            entrada.close();
            salida.close();
            client.close();
            System.out.println("Conexion cerrada");

        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
}
