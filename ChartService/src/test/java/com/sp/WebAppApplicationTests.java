package com.sp;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.test.context.web.WebAppConfiguration;

import com.sp.SpringBootWebApplication;

//import org.springframework.boot.test.SpringApplicationConfiguration;
import org.springframework.test.context.junit4.SpringJUnit4ClassRunner;

//@SuppressWarnings("deprecation")
@RunWith(SpringJUnit4ClassRunner.class)
//@SpringApplicationConfiguration(classes = SpringBootWebApplication.class)
@WebAppConfiguration
public class WebAppApplicationTests {

	@Test
	public void contextLoads() {
	}

}
