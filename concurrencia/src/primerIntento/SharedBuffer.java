package primerIntento;

import java.util.LinkedList;
import java.util.Queue;

public class SharedBuffer {
    private final Queue<BakedDough> buffer = new LinkedList<>();
    private int producers = 0;
    private final int capacity;
    

    public SharedBuffer(int capacity) {
        this.capacity = capacity;
    }

    public synchronized void produce(BakedDough dough) throws InterruptedException {
        while (buffer.size() == capacity) {
            System.out.println("Buffer is full. Producer is waiting...");
            wait(); // Wait until space is available
        }
        buffer.add(dough);
        System.out.println("Produced dough's wheight is: " + dough.weight);
        notify(); // Notify the consumer that an item has been produced
    }

    public synchronized BakedDough consume() throws InterruptedException {
        while (buffer.isEmpty()) {
            System.out.println("Buffer is empty. Consumer is waiting...");
            wait(); // Wait until an item is available
        }
        BakedDough dough = buffer.poll();
        System.out.println("Consumed dough's wheight is: " + dough.weight);
        notify(); // Notify the producer that space is available
        return dough;
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