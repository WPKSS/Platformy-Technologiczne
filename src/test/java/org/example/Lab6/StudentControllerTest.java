package org.example.Lab6;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.Optional;
import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.ArgumentMatchers.anyLong;
import static org.mockito.Mockito.*;

    class StudentControllerTest {

        private StudentRepository repositoryMock;
        private StudentController controller;

        @BeforeEach
        void setUp() {
            repositoryMock = mock(StudentRepository.class);
            controller = new StudentController(repositoryMock);
        }
        @Test
        // --- Testy DELETE
        void deleteExistingStudent_ReturnDone() {
            doNothing().when(repositoryMock).delete(1L);
            String result = controller.delete("1");
            assertThat(result).isEqualTo("done");
            verify(repositoryMock).delete(1L);
        }
        @Test
        void deleteNonExistingStudent_ReturnNotFound() {
            doThrow(IllegalArgumentException.class).when(repositoryMock).delete(2L);
            String result = controller.delete("2");
            assertThat(result).isEqualTo("not found");
            verify(repositoryMock).delete(2L);
        }

        // --- Testy GET
        @Test
        void getExistingStudent_ReturnStudentData() {
            Student student = new Student(1L, "Anna", 5.0f);
            when(repositoryMock.getStudentById(1L)).thenReturn(Optional.of(student));
            String result = controller.get("1");
            assertThat(result).isEqualTo("Id: 1 Name: Anna Grade: 5.0");
            verify(repositoryMock).getStudentById(1L);
        }
        @Test
        void getNonExistingStudent_ReturnNotFound() {
            when(repositoryMock.getStudentById(anyLong())).thenReturn(Optional.empty());
            String result = controller.get("99");
            assertThat(result).isEqualTo("not found");
            verify(repositoryMock).getStudentById(99L);
        }

        // --- Testy ADD
        @Test
        void addStudent_ReturnDone() {
            doNothing().when(repositoryMock).save(any(Student.class));
            String result = controller.add("3", "Marek", "4.5");
            assertThat(result).isEqualTo("done");
            verify(repositoryMock).save(any(Student.class));
        }
        @Test
        void addStudentId_ReturnBadRequest() {
            doThrow(IllegalArgumentException.class).when(repositoryMock).save(any(Student.class));
            String result = controller.add("1", "Ewa", "3.5");
            assertThat(result).isEqualTo("bad request");
            verify(repositoryMock).save(any(Student.class));
        }
    }
