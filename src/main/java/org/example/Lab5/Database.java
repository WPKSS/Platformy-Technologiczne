package org.example.Lab5;
import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

public class Database {
    private EntityManager em;
    private EntityManagerFactory emf;
    public Database() {
        this.emf = Persistence.createEntityManagerFactory("my-persistence-unit");
        this.em = this.emf.createEntityManager();
    }
    public void storeSchoolClass(SchoolClass schoolClass){
        this.em.getTransaction().begin();
        this.em.persist(schoolClass);
        this.em.getTransaction().commit();
    }
    public void close() {
        this.em.close();
        this.emf.close();
    }
    public void removeSchoolClass(SchoolClass schoolClass){
        this.em.getTransaction().begin();
        this.em.remove(schoolClass);
        this.em.getTransaction().commit();
    }

    public List<SchoolClass> getSchoolClasses(){
        Query query = em.createQuery("SELECT s FROM SchoolClass s");
        List<SchoolClass> results = query.getResultList();
        return results;
    }

    public List<SchoolClass> getSchoolClasses(Integer num){
        Query query = em.createQuery("SELECT s FROM SchoolClass s");
        query.setMaxResults(num);
        List<SchoolClass> results = query.getResultList();
        return results;
    }

    public List<Student> getStudents(){
        Query query = em.createQuery("SELECT s FROM Student s");
        List<Student> results = query.getResultList();
        return results;
    }

    public List<Student> getStudents(Integer num){
        Query query = em.createQuery("SELECT s FROM Student s");
        query.setMaxResults(num);
        List<Student> results = query.getResultList();
        return results;
    }
    public List<Student> getStudentsFromClass(SchoolClass schoolClass){
        Query query = em.createQuery("SELECT s FROM Student s WHERE s.schoolClass = :schoolClass");
        query.setParameter("schoolClass", schoolClass);
        List<Student> results = query.getResultList();
        return results;
    }

    public SchoolClass getSchoolClass(String name){
        Query query = em.createQuery("SELECT s FROM SchoolClass s WHERE s.name = :name");
        query.setParameter("name", name);
        List<SchoolClass> results = query.getResultList();
        return results.isEmpty() ? null : results.getFirst();
    }

    public List<Student> getStudentsWithGradeAbove(float grade){
        Query query = em.createQuery("SELECT s FROM Student s WHERE s.grade >= :grade");
        query.setParameter("grade", grade);
        List<Student> results = query.getResultList();
        return results;
    }
    public List<Student> getStudentsWithGradeBelow(float grade){
        Query query = em.createQuery("SELECT s FROM Student s WHERE s.grade <= :grade");
        query.setParameter("grade", grade);
        List<Student> results = query.getResultList();
        return results;
    }
    public List<Student> getStudentsWithGradeExactly(float grade){
        Query query = em.createQuery("SELECT s FROM Student s WHERE s.grade >= :grade");
        query.setParameter("grade", grade);
        List<Student> results = query.getResultList();
        return results;
    }

}
