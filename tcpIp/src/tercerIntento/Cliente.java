
import java.util.Queue;

public class Cliente {
    public int ID;
    private Queue<String> buffer;

    public Cliente(int ID, Queue<String> buffer) {
        this.ID = ID;
        this.buffer = buffer;
    }

    public Queue<String> getBuffer(){
        return this.buffer;
    }
}
