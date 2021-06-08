package com.sp.app;

// https://www.freeformatter.com/cron-expression-generator-quartz.html

public final class AppConstants {
	
	public static enum ChartInterval {
		TICKER,
		SECOND,
		MINUTE,
		HOUR,
		DAILY
	}
	
	// data_mode
	public static final Integer DATAMODE_DATA_ARRAY = 1;
	public static final Integer DATAMODE_TEXT_CSV = 2;
	public static final Integer DATAMODE_JAVASCRIPT = 3;
	public static final Integer DATAMODE_JSON_ARRAY = 4;
	
	/*URL MAPPING CONSTANTS*/
	public static final String URL_MAP_COUNTRY_LIST= "/list/countries";
	public static final String URL_MAP_COUNTRY_READ= "/read/country";	
	
	/*PARAMETER CONSTANTS*/
	public static final String PARAMETER_LANG= "lang";	
	
	/*CACHE NAME CONSTANTS*/
	public static final String CACHE_NAME_COUNTRY_LIST= "country-list";
		
	/*CRON SCHEDULERS*/
	public static final String CRON_EVERY_SECOND = "* * * ? * *";
	public static final String CRON_EVERY_FIVE_SECOND = "*/5 * * ? * *";
	public static final String CRON_EVERY_TWO_SECOND = "*/2 * * * * ?";
	public static final String CRON_EVERY_TWO_MINUTES = "0 0/2 * 1/1 * ?";
	public static final String CRON_EVERY_TWO_HOURS = "0 0 0/2 * * ?";
	public static final String CRON_EVERY_HOUR = "0 0 0/1 * * ?";
	public static final String CRON_EVERYDAY_AT_FIVE_AM = "0 0 5 * * ?";
	public static final String CRON_EVERYDAY_AT_MIDNIGHT = "0 0 0 * * ?";
	//public static final String CRON_EVERYDAY_AFTER_MIDNIGHT = "5 0 0 * * ?";
	public static final String CRON_EVERYDAY_AFTER_MIDNIGHT = "0 5 0 * * ?";
	public static final String CRON_EVERYDAY_AFTER_MARKET_TRADING = "0 0 7 * * ?";
	public static final String CRON_EVERYDAY_HOUSE_CLEAN = "0 30 7 * * ?";
}

