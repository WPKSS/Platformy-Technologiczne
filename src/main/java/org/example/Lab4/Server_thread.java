package org.example.Lab4;
import java.net.Socket;
import java.io.*;

public class Server_thread implements Runnable {
    private Socket socket;
    private Server server;

    public Server_thread(Socket socket, Server server){

        this.socket = socket;
        this.server = server;
    }

    @Override
    public void run(){

        try{

            // zródła wejścia i wyjścia dla servera
            ObjectOutputStream socket_output = new ObjectOutputStream(socket.getOutputStream());
            ObjectInputStream socket_input = new ObjectInputStream(socket.getInputStream());

            System.out.println(server.print_datetime() + "Started connection with player");

            socket_output.writeObject("READY");

            // przyjęcie seralizowanego obiektu od klienta
            Player player = (Player) socket_input.readObject();
            System.out.println(server.print_datetime() + "Player " + player.name + " has " + player.points + " points.");

            socket_output.writeObject("FINISHED");

            // zamknięcie soketu oraz źródeł
            socket_output.close();
            socket_input.close();
            socket.close();

            System.out.println(server.print_datetime() + "Closed connection with player.");

        }
        catch (Exception e){

            e.printStackTrace();

        }

    }

}
