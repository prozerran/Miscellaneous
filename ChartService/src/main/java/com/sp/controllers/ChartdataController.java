
package com.sp.controllers;

import java.time.LocalDate;
import java.time.LocalTime;
import java.util.Date;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.servlet.mvc.method.annotation.StreamingResponseBody;

import com.alibaba.fastjson.JSON;
import com.sp.app.AppConstants;
import com.sp.app.SPCommon;
import com.sp.app.SPConfig;
import com.sp.app.ChartFileImporter;
import com.sp.app.HttpClient;
import com.sp.app.TickerFileImporter;
import com.sp.app.AppConstants.ChartInterval;
import com.sp.entities.ChartdataDaily;
import com.sp.entities.ChartdataHour;
import com.sp.entities.ChartdataMinute;
import com.sp.entities.ChartdataSecond;
import com.sp.entities.ChartdataTicker;
import com.sp.app.LogTailer.ReadOrder;
import com.sp.models.ChartdataData;
import com.sp.services.ChartdataService;

/**
 * ChartdataSecond controller.
 */
@Controller
public class ChartdataController {

    private ChartdataService cdss;

    @Autowired
    public void setChartdataService(ChartdataService chartdataService) {
        this.cdss = chartdataService;
    }   
    
    @RequestMapping("cutofftime/{prod_code}")
    @ResponseBody
    public String getCutOffTime(@PathVariable String prod_code) {
    	LocalTime dt = cdss.getCutOffTime(prod_code);    	
    	return dt.toString();
    }     
    
    @RequestMapping("houseclean")
    @ResponseStatus(code = HttpStatus.OK)
    @Async
    public void doHouseClean() throws InterruptedException {      	
    	cdss.doHouseClean("tb.chartdata_second");
    	cdss.doHouseClean("tb.chartdata_ticker");
    	cdss.doHouseClean("tb.chartdata_minute");
    	cdss.doHouseClean("tb.chartdata_hour");
    	cdss.doHouseClean("tb.chartdata_daily");
    } 
    
    @RequestMapping("evictcache")
    @ResponseStatus(code = HttpStatus.OK)
    @Async
    public void doEvictCache() {
    	cdss.doEvictCache();
    }    
    
    // recover data from remote db:trading [old]
    @RequestMapping("recover/trading/{src_ip_addr}/{from_unixtime}/{to_unixtime}")
    @ResponseStatus(code = HttpStatus.OK)
    @ResponseBody
    public ResponseEntity<String> doRecoveryTrading(	
    		@PathVariable String src_ip_addr, 
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
        
        String url_s = String.format("http://%s/pserver/chartdata_recoverdata.php?prod_code=FULL&data_type=sec&from_time=%s&to_time=%s",
        		src_ip_addr, from_unixtime, to_unixtime);
        
        String url_m = String.format("http://%s/pserver/chartdata_recoverdata.php?prod_code=FULL&data_type=min&from_time=%s&to_time=%s",
        		src_ip_addr, from_unixtime, to_unixtime);
        
        String url_h = String.format("http://%s/pserver/chartdata_recoverdata.php?prod_code=FULL&data_type=hour&from_time=%s&to_time=%s",
        		src_ip_addr, from_unixtime, to_unixtime);
        
        String url_d = String.format("http://%s/pserver/chartdata_recoverdata.php?prod_code=FULL&data_type=day&from_time=%s&to_time=%s",
        		src_ip_addr, from_unixtime, to_unixtime);
    	
    	HttpClient hce = new HttpClient();    	
        
    	// recover for db second,minute,hour,daily
        cdss.putChartDataLegacy(hce.get(url_s), ChartInterval.SECOND);
        cdss.putChartDataLegacy(hce.get(url_m), ChartInterval.MINUTE);
        cdss.putChartDataLegacy(hce.get(url_h), ChartInterval.HOUR); 
        cdss.putChartDataLegacy(hce.get(url_d), ChartInterval.DAILY); 
                 
		return new ResponseEntity<String>("Recovery done!", HttpStatus.OK); 
    }      
        
    // recover data from remote db:trading_chart [new]
    @RequestMapping("recover/trading_chart/{src_ip_addr}/{from_unixtime}/{to_unixtime}")
    @ResponseStatus(code = HttpStatus.OK)
    @ResponseBody
    public ResponseEntity<String> doRecoveryTradingChart(	
    		@PathVariable String src_ip_addr, 
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
    	
        String port = SPConfig.getInstance().GetProperty("http.port");
        
        String url_s = String.format("http://%s:%s/recoverdata/trading_chart/sec/%s/%s",
        		src_ip_addr, port, from_unixtime, to_unixtime);
        
        String url_m = String.format("http://%s:%s/recoverdata/trading_chart/min/%s/%s",
        		src_ip_addr, port, from_unixtime, to_unixtime);
        
        String url_h = String.format("http://%s:%s/recoverdata/trading_chart/hour/%s/%s",
        		src_ip_addr, port, from_unixtime, to_unixtime);
        
        String url_d = String.format("http://%s:%s/recoverdata/trading_chart/day/%s/%s",
        		src_ip_addr, port, from_unixtime, to_unixtime);
    	
    	HttpClient hce = new HttpClient();    	
        
    	// recover for db second,minute,hour,daily
        cdss.setChartDataSecond(hce.get(url_s)); 
        cdss.setChartDataMinute(hce.get(url_m)); 
        cdss.setChartDataHour(hce.get(url_h)); 
        cdss.setChartDataDaily(hce.get(url_d));         
         
		return new ResponseEntity<String>("Recovery done!", HttpStatus.OK); 
    }    
 
    // recover data from remote db:trading_chart [new]
    @RequestMapping("recoverdata/trading_chart/sec/{from_unixtime}/{to_unixtime}")
    @ResponseBody
    public Iterable<ChartdataSecond> doRecoverDataTradingChartSecond(	
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
    	
    	Date from = SPCommon.GetDateTime(from_unixtime);
    	Date to = SPCommon.GetDateTime(to_unixtime);
    	
    	return cdss.getChartDataSecond(from, to); 	   	       
    } 
    
    @RequestMapping("recoverdata/trading_chart/min/{from_unixtime}/{to_unixtime}")
    @ResponseBody
    public Iterable<ChartdataMinute> doRecoverDataTradingChartMinute(	
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
    	
    	Date from = SPCommon.GetDateTime(from_unixtime);
    	Date to = SPCommon.GetDateTime(to_unixtime);
    	
    	return cdss.getChartDataMinute(from, to); 	   	       
    } 
    
    @RequestMapping("recoverdata/trading_chart/hour/{from_unixtime}/{to_unixtime}")
    @ResponseBody
    public Iterable<ChartdataHour> doRecoverDataTradingChartHour(	
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
    	
    	Date from = SPCommon.GetDateTime(from_unixtime);
    	Date to = SPCommon.GetDateTime(to_unixtime);
    	
    	return cdss.getChartDataHour(from, to); 	   	       
    } 
    
    @RequestMapping("recoverdata/trading_chart/day/{from_unixtime}/{to_unixtime}")
    @ResponseBody
    public Iterable<ChartdataDaily> doRecoverDataTradingChartDaily(	
    		@PathVariable String from_unixtime, 
    		@PathVariable String to_unixtime) throws Exception {
    	
    	Date from = SPCommon.GetDateTime(from_unixtime);
    	Date to = SPCommon.GetDateTime(to_unixtime);
    	
    	return cdss.getChartDataDaily(from, to); 	   	       
    }     
        
    // recover data locally from pserver csv files to DB
    @RequestMapping("recover/file/{from_yyyymmdd}/{to_yyyymmdd}")
    @ResponseStatus(code = HttpStatus.OK)
    @Async
    public void doRecoveryFile(
    		@PathVariable String from_yyyymmdd, 
    		@PathVariable String to_yyyymmdd) throws InterruptedException {    	
    	doConsolidation(from_yyyymmdd, to_yyyymmdd, true);    	
    }    
    
    @RequestMapping("consolidate/{from_yyyymmdd}/{to_yyyymmdd}/{add}")
    @ResponseStatus(code = HttpStatus.OK)
    @Async
    public void doConsolidation(
    		@PathVariable String from_yyyymmdd, @PathVariable String to_yyyymmdd,     		
    		@PathVariable Boolean add) throws InterruptedException {  
    	
		LocalDate dt_from = SPCommon.GetDateFromStringFormat(from_yyyymmdd, "yyyyMMdd");   	    	
		LocalDate dt_to = SPCommon.GetDateFromStringFormat(to_yyyymmdd, "yyyyMMdd");
		LocalDate dt = dt_from;
		
		while (!dt.isAfter(dt_to)) {
	
			if (add) {
				// parse files
				ChartFileImporter.getInstance().StartOnDate(dt, ReadOrder.UNTIL_EOF);
				TickerFileImporter.getInstance().StartOnDate(dt, ReadOrder.UNTIL_EOF);	
						
				// wait until parsing finishes
				while (!ChartFileImporter.getInstance().IsDone()) Thread.sleep(5000);
				while (!TickerFileImporter.getInstance().IsDone()) Thread.sleep(5000);				
			}
			
			// mass populate data in chartdata tables
			cdss.consolidateChartDataTables("ChartdataMinute", dt, 60L);
			cdss.consolidateChartDataTables("ChartdataHour", dt, 3600L);	
			cdss.consolidateChartDataTables("ChartdataDaily", dt, 86400L);		
			
			dt = dt.plusDays(1);
		}
		
		// TODO: May need to refactor so that it can be run in parallel
		if (add) {
			// because we used our instance to reparse files, we need to restart the parse service for today
			// Note that recovery of data SHOULD be done after business/trading hours
			TickerFileImporter.getInstance().StartService();
			ChartFileImporter.getInstance().StartService();
		}
    }  
    
    @RequestMapping("ticker/query/{prod_code}/{bars}")     
    @ResponseBody
    public ResponseEntity<String> queryTicker( 
    		HttpServletRequest req, 
    		HttpServletResponse resp,    		
    		@PathVariable String prod_code, @PathVariable Integer bars               
            ) throws Exception
    {    	
    	// query our data from DB/cache
		Iterable<ChartdataTicker> cdt = cdss.getTickerData(prod_code, bars);     	

		// return ticker format
		resp.setContentType("text/html"); 
		return new ResponseEntity<String>(ChartdataTicker.getTickerTextCSV(cdt), HttpStatus.OK);
    }    
    
    //@RequestMapping("ticker/query")
    //URL mapping made compatible with old [php] format
    @RequestMapping("pserver/ticker_query.php")     
    @ResponseBody
    public ResponseEntity<String> getTicker( 
    		HttpServletRequest req, 
    		HttpServletResponse resp,
            @RequestParam String prod_code,
            @RequestParam Integer second,
            @RequestParam(value = "from_time", required = false) String from_time                   
            ) throws Exception
    {
    	// set some limits
    	if (second < 60) second = 60;
    	if (second > 900) second = 900;   
    	
    	if (from_time == null) {
    		long from = SPCommon.GetDayStartDate(0);
    		from_time = Long.toString(from);   		
    	}
    	
    	// to_time not set, set some arbitrary future date
		long future = SPCommon.GetEpochTime(21001231, 0);
		String to_time = Long.toString(future);  
    	
    	Date from = SPCommon.GetDateTime(from_time);
    	Date to = SPCommon.GetDateTime(to_time);
    	
    	// query our data from DB/cache
		Iterable<ChartdataData> cdd = cdss.getChartDataCached(prod_code, second, from, to);     	

		// return ticker format
		resp.setContentType("text/html"); 
		return new ResponseEntity<String>(ChartdataData.getTickerTextCSV(cdd), HttpStatus.OK);
    }       
    
    //@RequestMapping("chartdata/query")
    //URL mapping made compatible with old [php] format
    @RequestMapping("pserver/chartdata_query.php")    
    @ResponseBody	
    public ResponseEntity<StreamingResponseBody> getChartData( 
    		HttpServletRequest req, 
    		HttpServletResponse resp,
            @RequestParam String prod_code,
            @RequestParam Integer second,
            @RequestParam(value = "from_time", required = false) String from_time,
            @RequestParam(value = "to_time", required = false) String to_time,
            @RequestParam(value = "days", required = false) Integer days,
            @RequestParam(value = "data_mode", required = false) Integer data_mode                      
            ) throws Exception
    {
    	// set some limits
    	if (second < 5) second = 5;
    	if (second > 86400) second = 86400;    	
    	
    	if (data_mode == null)
    		data_mode = AppConstants.DATAMODE_TEXT_CSV;   	
    	
    	if (from_time == null && (days == null || days <= 0)) {
    		// seconds: 5, 10, 30, ... get only from chartdata_second
    		// seconds: 60, 3600, ... get 1 day from chartdata_second + 8 day history from respective table    		
    		if (second < 60) 			days = 1;
    		else if (second < 3600)		days = 8;		// minute
    		else if (second < 86400)	days = 91;		// hourly
    		else						days = 15001;	// daily
    	}   
    	
    	if (from_time == null) {
    		long from = SPCommon.GetDayStartDate(days-1);
    		from_time = Long.toString(from);   		
    	}    		
    	
    	// to_time not set, set some arbitrary future date
    	if (to_time == null) {
    		long future = SPCommon.GetEpochTime(21001231, 0);
    		to_time = Long.toString(future);
    	}  
    	
    	Date from = SPCommon.GetDateTime(from_time);
    	Date to = SPCommon.GetDateTime(to_time);
    	
    	// query our data from DB/cache
    	Iterable<ChartdataData> cdd = null;
    	
    	if (second >= 60)    	
    		cdd = cdss.getChartDataCached(prod_code, second, from, to);		// cache-able history version     	
    	else
    		cdd = cdss.getChartDataCurrent(prod_code, second, from, to);	// current/today table only

		// determine which mode to return in
    	if (data_mode == AppConstants.DATAMODE_DATA_ARRAY) {
    		resp.setContentType("text/html"); 
    		String ss = ChartdataData.getChartdataArray(cdd);
    		final StreamingResponseBody body = out -> out.write(ss.toString().getBytes(), 0, ss.length());    		
    		return new ResponseEntity<StreamingResponseBody>(body, HttpStatus.OK);
    	}
    	else if (data_mode == AppConstants.DATAMODE_TEXT_CSV) { // default			
    		resp.setContentType("text/html"); 
    		String ss = ChartdataData.getChartdataTextCSV(cdd);
    		final StreamingResponseBody body = out -> out.write(ss.toString().getBytes(), 0, ss.length());    		
    		return new ResponseEntity<StreamingResponseBody>(body, HttpStatus.OK);
    	}    	
    	else if (data_mode == AppConstants.DATAMODE_JAVASCRIPT) {    			
    		resp.setContentType("text/javascript"); 	// no idea why this is JS in php
    		String ss = ChartdataData.getChartdataJavaScript(cdd);
    		final StreamingResponseBody body = out -> out.write(ss.toString().getBytes(), 0, ss.length());    		
    		return new ResponseEntity<StreamingResponseBody>(body, HttpStatus.OK);	
    	}
    	else if (data_mode == AppConstants.DATAMODE_JSON_ARRAY) {        	
    		resp.setContentType("application/json");   
    		String ss = JSON.toJSONString(cdd);	
    		final StreamingResponseBody body = out -> out.write(ss.getBytes(), 0, ss.length());    		
    		return new ResponseEntity<StreamingResponseBody>(body, HttpStatus.OK);	    		
    	}    	
    	else {	// should not come here    			
    		resp.setContentType("text/html"); 
    		String ss = ChartdataData.getChartdataHeader();
    		final StreamingResponseBody body = out -> out.write(ss.toString().getBytes(), 0, ss.length());    		
    		return new ResponseEntity<StreamingResponseBody>(body, HttpStatus.BAD_REQUEST);    		
    	}
    }
}





































