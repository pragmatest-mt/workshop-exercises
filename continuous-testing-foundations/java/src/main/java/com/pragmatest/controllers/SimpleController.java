package com.pragmatest.controllers;

import com.pragmatest.services.UserPortalService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
public class SimpleController {

    @Autowired
    UserPortalService userPortalService;

    @ResponseBody
    @GetMapping("/")
    public String showFullName() {
        return userPortalService.getName();
    }
}
