package com.sp.controllers;

import org.springframework.boot.autoconfigure.condition.ConditionalOnExpression;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Homepage controller.
 */
@Controller
@ConditionalOnExpression("${my.controller.enabled:false}")
public class IndexController {

    @RequestMapping("/")
    String index() {
        return "index";
    }

}
