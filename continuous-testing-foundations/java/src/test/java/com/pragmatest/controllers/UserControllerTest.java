package com.pragmatest.controllers;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.pragmatest.models.User;
import com.pragmatest.services.UserService;
import com.pragmatest.utils.UserMatcher;
import org.json.JSONException;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.skyscreamer.jsonassert.JSONAssert;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.http.*;
import org.springframework.test.context.ActiveProfiles;

import java.util.Arrays;
import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@ActiveProfiles("test")
public class UserControllerTest {

    private static final ObjectMapper om = new ObjectMapper();

    @Autowired
    private TestRestTemplate testRestTemplate;

    @MockBean
    private UserService userMockService;

    @BeforeEach
    public void init() {
        // Arrange
        User user = new User(1L, "John Smith", "London", 23);
        when(userMockService.getUserById(1L)).thenReturn(Optional.of(user));
    }

    @Test
    public void testGetUserByIdValidId() throws JSONException {
        // Arrange
        String expectedResponseBody = "{id:1, fullName:\"John Smith\", locality:\"London\", age:23}";
        String endpoint = "/users/1";

        // Act
        ResponseEntity<String> responseEntity = testRestTemplate.getForEntity(endpoint, String.class);

        // Assert
        assertEquals(HttpStatus.OK, responseEntity.getStatusCode());
        assertEquals(MediaType.APPLICATION_JSON_UTF8, responseEntity.getHeaders().getContentType());

        JSONAssert.assertEquals(expectedResponseBody, responseEntity.getBody(), false);

        verify(userMockService, times(1)).getUserById(1L);
    }

    @Test
    public void testGetUsersByIdValidIds() throws JsonProcessingException, JSONException {
        // Arrange
        List<User> users = Arrays.asList(
                new User(1L, "John Smith", "London", 23),
                new User(2L, "Mary Walsh", "Liverpool", 30));

        when(userMockService.getAllUsers()).thenReturn(users);

        String expectedUserList = om.writeValueAsString(users);
        String endpoint = "/users";

        // Act
        ResponseEntity<String> responseEntity = testRestTemplate.getForEntity(endpoint, String.class);

        // Assert
        assertEquals(HttpStatus.OK, responseEntity.getStatusCode());
        assertEquals(MediaType.APPLICATION_JSON_UTF8, responseEntity.getHeaders().getContentType());

        JSONAssert.assertEquals(expectedUserList, responseEntity.getBody(), false);

        verify(userMockService, times(1)).getAllUsers();
    }

    @Test
    public void testGetUserByIdInvalidId() throws Exception {
        // Arrange
        String expectedResponseBody = "{status:404,error:\"Not Found\",message:\"User with ID '5' not found.\",path:\"/users/5\"}";

        String endpoint = "/users/5";

        // Act
        ResponseEntity<String> response = testRestTemplate.getForEntity(endpoint, String.class);

        // Assert
        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
        JSONAssert.assertEquals(expectedResponseBody, response.getBody(), false);
    }

    @Test
    public void testSaveUserValidUser() throws Exception {
        // Arrange
        User newUser = new User(1L, "Marisa Jones", "Newcastle", 20);
        when(userMockService.saveUser(argThat(new UserMatcher(newUser)))).thenReturn(Optional.of(newUser));

        String expectedResponseBody = om.writeValueAsString(newUser);

        String endpoint = "/users";

        // Act
        ResponseEntity<String> response = testRestTemplate.postForEntity(endpoint, newUser, String.class);
//        userController.newUser(newUser);

        // Assert
        assertEquals(HttpStatus.CREATED, response.getStatusCode());
        JSONAssert.assertEquals(expectedResponseBody, response.getBody(), false);

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(newUser)));
    }

    @Test
    public void testSaveUserInvalidUser() throws JSONException {
        // Arrange
        User newUser = new User(1L, "Jane Stark", "Newcastle", 17);
        when(userMockService.saveUser(argThat(new UserMatcher(newUser)))).thenReturn(Optional.empty());

        String expectedResponseBody = "{status:400,error:\"Bad Request\",message:\"Invalid User\"}";

        String endpoint = "/users";

        // Act
        ResponseEntity<String> response = testRestTemplate.postForEntity(endpoint, newUser, String.class);

        //Assert
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
        JSONAssert.assertEquals(expectedResponseBody, response.getBody(), false);

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(newUser)));
    }


    @Test
    public void testUpdateUserValidUser() throws Exception {
        // Arrange
        User updateUser = new User(1L, "Peter Marshall", "London", 40);
        when(userMockService.saveUser(argThat(new UserMatcher(updateUser)))).thenReturn(Optional.of(updateUser));

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);

        HttpEntity<String> entity = new HttpEntity<>(om.writeValueAsString(updateUser), headers);

        String endpoint = "/users/1";

        // Act
        ResponseEntity<String> response = testRestTemplate.exchange(endpoint, HttpMethod.PUT, entity, String.class);

        // Assert
        assertEquals(HttpStatus.OK, response.getStatusCode());
        JSONAssert.assertEquals(om.writeValueAsString(updateUser), response.getBody(), false);

        verify(userMockService, times(1)).getUserById(1L);
        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(updateUser)));
    }

    @Test
    public void testDeleteUserValidUser() {
        // Arrange
        doNothing().when(userMockService).deleteUserById(1L);

        HttpEntity<String> entity = new HttpEntity<>(null, new HttpHeaders());

        String endpoint = "/users/1";

        // Act
        ResponseEntity<String> response = testRestTemplate.exchange(endpoint, HttpMethod.DELETE, entity, String.class);

        // Assert
        assertEquals(HttpStatus.OK, response.getStatusCode());

        verify(userMockService, times(1)).deleteUserById(1L);
    }
}
