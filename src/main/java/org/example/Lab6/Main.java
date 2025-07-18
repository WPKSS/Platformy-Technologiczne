package org.example.Lab6;

public class Main {

    public static void main(String[] args){

        StudentRepository repository = new StudentRepository();
        StudentController controller = new StudentController(repository);

        System.out.println(controller.get("1"));

        System.out.println(controller.delete("1"));

        System.out.println(controller.add("1", "Roman", "2.0"));
        System.out.println(controller.add("1", "Roman", "2.0"));

        System.out.println(controller.get("1"));

        System.out.println(controller.delete("1"));

        System.out.println(controller.get("1"));

    }

}
