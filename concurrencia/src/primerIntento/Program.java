package primerIntento;

public class Program {
	public static void main(String[] args) {
        SharedBuffer sharedBuffer = new SharedBuffer(15); // Buffer capacity of 5

        Thread producer1Thread = new Thread(new Producer(sharedBuffer));
        Thread producer2Thread = new Thread(new Producer(sharedBuffer));
        Thread consumer1Thread = new Thread(new Consumer(sharedBuffer));
        Thread consumer2Thread = new Thread(new Consumer(sharedBuffer));
        Thread consumer3Thread = new Thread(new Consumer(sharedBuffer));
        Thread consumer4Thread = new Thread(new Consumer(sharedBuffer));

        producer1Thread.start();
        producer2Thread.start();
        consumer1Thread.start();
        consumer2Thread.start();
        consumer3Thread.start();
        consumer4Thread.start();

        try {
            producer1Thread.join();
            producer2Thread.join();
            consumer1Thread.join();
            consumer2Thread.join();
            consumer3Thread.join();
            consumer4Thread.join();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }

        System.out.println("Production and consumption completed.");
    }
}
