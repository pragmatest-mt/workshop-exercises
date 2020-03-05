package com.pragmatest.services;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import static org.junit.jupiter.api.Assertions.assertEquals;

@SpringBootTest
public class UserPortalServiceTest {

    @Autowired
    UserPortalService userPortalService;

    @DisplayName("Test Spring @Autowired Integration")
    @Test
    void testGetName() {
        assertEquals("Clyde Vassallo", userPortalService.getName());
    }
}
