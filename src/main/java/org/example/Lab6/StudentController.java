package org.example.Lab6;

import java.util.Optional;

public class StudentController {

    private StudentRepository repository;

    StudentController(StudentRepository repository){
        this.repository = repository;
    }

    public String add(String idString, String name, String gradeString){
        try{

            long id = Long.parseLong(idString);
            float grade = Float.parseFloat(gradeString);

            StudentDTO dto = new StudentDTO(id, name, grade);
            Student student = fromDTO(dto);

            repository.save(student);

            return "done";

        }catch(IllegalArgumentException e){

            return "bad request";
        }
    }

    public String delete(String idString){
        try{
            long id = Long.parseLong(idString);
            repository.delete(id);

            return "done";

        }catch (IllegalArgumentException e){

            return "not found";
        }
    }

    public String get(String idString){

        long id = Long.parseLong(idString);
        Optional<Student> student = repository.getStudentById(id);

        if(student.isPresent()){

            StudentDTO dto = toDTO(student.get());

            return "Id: " + dto.getStudentId() + " Name: " + dto.getName() + " Grade: " + dto.getGrade();
        }
        else{
            return "not found";
        }

    }

    private Student fromDTO(StudentDTO dto){

        return new Student(dto.getStudentId(), dto.getName(), dto.getGrade());
    }

    private StudentDTO toDTO(Student student){

        return  new StudentDTO(student.getStudentId(), student.getName(), student.getGrade());
    }

}
