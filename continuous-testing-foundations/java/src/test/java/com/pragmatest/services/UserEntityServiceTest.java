package com.pragmatest.services;

import com.pragmatest.models.User;
import com.pragmatest.models.UserEntity;
import com.pragmatest.repositories.UserRepository;
import org.junit.jupiter.api.Test;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.context.ActiveProfiles;

import java.util.List;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.tuple;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.*;

@SpringBootTest
@ActiveProfiles("test")
public class UserEntityServiceTest {

    @MockBean
    UserRepository userMockRepository;

    @Autowired
    UserService userService;

    @Autowired
    ModelMapper modelMapper;

    @Test
    void testSaveUserValidUser() {
        // Arrange
        UserEntity newUserEntity = new UserEntity("John Smith", "London", 20);
        when(userMockRepository.save(newUserEntity)).thenReturn(newUserEntity);

        User user = modelMapper.map(newUserEntity, User.class);

        // Act
        Optional<User> returnedNewUser = userService.saveUser(user);

        // Assert
        assertEquals(newUserEntity, returnedNewUser.get());

        verify(userMockRepository, times(1)).save(newUserEntity);
    }

    @Test
    void testSaveUserInvalidUser() {
        // Arrange
        UserEntity newUserEntity = new UserEntity("John Smith", "London", 17);
        when(userMockRepository.save(newUserEntity)).thenReturn(newUserEntity);

        User user = modelMapper.map(newUserEntity, User.class);

        // Act
        Optional<User> returnedNewUser = userService.saveUser(user);

        //Assert
        assertEquals(returnedNewUser, Optional.empty());

        verify(userMockRepository, times(0)).save(any(UserEntity.class));
    }

    @Test
    void testGetUserByIdValidId() {
        //Arrange
        UserEntity newUserEntity = new UserEntity(1L, "John Smith", "London", 25);
        when(userMockRepository.findById(1L)).thenReturn(Optional.of(newUserEntity));

        User expectedUser = modelMapper.map(newUserEntity, User.class);

        // Act
        Optional<User> returnedUser = userService.getUserById(1L);

        //Assert
        assertEquals(expectedUser, returnedUser.get());

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
        List<UserEntity> usersList = List.of(
            new UserEntity("John Smith", "London", 45),
            new UserEntity("Mary Jones", "Manchester", 60)
        );

        when(userMockRepository.findAll()).thenReturn(usersList);

        User expectedUser1 = new User("John Smith", "London", 45);
        User expectedUser2 = new User("Mary Jones", "Manchester", 60);

        // Act
        List<User> actualUsersList = userService.getAllUsers();

        // Assert
        assertThat(actualUsersList)
                .extracting(User::getFullName, User::getLocality, User::getAge)
                .containsExactly(
                        tuple(expectedUser1.getFullName(), expectedUser1.getLocality(), expectedUser1.getAge()),
                        tuple(expectedUser2.getFullName(), expectedUser2.getLocality(), expectedUser2.getAge())
                );

        verify(userMockRepository, times(1)).findAll();
    }
}