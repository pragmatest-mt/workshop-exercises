package com.pragmatest.services;

import com.pragmatest.repositories.UserPortalRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.springframework.boot.test.context.SpringBootTest;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.when;

@SpringBootTest
public class UserPortalMockServiceTest {

    @Mock
    private UserPortalRepository userPortalRepository;

    @BeforeEach
    void setMockOutput() {
        when(userPortalRepository.getName()).thenReturn("John Smith");
    }

    @DisplayName("Test with Mockito")
    @Test
    void testGetName() {
        assertEquals("John Smith", userPortalRepository.getName());
    }


}
