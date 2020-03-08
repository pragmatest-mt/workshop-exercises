package com.pragmatest.controllers;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.pragmatest.models.User;
import com.pragmatest.repositories.jpa.UserRepository;
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
    private UserRepository userMockRepository;

    @BeforeEach
    public void init() {
        User user = new User(1L, "John Smith", "London", 23);
        when(userMockRepository.findById(1L)).thenReturn(Optional.of(user));
    }

    @Test
    public void findUserIdOK() throws JSONException {
        String expected = "{id:1, fullName:\"John Smith\", locality:\"London\", age:23}";

        ResponseEntity<String> responseEntity = testRestTemplate.getForEntity("/users/1", String.class);

        assertEquals(HttpStatus.OK, responseEntity.getStatusCode());
        assertEquals(MediaType.APPLICATION_JSON_UTF8, responseEntity.getHeaders().getContentType());

        JSONAssert.assertEquals(expected, responseEntity.getBody(), false);

        verify(userMockRepository, times(1)).findById(1L);
    }

    @Test
    public void findAllUsersOK() throws JsonProcessingException, JSONException {
        List<User> users = Arrays.asList(
                new User(1L, "John Smith", "London", 23),
                new User(1L, "Mary Walsh", "Liverpool", 30));

        when(userMockRepository.findAll()).thenReturn(users);

        String expected = om.writeValueAsString(users);

        ResponseEntity<String> responseEntity = testRestTemplate.getForEntity("/users", String.class);

        assertEquals(HttpStatus.OK, responseEntity.getStatusCode());
        assertEquals(MediaType.APPLICATION_JSON_UTF8, responseEntity.getHeaders().getContentType());

        JSONAssert.assertEquals(expected, responseEntity.getBody(), false);

        verify(userMockRepository, times(1)).findAll();
    }

    @Test
    public void findUserIdNotFound404() throws Exception {

        String expected = "{status:404,error:\"Not Found\",message:\"User with ID '5' not found.\",path:\"/users/5\"}";

        ResponseEntity<String> response = testRestTemplate.getForEntity("/users/5", String.class);

        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
        JSONAssert.assertEquals(expected, response.getBody(), false);
    }

    @Test
    public void saveUserOK() throws Exception {
        User newUser = new User(1L, "Marisa Jones", "Newcastle", 20);
        when(userMockRepository.save(any(User.class))).thenReturn(newUser);

        String expected = om.writeValueAsString(newUser);

        ResponseEntity<String> response = testRestTemplate.postForEntity("/users", newUser, String.class);

        assertEquals(HttpStatus.CREATED, response.getStatusCode());
        JSONAssert.assertEquals(expected, response.getBody(), false);

        verify(userMockRepository, times(1)).save(any(User.class));
    }

    @Test
    public void updateUserOK() throws Exception {
        User updateUser = new User(1L, "Peter Marshall", "London", 40);
        when(userMockRepository.save(any(User.class))).thenReturn(updateUser);

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);

        HttpEntity<String> entity = new HttpEntity<>(om.writeValueAsString(updateUser), headers);

        ResponseEntity<String> response = testRestTemplate.exchange("/users/1", HttpMethod.PUT, entity, String.class);

        assertEquals(HttpStatus.OK, response.getStatusCode());
        JSONAssert.assertEquals(om.writeValueAsString(updateUser), response.getBody(), false);

        verify(userMockRepository, times(1)).findById(1L);
        verify(userMockRepository, times(1)).save(any(User.class));
    }

    @Test
    public void patchUserLocalityOK() throws JSONException {
        when(userMockRepository.save(any(User.class))).thenReturn(new User());

        String patchInJson = "{\"locality\":\"Chicago\"}";

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        HttpEntity<String> entity = new HttpEntity<>(patchInJson, headers);

        ResponseEntity<String> response = testRestTemplate.exchange("/users/1", HttpMethod.PUT, entity, String.class);

        assertEquals(HttpStatus.OK, response.getStatusCode());

        verify(userMockRepository, times(1)).findById(1L);
        verify(userMockRepository, times(1)).save(any(User.class));
    }

    @Test
    public void patchUserFullName405() throws JSONException {
        String expected = "{status:405,error:\"Method Not Allowed\",message:\"No update is allowed on fields '[fullName]'\"}";

        String patchInJson = "{\"fullName\":\"Michael Styles\"}";

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        HttpEntity<String> entity = new HttpEntity<>(patchInJson, headers);

        ResponseEntity<String> response = testRestTemplate.exchange("/users/1", HttpMethod.PATCH, entity, String.class);

        assertEquals(HttpStatus.METHOD_NOT_ALLOWED, response.getStatusCode());
        JSONAssert.assertEquals(expected, response.getBody(), false);

        verify(userMockRepository, times(1)).findById(1L);
        verify(userMockRepository, times(0)).save(any(User.class));
    }

    @Test
    public void deleteUserOK(){
        doNothing().when(userMockRepository).deleteById(1L);

        HttpEntity<String> entity = new HttpEntity<>(null, new HttpHeaders());
        ResponseEntity<String> response = testRestTemplate.exchange("/users/1", HttpMethod.DELETE, entity, String.class);

        assertEquals(HttpStatus.OK, response.getStatusCode());

        verify(userMockRepository, times(1)).deleteById(1L);
    }
}
