package org.example.Lab6;

public class Student {

    private final Long studentId;
    private final String name;
    private final float grade;

    public Student(Long studentId, String name, float grade) {

        this.studentId = studentId;
        this.name = name;
        this.grade = grade;
    }

    public Long getStudentId(){
        return this.studentId;
    }

    public String getName(){
        return this.name;
    }

    public float getGrade(){
        return this.grade;
    }

}
