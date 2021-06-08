
package com.sp.app;

import java.io.InputStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

class Response {
    private InputStream body;

    public Response(InputStream body) {
        this.body = body;
    }

    public InputStream getBody() {
        return body;
    }
}

class Request implements Callable<Response> {
    private URL url;

    public Request(URL url) {
        this.url = url;
    }

    @Override
    public Response call() throws Exception {
        return new Response(url.openStream());
    }
}

public class ChartSchedulerService {
	
    private static ChartSchedulerService _instance = null; 	    
	private static ExecutorService _executor = null;
	  
    private ChartSchedulerService() {} 
  
    public static ChartSchedulerService getInstance() { 
        if (_instance == null) {
        	_instance = new ChartSchedulerService();
        	_executor = Executors.newFixedThreadPool(1); 
        }
        return _instance; 
    }	
    
    public void Delete() {
		_executor.shutdown();
		_executor = null;
		_instance = null;
    }
    
    public void ConsolidateChartData() throws MalformedURLException {    	    	
        LocalDate date = LocalDate.now().minusDays(1);
        DateTimeFormatter formatters = DateTimeFormatter.ofPattern("uuuuMMdd");
        String date_str = date.format(formatters);        
        String port = SPConfig.getInstance().GetProperty("http.port");
					
        // async get request
		String url = String.format("http://localhost:%s/consolidate/%s/%s/false", port, date_str, date_str);
		_executor.submit(new Request(new URL(url)));    	
    }
    
    public void HouseClean() throws MalformedURLException {      
        String port = SPConfig.getInstance().GetProperty("http.port");
					
        // async get request
		String url = String.format("http://localhost:%s/houseclean", port);
		_executor.submit(new Request(new URL(url)));     	
    }      
}


