package com.pragmatest.services;

import com.pragmatest.models.User;
import com.pragmatest.repositories.UserRepository;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.context.ActiveProfiles;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.*;


@SpringBootTest
@ActiveProfiles("test")
public class UserServiceTest {

    @MockBean
    UserRepository userMockRepository;

    @Autowired
    UserService userService;

    @Test
    void testSaveUserValidUser() {
        // Arrange
        User newUser = new User("John Smith", "London", 20);
        when(userMockRepository.save(newUser)).thenReturn(newUser);

        // Act
        Optional<User> returnedNewUser = userService.saveUser(newUser);

        // Assert
        assertEquals(newUser, returnedNewUser.get());

        verify(userMockRepository, times(1)).save(newUser);
    }

    @Test
    void testSaveUserInvalidUser() {
        // Arrange
        User newUser = new User("John Smith", "London", 17);
        when(userMockRepository.save(newUser)).thenReturn(newUser);

        // Act
        Optional<User> returnedNewUser = userService.saveUser(newUser);

        //Assert
        assertEquals(returnedNewUser, Optional.empty());

        verify(userMockRepository, times(0)).save(any(User.class));
    }

    @Test
    void testGetUserByIdValidId() {
        //Arrange
        User newUser = new User(1L, "John Smith", "London", 25);
        when(userMockRepository.findById(1L)).thenReturn(Optional.of(newUser));

        // Act
        Optional<User> returnedUser = userService.getUserById(1L);

        //Assert
        assertEquals(newUser, returnedUser.get());

        verify(userMockRepository, times(1)).findById(1L);
    }

    @Test
    void testGetUserByIdInvalidId() {
        //Arrange
        when(userMockRepository.findById(1L)).thenReturn(Optional.empty());

        // Act
        Optional<User> returnedUser = userService.getUserById(1L);

        //Assert
        assertTrue(returnedUser.isEmpty());

        verify(userMockRepository, times(1)).findById(1L);
    }

    @Test
    void testGetAllUsersValidUsers() {
        //Arrange
        List<User> expectedUsersList = List.of(
            new User("John Smith", "London", 45),
            new User("Mary Jones", "Manchester", 60)
        );
        when(userMockRepository.findAll()).thenReturn(expectedUsersList);


        // Act
        List<User> actualUsersList = userService.getAllUsers();

        // Assert
        assertEquals(expectedUsersList, actualUsersList);

        verify(userMockRepository, times(1)).findAll();
    }


}