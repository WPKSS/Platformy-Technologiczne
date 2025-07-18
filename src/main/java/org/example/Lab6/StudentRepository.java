package org.example.Lab6;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

public class StudentRepository {

    private final Map<Long, Student> repository = new HashMap<>();

    public void save(Student student){

        if(repository.containsKey(student.getStudentId())){

            throw new IllegalArgumentException("Student with this ID already exist.");
        }

        repository.put(student.getStudentId(), student);
    }

    public void delete(long studentId){

        if(!repository.containsKey(studentId)){
            throw new IllegalArgumentException("Student with this ID does not exist.");
        }

        repository.remove(studentId);
    }

    public Optional<Student> getStudentById(long studentId){
        return Optional.ofNullable(repository.get(studentId));
    }

}
