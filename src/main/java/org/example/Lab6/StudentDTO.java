package org.example.Lab6;

public class StudentDTO {

    private Long studentId;
    private String name;
    private float grade;

    public StudentDTO(){}

    public StudentDTO(Long studentId, String name, float grade) {

        this.studentId = studentId;
        this.name = name;
        this.grade = grade;
    }

    public Long getStudentId(){
        return this.studentId;
    }

    public void setStudentId(Long studentId){
        this.studentId = studentId;
    }

    public String getName(){
        return this.name;
    }

    public void setName(String name){
        this.name = name;
    }

    public float getGrade(){
        return this.grade;
    }

    public void setGrade(float grade){
        this.grade = grade;
    }

}
