package com.sp.app;

import java.sql.*;
import java.text.ParseException;

public class DBImporter {	
	
	private Connection conn = null;	
	private PreparedStatement ps_s = null;	// chartdata_second [5s]
	private PreparedStatement ps_t = null;	// chartdata_ticker
	
	private String driver = null;
	private String url = null;
	private String user = null;
	private String pass = null;
    
    DBImporter() {
    	try {    		
    		this.driver = SPConfig.getInstance().GetProperty("spring.datasource.driver");
    		this.url = SPConfig.getInstance().GetProperty("spring.datasource.url");
    		this.user = SPConfig.getInstance().GetProperty("spring.datasource.username");
    		this.pass = SPConfig.getInstance().GetProperty("spring.datasource.password");     		
    		
			Class.forName(this.driver);
		} catch (ClassNotFoundException e) {
			System.err.println(e.getMessage());
		}	   	    	   
    }
	
	boolean Connect() {
		try {									
			DriverManager.setLoginTimeout(10);			
			conn = DriverManager.getConnection(url, user, pass);					
			this.PrepareStatements();
			
		} catch (SQLException e) {
			System.err.println(e.getMessage());
			return false;
		}		
		return (conn != null ? true : false);
	}
	
	boolean IsConnected() {		
		try {
			return conn.isValid(0);
		} catch (SQLException e) {
			System.err.println(e.getMessage());
		}
		return false;
	}
	
	boolean Close() {
		try {			
			ps_s.close();
			ps_t.close();
			
			conn.close();
		} catch (SQLException e) {
			System.err.println(e.getMessage());
			return false;
		}
		return true;
	}
	
	// prepare statement separately before inserts for efficiency
	private void PrepareStatements() throws SQLException {
    	// prepare chartdata sql
        String sql_c = "REPLACE INTO chartdata_second "
      		+ " (rec_id, mkt_datetime, prod_code, open, high, low, close, prev_close, qty, turnover, instmnt_code) "
      		+ " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";        
        ps_s = conn.prepareStatement(sql_c); 
        
        // prepare ticker sql
        String sql_t = "REPLACE INTO chartdata_ticker (rec_id, mkt_datetime, prod_code, price, qty, deal_src, side) "
        	+ " VALUES (?, ?, ?, ?, ?, ?, ?)";
	 	ps_t = conn.prepareStatement(sql_t); 		 	
	}
	
	boolean InsertTickerData(long rec_id, int date, int time, String prodcode, double price, int qty, int deal_src, String side) {		     
      try {       	     	      	     
          // prepare statement	      
	      ps_t.clearParameters();
	      ps_t.setLong(1, rec_id);
	      ps_t.setTimestamp(2, SPCommon.GetTimeStamp(date, time));
	      ps_t.setString(3, prodcode);     
	      ps_t.setDouble(4, price);
	      ps_t.setInt(5, qty);
	      ps_t.setInt(6, deal_src);
	      ps_t.setString(7, side);
	      ps_t.execute();
	      	      
	      return true;	      
	      
		} catch (SQLException e) {			
			String msg = e.getMessage();			
			if (!msg.contains("Duplicate entry"))				
				System.err.println(msg);
			return false;
		} catch (ParseException e) {
			System.err.println(e.getMessage());
			return false;
		}
	}	
	
	boolean InsertChartSecond(long rec_id, int date, int time, String prodcode, double open, double high, 
			double low, double close, double prev_close, int qty, long turnover, String instmnt_code) {			           
      try {   
          // prepare statement	   
    	  ps_s.clearParameters();
    	  ps_s.setLong(1, rec_id);    	  
    	  ps_s.setTimestamp(2, SPCommon.GetTimeStamp(date, time));
    	  ps_s.setString(3, prodcode);     
    	  ps_s.setDouble(4, open);
    	  ps_s.setDouble(5, high);
    	  ps_s.setDouble(6, low);
	      ps_s.setDouble(7, close);
	      ps_s.setDouble(8, prev_close);	     	     
	      ps_s.setInt(9, qty);
	      ps_s.setLong(10, turnover);
	      ps_s.setString(11, instmnt_code);   	      
	      ps_s.execute();
	      
	      return true;	      
	      
		} catch (SQLException e) {			
			String msg = e.getMessage();			
			if (!msg.contains("Duplicate entry"))				
				System.err.println(msg);
			return false;
		} catch (ParseException e) {
			System.err.println(e.getMessage());
			return false;
		}
	}
}

