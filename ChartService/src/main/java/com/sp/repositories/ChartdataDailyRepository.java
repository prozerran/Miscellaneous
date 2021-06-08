
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

import com.sp.entities.ChartdataDaily;

@Repository
@Component
public interface ChartdataDailyRepository extends IChartData, CrudRepository<ChartdataDaily, Integer> {
	
	@Cacheable("getListChartDataDaily")
	@Query( "SELECT c FROM ChartdataDaily c WHERE c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<ChartdataDaily> getListChartDataDaily(@Param("from") Date from, @Param("to") Date to);	
	
	@Transactional
	@Modifying
	@Override	
	@Query( "DELETE FROM ChartdataDaily WHERE mkt_datetime < :date" )		
	void purgeDataByDate(@Param("date") Date date);
	
	@Transactional
	@Modifying
	@Override	
	@Query( value = "OPTIMIZE TABLE chartdata_daily", nativeQuery = true )			
	void optimizeTable();		
	
	@Transactional
	@Modifying	
	@Override	
	@Query( "DELETE FROM ChartdataDaily WHERE DATE(mkt_datetime) = :date" )	
	void deleteDataByDate(@Param("date") Date date);		
	
	@Transactional
	@Modifying	
	@Override		
	@Query( value = "REPLACE INTO chartdata_daily (mkt_datetime, prod_code, open, high, low, close, prev_close, qty, turnover, instmnt_code)"
	+ " VALUES(FROM_UNIXTIME(:date), :prod_code, :first_open, :high, :low, :last_close, :prev_close, :qty, :turnover, :instmnt_code)",
	nativeQuery = true)
	void replaceChartData(@Param("date") Long date, @Param("prod_code") String prod_code, @Param("first_open") double first_open,
			@Param("high") double high, @Param("low") double low, @Param("last_close") double last_close,	
			@Param("prev_close") double prev_close,  @Param("qty") Integer qty, @Param("turnover") BigInteger turnover,
			@Param("instmnt_code") String instmnt_code);
	
	@Override	
	@Cacheable("getListChartDataDaily")
	@Query( "SELECT c FROM ChartdataDaily c WHERE c.prod_code = :prodcode AND c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<Object[]> getListChartData(@Param("prodcode") String prodcode, @Param("from") Date from, @Param("to") Date to);	
}


