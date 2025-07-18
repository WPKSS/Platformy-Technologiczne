package org.example.Lab4;
import java.io.*;
import java.net.Socket;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Scanner;

public class Client {

    Client() throws IOException{

        Socket socket = new Socket("localhost", 15190);

        // zródła wejścia i wyjścia dla klienta
        ObjectOutputStream socket_output = new ObjectOutputStream(socket.getOutputStream());
        ObjectInputStream socket_input = new ObjectInputStream(socket.getInputStream());

        Scanner keyboard = new Scanner(System.in);

        System.out.println(print_datetime() + "Started connection with sever " + socket.getInetAddress() + socket.getPort());

        // sprawdzamy czy sewer jest gotowy
        try {

            Object received = socket_input.readObject();
            System.out.println(print_datetime() + "[SEVER]: " + received);

        } catch (ClassNotFoundException e) {

            System.out.println("Can't read the message form sever: " + e.getMessage());

            socket_output.close();
            socket_input.close();
            socket.close();
            return;
        }

        // podanie danych gracza
        System.out.print(print_datetime() + "Give name:");
        String name = keyboard.nextLine();

        System.out.print(print_datetime() + "Give players points: ");
        int points = Integer.parseInt(keyboard.nextLine());

        socket_output.writeObject(new Player(name, points));

        // sprawdzamy czy sewer jest zaknończył zadanie
        try {

            Object received = socket_input.readObject();
            System.out.println(print_datetime() + "[SEVER]: " + received);

        } catch (ClassNotFoundException e) {

            System.out.println("Can't read the message form sever: " + e.getMessage());

            socket_output.close();
            socket_input.close();
            socket.close();
            return;
        }

        // zamknięcie soketu oraz źródeł
        socket_output.close();
        socket_input.close();
        socket.close();

        System.out.println(print_datetime() + "Closed connection with sever.");

    }

    private String print_datetime(){

        return "["  + LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss")) + "] ";

    }

    public static void main(String[] args){

        try{

            new Client();

        }catch(Exception e){

            e.printStackTrace();

        }

    }

}
