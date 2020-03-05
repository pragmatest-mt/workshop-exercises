package com.pragmatest.utils;

public class UserUtils {


    public String generateFullName(String firstName, String lastName) {
        return firstName + " " + lastName;
    }

    public boolean isOver18(int age) {
        if (age == 0) throw new IllegalArgumentException();

        return (age >= 18);
    }
}
