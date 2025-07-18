package org.example.Lab4;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Server {

    public Server() throws IOException{

        // utworzenie socketu dla serwera
        ServerSocket sever_socket = new ServerSocket(15190);
        System.out.println(print_datetime() + "Port is open, server started working.");

        // pętla nasłuchujaca klienta
        while(true){

            Socket socket = sever_socket.accept();

            // utworzenie nowego wątku dla klienta
            Server_thread sever_thread = new Server_thread(socket, this);
            Thread thread = new Thread(sever_thread);
            thread.start();
        }

    }

    // funkcja zwracająca date i godzine
    public String print_datetime(){

        return "["  + LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss")) + "] ";

    }

    public static void main(String[] args){

        try{

            new Server();

        }catch(Exception e){

            e.printStackTrace();
        }

    }
}
