package com.pragmatest.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.pragmatest.models.User;
import com.pragmatest.models.UserRequest;
import com.pragmatest.models.UserResponse;
import com.pragmatest.services.UserService;
import org.junit.jupiter.api.Test;
import org.skyscreamer.jsonassert.JSONAssert;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.http.*;
import org.springframework.test.context.ActiveProfiles;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@ActiveProfiles("test")
public class UserControllerIntegrationTest {

    private static final ObjectMapper om = new ObjectMapper();

    @Autowired
    private TestRestTemplate testRestTemplate;

    @MockBean
    private UserService userMockService;

    @Test
    public void testCreateValidUser() throws Exception {
        UserRequest userRequest = new UserRequest("Joe Borg", "Mosta", 20);

        UserResponse expectedUserResponse = new UserResponse(1L, "Joe Borg", "Mosta", 20);

        String expectedResponseBody = om.writeValueAsString(expectedUserResponse);

        String endpoint = "/users";

        User serviceUser = new User(1L, "Joe Borg", "Mosta", 20);
        serviceUser.setIsActive(true);

        when(userMockService.saveUser(any(User.class))).thenReturn(serviceUser);

        ResponseEntity<String> responseEntity =
                testRestTemplate.postForEntity(endpoint, userRequest, String.class);

        assertEquals(HttpStatus.CREATED, responseEntity.getStatusCode());
        JSONAssert.assertEquals(expectedResponseBody, responseEntity.getBody(), true);
    }


    @Test
    public void testCreateUnderAgeUser(){
        UserRequest userRequest = new UserRequest("Joe Borg", "Mosta", 0);

        String endpoint = "/users";

        ResponseEntity<String> responseEntity =
                testRestTemplate.postForEntity(endpoint, userRequest, String.class);

        assertEquals(HttpStatus.BAD_REQUEST, responseEntity.getStatusCode());
    }

    @Test
    public void testCreateOverAgeUser(){
        UserRequest userRequest = new UserRequest("Claire Cini", "Qormi", 123);

        String endpoint = "/users";

        ResponseEntity<String> responseEntity =
                testRestTemplate.postForEntity(endpoint, userRequest, String.class);

        assertEquals(HttpStatus.BAD_REQUEST, responseEntity.getStatusCode());
    }

}
