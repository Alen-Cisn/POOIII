package primerIntento;

import java.util.concurrent.ThreadLocalRandom;

public class Producer implements Runnable {
    private final SharedBuffer sharedBuffer;

    public Producer(SharedBuffer sharedBuffer) {
        this.sharedBuffer = sharedBuffer;
    }

    @Override
    public void run() {
        try {
        	sharedBuffer.startProducing();
            for (int i = 0; i < 20; i++) {
                sharedBuffer.produce(new Bread(ThreadLocalRandom.current().nextInt(90, 110 + 1)));
                Thread.sleep(100);
            }
        	sharedBuffer.endProducing();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }
}