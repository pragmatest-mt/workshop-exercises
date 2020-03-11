package com.pragmatest.utils;

import org.springframework.stereotype.Component;

@Component
public class UserUtils {

    public boolean isAdult(int age) {
        if (age == 0) throw new IllegalArgumentException();

        return (age >= 18);
    }
}
