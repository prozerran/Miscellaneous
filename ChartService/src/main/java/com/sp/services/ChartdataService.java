package com.sp.services;

import java.time.LocalDate;
import java.time.LocalTime;
import java.util.Date;

import com.sp.app.AppConstants.ChartInterval;
import com.sp.entities.ChartdataDaily;
import com.sp.entities.ChartdataHour;
import com.sp.entities.ChartdataMinute;
import com.sp.entities.ChartdataSecond;
import com.sp.entities.ChartdataTicker;
import com.sp.models.ChartdataData;

public interface ChartdataService {        
    
	// general data
    Iterable<ChartdataData> getChartDataCurrent(String prodcode, Integer second, Date from, Date to);    
    Iterable<ChartdataData> getChartDataCached(String prodcode, Integer second, Date from, Date to);      
    Iterable<ChartdataTicker> getTickerData(String prodcode, Integer bars);
    
    // for recovery
    Iterable<ChartdataSecond> getChartDataSecond(Date from, Date to);    
    Iterable<ChartdataMinute> getChartDataMinute(Date from, Date to);    
    Iterable<ChartdataHour> getChartDataHour(Date from, Date to);    
    Iterable<ChartdataDaily> getChartDataDaily(Date from, Date to);
                
    // new data set
    void setChartDataSecond(String json_data);     
    void setChartDataMinute(String json_data);    
    void setChartDataHour(String json_data);    
    void setChartDataDaily(String json_data);
    
    // old data set
    void putChartDataLegacy(String json_data, ChartInterval intv);     

    // misc
    void consolidateChartDataTables(String updatetable, LocalDate dt, Long seconds);      
    void doHouseClean(String tablename); 
    void doEvictCache();
    LocalTime getCutOffTime(String prodcode);
}

