package org.example.Lab4;
import java.io.Serializable;

public class Player implements Serializable{

    String name;
    int points;

    public Player(String name, int points){

        this.name = name;
        this.points = points;

    }

}
