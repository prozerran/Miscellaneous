package com.sp.tasks;

import com.sp.app.AppConstants;
import com.sp.app.ChartFileImporter;
import com.sp.app.ChartSchedulerService;
import com.sp.app.TickerFileImporter;

import java.net.MalformedURLException;

import org.springframework.stereotype.Service;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.Scheduled;

// https://www.freeformatter.com/cron-expression-generator-quartz.html

@Service
public class ChartScheduler {	
	
	// every 5 second	
	@Scheduled(cron = AppConstants.CRON_EVERY_FIVE_SECOND)
	public static void ServerHeartBeat() throws InterruptedException {
		//SPCommon.ConsoleLog("ServerHeartBeat - Alive: " + new Date());
	}	
	
	// import chartdata_xxxx.csv file into chartdata_second table
	@Async
	@Scheduled(cron = AppConstants.CRON_EVERYDAY_AFTER_MIDNIGHT)
	public static void ImportChartSecond() throws InterruptedException {
		ChartFileImporter.getInstance().StartService();					
	}
	
	// import ticker_xxxx.csv file into chartdata_ticker table
	@Async	
	@Scheduled(cron = AppConstants.CRON_EVERYDAY_AFTER_MIDNIGHT)
	public static void ImportTickerData() throws InterruptedException {
		TickerFileImporter.getInstance().StartService();
	}
	
	// every day 07:00am
	// consolidate all chartdata at end of trading day
	@Async		
	@Scheduled(cron = AppConstants.CRON_EVERYDAY_AFTER_MARKET_TRADING)
	public static void ConsolidateChartData() throws InterruptedException, MalformedURLException {
		ChartSchedulerService.getInstance().ConsolidateChartData();
	}	
	
	// every day 07:00am
	// clean up db table regularly
	@Async		
	@Scheduled(cron = AppConstants.CRON_EVERYDAY_HOUSE_CLEAN)
	public static void HouseClean() throws InterruptedException, MalformedURLException {
		ChartSchedulerService.getInstance().HouseClean();
	}
}
