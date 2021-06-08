package com.sp;

import javax.persistence.EntityManager;

import org.apache.catalina.connector.Connector;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.embedded.tomcat.TomcatServletWebServerFactory;
import org.springframework.boot.web.servlet.server.ServletWebServerFactory;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ImportResource;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.scheduling.annotation.EnableAsync;

import com.sp.app.JPAUtility;
import com.sp.app.SPCommon;

// https://spring.io/guides/gs/caching/
// https://www.javadevjournal.com/spring-boot/how-to-enable-http-https-in-spring-boot/

import com.sp.tasks.ChartScheduler;

@SpringBootApplication
@EnableJpaRepositories
@EnableCaching
@ImportResource("classpath:applicationContext.xml")
public class SpringBootWebApplication {

	//HTTP port
	@Value("${http.port}")
	private int httpPort;	
	
    public static void main(String[] args) {  
    	
		// run once at service startup
		try {
			
			// init
			//JPAUtility.getEntityManager();
			
	    	// start some background task first
			ChartScheduler.ImportTickerData();
			ChartScheduler.ImportChartSecond();

		} catch (InterruptedException e) {
			e.printStackTrace();
		} 
		
		SPCommon.ConsoleLog("Starting ChartServer Service.");
    	   
    	// run web server
        SpringApplication.run(SpringBootWebApplication.class, args);
    }
    
	// Let's configure additional connector to enable support for both HTTP and HTTPS
	@Bean
	public ServletWebServerFactory servletContainer() {
		TomcatServletWebServerFactory tomcat = new TomcatServletWebServerFactory();
		tomcat.addAdditionalTomcatConnectors(createStandardConnector());
		return tomcat;
	}

	private Connector createStandardConnector() {
		Connector connector = new Connector("org.apache.coyote.http11.Http11NioProtocol");
		connector.setPort(httpPort);
		return connector;
	}    
}
