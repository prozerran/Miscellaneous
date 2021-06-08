package com.sp.repositories;

import java.math.BigInteger;
import java.util.Date;
import java.util.List;

import org.springframework.data.repository.query.Param;

public interface IChartData {
	
	void purgeDataByDate(@Param("date") Date date);
	
	void optimizeTable();

	void deleteDataByDate(@Param("date") Date date);		
	
	void replaceChartData(@Param("date") Long date, @Param("prod_code") String prod_code, @Param("first_open") double first_open,
			@Param("high") double high, @Param("low") double low, @Param("last_close") double last_close,	
			@Param("prev_close") double prev_close,  @Param("qty") Integer qty, @Param("turnover") BigInteger turnover,
			@Param("instmnt_code") String instmnt_code);	
	
	List<Object[]> getListChartData(@Param("prodcode") String prodcode, @Param("from") Date from, @Param("to") Date to);

	//List<Object[]> getListChartDataObject(@Param("sel_sec") Long sel_sec, @Param("date") Date date, @Param("seconds") Long seconds);	
}
