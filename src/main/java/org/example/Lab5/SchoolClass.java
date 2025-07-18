package org.example.Lab5;
import jakarta.persistence.*;

import java.util.LinkedList;
import java.util.List;

@Entity
public class SchoolClass {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int school_class_id;

    private String className;

    @OneToMany(mappedBy = "schoolClass", cascade = CascadeType.ALL)
    private List<Student> students;

    public SchoolClass() {
        this.students = new LinkedList<>();
    }

    public void setClassName(String className){
        this.className = className;
    }
    public void setStudents(List<Student> students) { this.students = students; }
    public void addStudent(Student student){
        student.setSchoolClass(this);
        this.students.add(student);
    }
    public void removeStudent(Student student){
        this.students.remove(student);
    }
    public String toString(){
        return String.format("SchoolClass(%s)", className);
    }
}
