package com.pragmatest.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.pragmatest.matchers.UserMatcher;
import com.pragmatest.models.User;
import com.pragmatest.models.UserRequest;
import com.pragmatest.services.UserService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.context.ActiveProfiles;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;

import java.util.Arrays;
import java.util.List;
import java.util.Optional;

import static org.hamcrest.Matchers.hasSize;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@SpringBootTest()
@ActiveProfiles("test")
@AutoConfigureMockMvc
public class UserControllerIntegrationTestMockMvc {

    private static final ObjectMapper om = new ObjectMapper();

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private UserService userMockService;

      @Test
      public void testGetUserByIdValidId() throws Exception {
          // Arrange
          User user = new User("John Smith", "London", 23);
          when(userMockService.getUserById(1L)).thenReturn(Optional.of(user));
          String endpoint = "/users/1";

          // Act
          ResultActions perform = mockMvc.perform(get(endpoint));

          // Assert
          perform.andExpect(status().isOk())
              .andExpect(content().contentType(MediaType.APPLICATION_JSON_UTF8))
              .andExpect(jsonPath("$.fullName").value("John Smith"))
              .andExpect(jsonPath("$.locality").value("London"))
              .andExpect(jsonPath("$.age").value(23));

          verify(userMockService, times(1)).getUserById(1L);
      }

    @Test
    public void testGetUsersByIdValidIds() throws Exception {
        // Arrange
        List<User> users = Arrays.asList(
                new User("John Smith", "London", 23),
                new User("Mary Walsh", "Liverpool", 30));

        when(userMockService.getAllUsers()).thenReturn(users);

        String endpoint = "/users";

        // Act
        ResultActions perform = mockMvc.perform(get(endpoint));

        // Assert
        perform.andExpect(status().isOk())
                .andExpect(content().contentType(MediaType.APPLICATION_JSON_UTF8))
                .andExpect(jsonPath("$", hasSize(2)))
                .andExpect(jsonPath("$[0].fullName").value("John Smith"))
                .andExpect(jsonPath("$[0].locality").value("London"))
                .andExpect(jsonPath("$[0].age").value(23))
                .andExpect(jsonPath("$[1].fullName").value("Mary Walsh"))
                .andExpect(jsonPath("$[1].locality").value("Liverpool"))
                .andExpect(jsonPath("$[1].age").value(30));

        verify(userMockService, times(1)).getAllUsers();
    }

    @Test
    public void testGetUserByIdInvalidId() throws Exception {
        // Arrange
        //String expectedResponseBody = "{status:404,error:\"Not Found\",message:\"User with ID '5' not found.\",path:\"/users/5\"}";
        String endpoint = "/users/5";

        // Act
        ResultActions perform = mockMvc.perform(get(endpoint));

        // Assert
        perform.andExpect(status().isNotFound());

        verify(userMockService, times(1)).getUserById(5L);
    }

    @Test
    public void testSaveUserValidUser() throws Exception {
        // Arrange
        UserRequest newUserRequest = new UserRequest();
        newUserRequest.setFullName("Marisa Jones");
        newUserRequest.setLocality("Newcastle");
        newUserRequest.setAge(20);

        User expectedServiceInput = new User("Marisa Jones", "Newcastle", 20);

        User mockedServiceOutput = new User("Marisa Jones", "Newcastle", 20);
        mockedServiceOutput.setId(1L);

        when(userMockService.saveUser(argThat(new UserMatcher(expectedServiceInput)))).thenReturn(Optional.of(mockedServiceOutput));

        String request = om.writeValueAsString(newUserRequest);

        String endpoint = "/users";

        // Act
        ResultActions perform = mockMvc.perform(
                post(endpoint)
                .contentType(MediaType.APPLICATION_JSON_UTF8)
                .content(request)
                .accept(MediaType.APPLICATION_JSON_UTF8));

        // Assert
        perform.andExpect(status().isCreated())
                .andExpect(jsonPath("$.fullName").value("Marisa Jones"))
                .andExpect(jsonPath("$.locality").value("Newcastle"))
                .andExpect(jsonPath("$.age").value(20))
                .andExpect(jsonPath("$.id").value(1));

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(expectedServiceInput)));
    }

    @Test
    public void testSaveUserInvalidUser() throws Exception {
        // Arrange
        UserRequest newUserRequest = new UserRequest();
        newUserRequest.setFullName("Michelle Waters");
        newUserRequest.setLocality("Dubai");
        newUserRequest.setAge(17);
        String request = om.writeValueAsString(newUserRequest);

        User expectedServiceInput = new User("Michelle Waters", "Dubai", 17);
        when(userMockService.saveUser(argThat(new UserMatcher(expectedServiceInput)))).thenReturn(Optional.empty());

        String endpoint = "/users";

        // Act
        ResultActions perform = mockMvc.perform(
                post(endpoint)
                .contentType(MediaType.APPLICATION_JSON_UTF8)
                .content(request)
                .accept(MediaType.APPLICATION_JSON_UTF8));

        //Assert
        perform.andExpect(status().isBadRequest());

        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(expectedServiceInput)));
    }

    @Test
    public void testUpdateUserValidUser() throws Exception {
        // Arrange
        UserRequest updateUserRequest = new UserRequest();
        updateUserRequest.setFullName("Marisa Jones");
        updateUserRequest.setLocality("Newcastle");
        updateUserRequest.setAge(20);
        String request = om.writeValueAsString(updateUserRequest);

        long updateUserId = 1L;

        User user = new User("Marisa Jones", "Newcastle", 20);
        user.setId(updateUserId);

        when(userMockService.getUserById(updateUserId)).thenReturn(Optional.of(user));
        when(userMockService.saveUser(argThat(new UserMatcher(user)))).thenReturn(Optional.of(user));

        String endpoint = "/users/" + updateUserId;

        // Act
        ResultActions perform = mockMvc.perform(
                put(endpoint)
                .contentType(MediaType.APPLICATION_JSON_UTF8)
                .content(request)
                .accept(MediaType.APPLICATION_JSON_UTF8));

        // Assert
        perform.andExpect(status().isOk())
                .andExpect(jsonPath("$.fullName").value("Marisa Jones"))
                .andExpect(jsonPath("$.locality").value("Newcastle"))
                .andExpect(jsonPath("$.age").value(20))
                .andExpect(jsonPath("$.id").value(updateUserId));

        verify(userMockService, times(1)).getUserById(updateUserId);
        verify(userMockService, times(1)).saveUser(argThat(new UserMatcher(user)));
    }

    @Test
    public void testDeleteUserValidUser() throws Exception {
        // Arrange
        doNothing().when(userMockService).deleteUserById(1L);

        long deleteUserId = 1L;
        String endpoint = "/users/" + deleteUserId;

        // Act
        ResultActions perform = mockMvc.perform(delete(endpoint));

        // Assert
        perform.andExpect(status().isOk());

        verify(userMockService, times(1)).deleteUserById(1L);
    }

}
