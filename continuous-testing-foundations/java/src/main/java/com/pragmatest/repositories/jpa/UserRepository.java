package com.pragmatest.repositories.jpa;

import com.pragmatest.models.User;
import org.springframework.data.jpa.repository.JpaRepository;

public interface UserRepository extends JpaRepository<User, Long> {
}
