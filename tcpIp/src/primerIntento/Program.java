package primerIntento;

import java.net.*;

public class Program {
	
	@SuppressWarnings("resource")
	public static void main(String[] args) {
		int PUERTO = 60001;
		System.out.print("\n Arranca Servidor");
		ServerSocket socketServidor = null;
		try {
			socketServidor = new ServerSocket(PUERTO);
		} catch (Exception e) {
			System.out.println(e.getMessage());
		}
		while (true) {
			Socket cliente = null;
			try {
				cliente = socketServidor.accept();
			} catch (Exception e) {
				System.out.println(e.getMessage());
			}
			ServidorEscucha nuevoCliente = new ServidorEscucha(cliente);
			Thread hilo = new Thread(nuevoCliente);
			hilo.start();
		}
	}

}
