package com.sp.models;

import java.math.BigInteger;
import java.util.Calendar;

import org.springframework.cache.annotation.Cacheable;

import com.sp.app.SPCommon;

public class ChartdataData {

	public double O;		// open
	public double H;		// high
	public double L;		// low
	public double C;		// close
	public Integer V;		// volume (qty)
	public long T;			// trade date
	public BigInteger TO;	// turnover
	public Integer CO;		// cutoff	
	
	// data_mode = 1
	@Cacheable("getChartdataArray")		
	public static String getChartdataArray(Iterable<ChartdataData> cdd) {		
		StringBuilder data_str = new StringBuilder("var dataArray = ["); 		
		for (ChartdataData c: cdd) {			
			Calendar cal = Calendar.getInstance();
			cal.setTime(SPCommon.GetDateTime(c.T));
			int hh = cal.get(Calendar.HOUR_OF_DAY);
			int mm = cal.get(Calendar.MINUTE);
			int ss = cal.get(Calendar.SECOND);
						
			data_str.append('[');				
			data_str.append(String.format("\"%02d:%02d:%02d\"", hh, mm, ss));			
			data_str.append(',');
			data_str.append(c.O);
			data_str.append(',');
			data_str.append(c.H);
			data_str.append(',');
			data_str.append(c.L);
			data_str.append(',');
			data_str.append(c.C);
			data_str.append("],"); 			
		}
		data_str.append(']');
		return data_str.toString();
	}	
	
	public static String getChartdataHeader() {
		return "$prev_open,$prev_high,$prev_low,$prev_close,$prev_vol,$prev_time,$data_mode,$cutoff_data\r\n";
	}
	
	// data_mode = 2 [default]	
	@Cacheable("getChartdataTextCSV")		
	public static String getChartdataTextCSV(Iterable<ChartdataData> cdd) { 		
		StringBuilder data_str = new StringBuilder(); 
		for (ChartdataData c: cdd) {			
			data_str.append(c.O);
			data_str.append(',');
			data_str.append(c.H);
			data_str.append(',');
			data_str.append(c.L);
			data_str.append(',');
			data_str.append(c.C);
			data_str.append(',');
			data_str.append(c.V);
			data_str.append(',');
			data_str.append(c.T);
			data_str.append(',');
			data_str.append(c.CO);
			data_str.append("\r\n");		
		}
		
		// get content length	
		int msglen = data_str.length();
			
		// simple hack, just empty result.
		if (msglen == 0) {
			return "19:-1:EMPTY RESULT!";
		}
		
		// return header + data
		int msglencount = (int) (Math.log10(msglen) + 1);
		int total = msglen + msglencount + 1;
		int totalcount = (int) (Math.log10(total) + 1);
		int extra = totalcount - msglencount;
		int checksum = total + extra + 6;	// + :0:0:0:
				
		data_str.insert(0, ":0:0:0:");
		data_str.insert(0,  checksum);		
		return data_str.toString();
	}
		
	// data_mode = 3
	@Cacheable("getChartdataJavaScript")	
	public static String getChartdataJavaScript(Iterable<ChartdataData> cdd) {
		StringBuilder data_str = new StringBuilder("var dataArray = ["); 
		for (ChartdataData c: cdd) {			
			data_str.append('[');				
			data_str.append(c.T);		
			data_str.append("000,");
			data_str.append(c.O);
			data_str.append(',');
			data_str.append(c.H);
			data_str.append(',');
			data_str.append(c.L);
			data_str.append(',');
			data_str.append(c.C);
			data_str.append(',');
			data_str.append(c.V);			
			data_str.append("],");  			
		}
		data_str.append(']');
		return data_str.toString();
	}	
	
	// data_mode = 4
	public static Iterable<ChartdataData> getChartdataJson(Iterable<ChartdataData> cdd) {
		return cdd;		// returning self object is JSON be default
	}
	
	// data_mode = 2 [ticker]
	@Cacheable("getTickerTextCSV")		
	public static String getTickerTextCSV(Iterable<ChartdataData> cdd) { 		
		StringBuilder data_str = new StringBuilder();   		
		for (ChartdataData c: cdd) {
			data_str.append(c.O);
			data_str.append(',');
			data_str.append(c.H);
			data_str.append(',');
			data_str.append(c.L);
			data_str.append(',');
			data_str.append(c.C);
			data_str.append(',');
			data_str.append(c.V);
			data_str.append(',');
			data_str.append(c.T);
			data_str.append(",0,");
			data_str.append(c.CO);
			data_str.append("\r\n");			
		}		
		return data_str.toString();
	}	
}
