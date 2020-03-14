package com.pragmatest.models;


public class UserRequest {

    private String fullName;
    private String locality;
    private int age;

    public UserRequest() {

    }

    public UserRequest(String fullName, String locality, int age) {
        this.fullName = fullName;
        this.locality = locality;
        this.age = age;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getLocality() {
        return locality;
    }

    public void setLocality(String locality) {
        this.locality = locality;
    }

    public int getAge() {
        return age;
    }

    public void setAge(int age) {
        this.age = age;
    }
}
