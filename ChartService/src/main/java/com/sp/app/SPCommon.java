package com.sp.app;

import java.sql.Timestamp;
import java.text.DateFormat;
import java.text.NumberFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Calendar;
import java.util.Date;

public final class SPCommon {
	
	public static long GetEpochTime(int yyyyMMdd, int hhmmss) throws ParseException {	
        String dateString = String.format("%08d%06d", yyyyMMdd, hhmmss);
        DateFormat dateFormat = new SimpleDateFormat("yyyyMMddHHmmss");        
        java.util.Date dt = dateFormat.parse(dateString);                   
        return dt.getTime() / 1000;			
	}
	
	public static Timestamp GetTimeStamp(int yyyyMMdd, int hhmmss) throws ParseException {	
        String dateString = String.format("%08d%06d", yyyyMMdd, hhmmss);
        DateFormat dateFormat = new SimpleDateFormat("yyyyMMddHHmmss");        
        java.util.Date dt = dateFormat.parse(dateString);                 
        return new Timestamp(dt.getTime());        		
	}	
	
	public static Date GetDateTime(long epochtime) {		
		return new java.util.Date((long) epochtime * 1000);
	}
	
	public static Date GetDateTime(String epochtime) {		
		return GetDateTime(Long.parseLong(epochtime));
	}	
	
	public static void ConsoleLog(String format, Object... arguments) {
		DateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		Date date = new Date();						
		String output = String.format("%s %s", dateFormat.format(date), String.format(format, arguments));			
		System.out.println(output);
	}
	
	public static LocalDate GetDateFromStringFormat(String date_string, String format) {
		DateTimeFormatter formatter = DateTimeFormatter.ofPattern(format);
	    return LocalDate.parse(date_string, formatter);
	}
	
	public static long GetDayStartDate(int days_ago) {
		Calendar today = Calendar.getInstance();
		today.add(Calendar.DATE, -days_ago);		
		today.set(Calendar.HOUR_OF_DAY, 0);
		today.set(Calendar.MINUTE, 0);
		today.set(Calendar.SECOND, 0);
		today.set(Calendar.MILLISECOND, 0);
		return today.getTimeInMillis() / 1000;
	}
	
	public static String currencyFormat(double n) {
	    return NumberFormat.getCurrencyInstance().format(n);
	}	
}

