
package com.sp.repositories;

import java.math.BigInteger;
import java.util.Date;
import java.util.List;

import javax.transaction.Transactional;

import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Repository;

import com.sp.entities.ChartdataMinute;

@Repository
@Component
public interface ChartdataMinuteRepository extends IChartData, CrudRepository<ChartdataMinute, Integer> {
	
	@Cacheable("getListChartDataMinute")
	@Query( "SELECT c FROM ChartdataMinute c WHERE c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<ChartdataMinute> getListChartDataMinute(@Param("from") Date from, @Param("to") Date to);	
	
	@Transactional
	@Modifying
	@Override	
	@Query( "DELETE FROM ChartdataMinute WHERE mkt_datetime < :date" )		
	void purgeDataByDate(@Param("date") Date date);
	
	@Transactional
	@Modifying
	@Override	
	@Query( value = "OPTIMIZE TABLE chartdata_minute", nativeQuery = true )			
	void optimizeTable();	
	
	@Transactional
	@Modifying
	@Override
	@Query( "DELETE FROM ChartdataMinute WHERE DATE(mkt_datetime) = :date" )	
	void deleteDataByDate(@Param("date") Date date);		
	
	@Transactional
	@Modifying	
	@Override	
	@Query( value = "REPLACE INTO chartdata_minute (mkt_datetime, prod_code, open, high, low, close, prev_close, qty, turnover, instmnt_code)"
	+ " VALUES(FROM_UNIXTIME(:date), :prod_code, :first_open, :high, :low, :last_close, :prev_close, :qty, :turnover, :instmnt_code)",
	nativeQuery = true)
	void replaceChartData(@Param("date") Long date, @Param("prod_code") String prod_code, @Param("first_open") double first_open,
			@Param("high") double high, @Param("low") double low, @Param("last_close") double last_close,	
			@Param("prev_close") double prev_close,  @Param("qty") Integer qty, @Param("turnover") BigInteger turnover,
			@Param("instmnt_code") String instmnt_code);
	
	@Override	
	@Cacheable("getListChartDataMinute")
	@Query( "SELECT c FROM ChartdataMinute c WHERE c.prod_code = :prodcode AND c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<Object[]> getListChartData(@Param("prodcode") String prodcode, @Param("from") Date from, @Param("to") Date to);
}

