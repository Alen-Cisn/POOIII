package primerIntento;

import java.util.LinkedList;
import java.util.Queue;

public class SharedBuffer {
    private final Queue<String> buffer = new LinkedList<>();
    private int producers = 0;
    private final int capacity;
    

    public SharedBuffer(int capacity) {
        this.capacity = capacity;
    }

    public synchronized void produce(String value) throws InterruptedException {
        while (buffer.size() == capacity) {
            System.out.println("Buffer is full. Producer is waiting...");
            wait(); // Wait until space is available
        }
        buffer.add(value);
        System.out.println("Produced: " + value);
        notify(); // Notify the consumer that an item has been produced
    }

    public synchronized String consume() throws InterruptedException {
        while (buffer.isEmpty()) {
            System.out.println("Buffer is empty. Consumer is waiting...");
            wait(); // Wait until an item is available
        }
        String value = buffer.poll();
        System.out.println("Consumed string is: " + value);
        notify(); // Notify the producer that space is available
        return value;
    }
    
    public synchronized void startProducing() {
    	producers++;
    }
    
    public synchronized void endProducing() {
    	producers--;
    }
    
    public synchronized Boolean isProducing() { 
    	return producers > 0;
    }
    
}