import java.io.*;
import java.net.*;
import java.util.*;

public class ServidorEscucha implements Runnable{
    Socket client;
    DataOutputStream salida;
    BufferedReader entrada;
    String line;
    Map<Integer, Queue<String>> clientes;

    ServidorEscucha(Socket cliente, Map<Integer, Queue<String>> clientes) {
        this.client = cliente;
        this.clientes = clientes;
    }

    public void run(){
        //Conexion
        int clientID = client.getPort();
        System.out.println("Entro: Puerto: " + clientID +
        "IP: " + client.getRemoteSocketAddress());

        try {
            salida = new DataOutputStream(client.getOutputStream());
            entrada = new BufferedReader(new InputStreamReader(client.getInputStream()));{}

            salida.writeBytes("Conexion establecida\n\r");
            Thread.sleep(5000);

            synchronized (clientes) {
                clientes.put(clientID, new LinkedList<String>());
            }

            while (true) { 
                //READ WAITING
                System.out.println("Esperando...");
                salida.writeBytes("Esperando...\n\r");

                line = entrada.readLine();

                if (line == null) {
                    System.out.println("El cliente cerro la conexion"); break;
                }

                //SEND
                if (line.startsWith("/send")) {
                    String[] sections = line.split(" ", 3);
                    if (sections.length < 3) {
                        salida.writeBytes("Error de sintaxis: use /send <ID_destinatario> <mensaje>\n\r");
                        continue;
                    }  

                    String mensaje = sections[2];
                    
                    //ALL
                    if (sections[1].equals("all")) {
                        if (clientes.size() == 1) {
                            salida.writeBytes("No hay mas clientes conectados\n\r");    
                        } else {
                            clientes.forEach((k,v) -> {
                                if (!k.equals(clientID)) {
                                    v.add("Mensaje de " + clientID + ": " + mensaje);
                                }
                            });
                            salida.writeBytes("Mensaje enviado a todos los conectados\n\r");
                            continue;
                        }
                    } else {
                        if (isNumeric(sections[1])) {
                            int destinatario = Integer.parseInt(sections[1]);
    
                            synchronized (clientes) {
                                if (clientes.containsKey(destinatario)) {
                                    Queue<String> bufferDestinatario = clientes.get(destinatario);
                                    bufferDestinatario.add("Mensaje de " + clientID + ": " + mensaje);
                                    salida.writeBytes("Mensaje enviado a " + destinatario + "\n\r");
                                } else {
                                    salida.writeBytes("El destinatario " + destinatario + " no se encuentra\n\r");
                                }
                            } 
                        } else {
                            salida.writeBytes("Error: el formato del destinario no es correcto\n\r");
                        }   
                    }                     

                //CHECK
                } else if (line.equals("/check")) {
                    synchronized (clientes) {
                        Queue<String> bufferCliente = clientes.get(clientID);
                        if (bufferCliente.isEmpty()) {
                            salida.writeBytes("No hay nuevos mensajes\n\r");
                        } else {
                            salida.writeBytes(bufferCliente.poll() + "\n\r");
                        }
                    }

                //CLOSE
                } else if(line.equals("/close")){
                    salida.writeBytes("Cerrando conexion...");
                    break;

                //WHOAMI
                } else if(line.equals("/whoami")){
                    salida.writeBytes("Cliente [" + clientID + "]\n\r");

                //LIST
                } else if(line.equals("/list")){
                    salida.writeBytes("Clientes conectados: [yo: " + clientID + "]");
                    synchronized (clientes) {
                        for (Integer cliente : clientes.keySet()) {
                            if (!cliente.equals(clientID)) {
                                salida.writeBytes(" [" + cliente + "]");
                            }
                        }
                        salida.writeBytes("\n\r");
                    }
                
                //HELP
                } else if (line.equals("/help")) {
                    salida.writeBytes("""
                            Lista de comandos:\r
                                * /help\r
                                * /send <ID_destinatario> <mensaje>\r
                                * /check\r
                                * /list\r
                                * /whoami\r
                                * /close\r
                            """);

                //NO-COMMAND READ    
                } else {
                    salida.writeBytes("Leido " + line +  "\n\r");
                }
            }

            //cierre de recursos
            synchronized (clientes) {
                clientes.remove(clientID);
            }
            
            entrada.close();
            salida.close();
            client.close();
            System.out.println("Conexion cerrada con " + clientID);

        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }

    public boolean isNumeric(String cadena){
        try {
            Integer.parseInt(cadena);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }
}
