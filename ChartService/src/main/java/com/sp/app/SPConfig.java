package com.sp.app;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class SPConfig {
	 
    private static SPConfig _instance = null; 
    private InputStream config_file = null;
    private Properties prop;
  
    private SPConfig() { 
    	try {    		
    		config_file = new FileInputStream("src/main/resources/application.properties");
			prop = new Properties();
			prop.load(config_file);			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    } 
  
    public static SPConfig getInstance() { 
        if (_instance == null) 
        	_instance = new SPConfig(); 
  
        return _instance; 
    }
	
    public void SetProperty(String key, String value) {
    	prop.setProperty(key, value);
    }
    
    public String GetProperty(String key) {
    	return prop.getProperty(key);
    }
}


