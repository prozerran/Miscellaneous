package com.sp.app;

import java.io.File;
import java.util.Date;

import org.springframework.scheduling.annotation.Async;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.ZoneId;

import com.sp.app.LogTailer.ReadOrder;

public abstract class BaseImporter implements LogTailerListener {

	private Boolean isDone = false;	
	
	protected LogTailer tailer = null;
	protected DBImporter dba = null;
	
	public void OnFileNotFound(File file) {
		SPCommon.ConsoleLog("FileNotFound: %s", file.getName());		
	}

	public void OnError(Exception ex) {
		SPCommon.ConsoleLog("Error: %s", ex.getMessage());		
	}

	public void OnFileDeleted(File file) {
		SPCommon.ConsoleLog("File Deleted: %s", file.getName());
	}
	
	public void OnStarted() {	
		
		SPCommon.ConsoleLog("Trying to connect to DB...");			
		
		dba = new DBImporter();	    
    		
		if (dba.Connect()) {
			SPCommon.ConsoleLog("Connected to DB.");		
		} else {
			SPCommon.ConsoleLog("Unable to connect to DB.");	
		}			
	}

	public void OnStopped() {
		if (dba != null) {
			dba.Close();
		}
		dba = null;
		isDone = true;		
	}	
	
	public Boolean IsDone() {
		return isDone;
	}
	
	@Async
	private void Start(LocalDate ld, String filepath, ReadOrder readorder) throws InterruptedException {	
				
		File file = new File(filepath);
				
		// file may not exist yet, so need to check
		while (true) {	
				
			// continuously check if file exist for today, if not, break out
			if (readorder != ReadOrder.UNTIL_EOF) {
				if (ld.compareTo(LocalDate.now()) != 0) {
					isDone = true;
					break;	
				}
			}			
			
			if (file.exists() && !file.isDirectory()) { 
				// file finally exist, get out and start parsing
				isDone = false;						
				break;
			}
			
			// file doesn't exist and user manual consolidation parsing, return immediately!
			if (readorder == ReadOrder.UNTIL_EOF) {
				isDone = true;
				break;
			}				
			
			Thread.sleep(60000);	
			SPCommon.ConsoleLog("Searching for file: %s", filepath);
		}
		
		// done, or file not found, do nothing!
		if (isDone == true) {
			SPCommon.ConsoleLog("File doesn't exist: %s", filepath);
			return;
		}
				
		// start parsing
		tailer = new LogTailer(file);	
		tailer.setReadOrder(readorder);
		tailer.RegisterCallback(this);		
		
		new Thread(tailer).start();
	}	
	
	private void Stop() {	
		if (tailer != null) {
			tailer.setStopped(true);
		}
		tailer = null;
	}
	
	// to be implemented on child classes
	public abstract String GetFileName();
	
	public void StartService() {
		
		Date date = new Date();
		LocalDate ld = date.toInstant().atZone(ZoneId.systemDefault()).toLocalDate();
		
		if (ld.getDayOfWeek() == DayOfWeek.SUNDAY) {
			SPCommon.ConsoleLog("Skip Sunday: %s", ld.toString());
			isDone = true;			
			return;
		}						
		
		StartOnDate(ld, ReadOrder.FORWARD);		
	}
	
	public void StartOnDate(LocalDate ld, ReadOrder readorder) {
		
		int year  = ld.getYear();
		int month = ld.getMonthValue();
		int day   = ld.getDayOfMonth(); 		
		
	    String ticker_dir = SPConfig.getInstance().GetProperty("ticker.dir");    	  		
		String filename = String.format(GetFileName() + "_%04d%02d%02d.csv",  year, month, day);	    
		String filepath = String.format("%s/%04d%02d/%s", ticker_dir, year, month, filename);		
		SPCommon.ConsoleLog("Start Importing: %s", filepath);	  
		
		try {			
			// stop previous running
			Stop();
			
			// add some lead time
			Thread.sleep(1000);
			
			// start new
			Start(ld, filepath, readorder);
			
		} catch (Exception e) {
			System.err.println(e.getMessage());				
		}
	}
}

