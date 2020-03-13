package com.pragmatest.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.pragmatest.exceptions.UserInvalidException;
import com.pragmatest.exceptions.UserNotFoundException;
import com.pragmatest.matchers.UserMatcher;
import com.pragmatest.models.User;
import com.pragmatest.models.UserRequest;
import com.pragmatest.services.UserService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.function.Executable;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.context.ActiveProfiles;

import java.util.Arrays;
import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.Mockito.*;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@ActiveProfiles("test")
public class UserControllerUnitTest {

    private static final ObjectMapper om = new ObjectMapper();

    @Autowired
    private UserController userController;

    @MockBean
    private UserService userMockService;

    @Test
    public void testGetUserByIdValidId() {
        // Arrange
        User user = new User("John Smith", "London", 23);
        when(userMockService.getUserById(1L)).thenReturn(Optional.of(user));

        Long userId = 1L;

        //Act
        userController.findOne(userId);

        // Assert
        verify(userMockService, times(1)).getUserById(1L);
    }

    @Test
    public void testGetUsersValidUsers() {
        // Arrange
        List<User> users = Arrays.asList(
                new User("John Smith", "London", 23),
                new User("Mary Walsh", "Liverpool", 30));

        when(userMockService.getAllUsers()).thenReturn(users);

        userController.findAll();

        verify(userMockService, times(1)).getAllUsers();
    }

    @Test
    public void testGetUserByIdNonExistentId() {
        when(userMockService.getUserById(1L)).thenReturn(Optional.empty());

        Executable executable = () -> {
            userController.findOne(1L);
        };

        assertThrows(UserNotFoundException.class, executable);

        verify(userMockService, times(1)).getUserById(1L);
    }

    //
    @Test
    public void testSaveUserValidUser() {
        // Arrange
        User newUser = new User("Marisa Jones", "Newcastle", 20);
        when(userMockService.saveUser(argThat(new UserMatcher(newUser)))).thenReturn(Optional.of(newUser));

        UserRequest userRequest = new UserRequest();
        userRequest.setFullName("Marisa Jones");
        userRequest.setAge(20);
        userRequest.setLocality("Newcastle");
        userController.newUser(userRequest);

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(newUser)));
    }

    @Test
    public void testSaveUserInvalidUser() {
        // Arrange
        User newUser = new User("Jane Stark", "Newcastle", 17);
        when(userMockService.saveUser(argThat(new UserMatcher(newUser)))).thenReturn(Optional.empty());

        UserRequest userRequest = new UserRequest();
        userRequest.setFullName("Jane Stark");
        userRequest.setAge(17);
        userRequest.setLocality("Newcastle");

        Executable executable = () -> {
            userController.newUser(userRequest);
        };

        assertThrows(UserInvalidException.class, executable);

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(newUser)));
    }


//    @Test
//    public void testUpdateUserValidUser() throws Exception {
//        // Arrange
//        UserEntity updateUserEntity = new UserEntity(1L, "Peter Marshall", "London", 40);
//        when(userMockService.saveUser(argThat(new UserMatcher(updateUserEntity)))).thenReturn(Optional.of(updateUserEntity));
//
//        HttpHeaders headers = new HttpHeaders();
//        headers.setContentType(MediaType.APPLICATION_JSON);
//
//        HttpEntity<String> entity = new HttpEntity<>(om.writeValueAsString(updateUserEntity), headers);
//
//        String endpoint = "/users/1";
//
//        // Act
//        ResponseEntity<String> response = testRestTemplate.exchange(endpoint, HttpMethod.PUT, entity, String.class);
//
//        // Assert
//        assertEquals(HttpStatus.OK, response.getStatusCode());
//        JSONAssert.assertEquals(om.writeValueAsString(updateUserEntity), response.getBody(), false);
//
//        verify(userMockService, times(1)).getUserById(1L);
//        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(updateUserEntity)));
//    }
//
//    @Test
//    public void testDeleteUserValidUser() {
//        // Arrange
//        doNothing().when(userMockService).deleteUserById(1L);
//
//        HttpEntity<String> entity = new HttpEntity<>(null, new HttpHeaders());
//
//        String endpoint = "/users/1";
//
//        // Act
//        ResponseEntity<String> response = testRestTemplate.exchange(endpoint, HttpMethod.DELETE, entity, String.class);
//
//        // Assert
//        assertEquals(HttpStatus.OK, response.getStatusCode());
//
//        verify(userMockService, times(1)).deleteUserById(1L);
//    }

}
