package org.example.Lab5;
import java.util.List;

public class Main {

    public static void main(String[] args){
        Database db = new Database();

        SchoolClass schoolClass = new SchoolClass();
        schoolClass.setClassName("3B");

        Student student1 = new Student();
        student1.setName("Emilian Zawrotny");
        student1.setSchoolClass(schoolClass);
        student1.setGrade(3.0F);
        schoolClass.addStudent(student1);

        Student student2 = new Student();
        student2.setName("Konrad Cichosz");
        student2.setSchoolClass(schoolClass);
        student2.setGrade(3.5F);
        schoolClass.addStudent(student2);

        Student student3 = new Student();
        student3.setName("Mateusz Gwiaździnski");
        student3.setGrade(4.0F);
        student3.setSchoolClass(schoolClass);
        schoolClass.addStudent(student3);

        db.storeSchoolClass(schoolClass);
        System.out.println(db.getSchoolClasses());
        System.out.println(db.getStudents());
        System.out.println(db.getStudentsWithGradeBelow(3.5F));
        System.out.println(db.getStudentsWithGradeExactly(3.5F));
        System.out.println(db.getStudentsWithGradeAbove(3.5F));
        db.close();
    }

}
