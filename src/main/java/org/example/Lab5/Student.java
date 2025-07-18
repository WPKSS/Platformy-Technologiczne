package org.example.Lab5;
import jakarta.persistence.*;

@Entity
public class Student {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int sutdent_id;

    private String name;
    private float grade;
    @ManyToOne
    @JoinColumn(name = "school_class_id")
    private SchoolClass schoolClass;

    public void setName(String name){ this.name = name; }
    public void setSchoolClass(SchoolClass schoolClass){
        this.schoolClass = schoolClass;
    }
    public void setGrade(float grade){
        if (grade >= 2.0 && grade <= 5.0) {
            this.grade = grade;
        }
        else {
            System.out.println("Invalid grade");
        }
    }
    public String toString(){
        return String.format("Student(%s, %s, %.2f)", name, schoolClass, grade);
    }
    public Student() {}
}
