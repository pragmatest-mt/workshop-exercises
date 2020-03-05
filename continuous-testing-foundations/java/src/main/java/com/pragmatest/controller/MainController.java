package com.pragmatest.controller;

import com.pragmatest.repository.UserPortalRepository;
import com.pragmatest.services.UserPortalService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class MainController {

    @Autowired
    UserPortalService userPortalService;

    @ResponseBody
    @GetMapping("/")
    public String hello() {
        return userPortalService.getName();
    }
}
