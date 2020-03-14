package com.pragmatest.models;

public class UserResponse extends UserRequest {

    private Long Id;

    public UserResponse() {

        super();
    }

    public UserResponse(Long id, String fullName, String locality, int age) {
        super(fullName, locality, age);
        Id = id;
    }

    public Long getId() {
        return Id;
    }

    public void setId(Long id) {
        Id = id;
    }
}
