package primerIntento;

import java.io.*;
import java.net.*;

public class ServidorEscucha implements Runnable {
	Socket elCliente;
	DataOutputStream salida;
	BufferedReader entrada;
	SharedBuffer buffer;
	String leido;

	ServidorEscucha(Socket cliente, SharedBuffer buffer) {
		elCliente = cliente;
		this.buffer = buffer;
	}

	public void run() {
		System.out.println("\n Entro " + " puerto=" + elCliente.getPort() + "ip=" + elCliente.getRemoteSocketAddress());
		try {
			salida = new DataOutputStream(elCliente.getOutputStream());
			salida.writeBytes("\nHola\n");
			Thread.sleep(5000);
			while (true) {
				entrada = new BufferedReader(new InputStreamReader(elCliente.getInputStream()));
				System.out.print("\nEsperando");
				leido = entrada.readLine();
				buffer.produce(leido);
				System.out.print("\nLeido" + leido + "\n");
				salida.writeBytes("\nLeido" + leido + "\n");
			}
		} catch (Exception e) {
			System.out.println(e.getMessage());
		}
	}
}
