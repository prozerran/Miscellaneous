

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

import com.sp.entities.ChartdataSecond;

// https://stackoverflow.com/questions/44565820/what-is-the-limit-clause-alternative-in-jpql
// https://stackoverflow.com/questions/20679237/jpql-limit-query/36790914

@Repository
@Component
public interface ChartdataSecondRepository extends IChartData, CrudRepository<ChartdataSecond, Integer> {
	
	@Transactional
	@Modifying
	@Override	
	@Query( "DELETE FROM ChartdataSecond WHERE mkt_datetime < :date" )		
	void purgeDataByDate(@Param("date") Date date);
	
	@Transactional
	@Modifying
	@Override	
	@Query( value = "OPTIMIZE TABLE chartdata_second", nativeQuery = true )		
	void optimizeTable();	
	
	@Transactional
	@Modifying
	@Override
	@Query( "DELETE FROM ChartdataSecond WHERE DATE(mkt_datetime) = :date" )	
	void deleteDataByDate(@Param("date") Date date);	
	
	@Transactional
	@Modifying	
	@Query( value = "REPLACE INTO chartdata_second (rec_id, mkt_datetime, prod_code, open, high, low, close, prev_close, qty, turnover, instmnt_code)"
	+ " VALUES(:rec_id, FROM_UNIXTIME(:date), :prod_code, :first_open, :high, :low, :last_close, :prev_close, :qty, :turnover, :instmnt_code)",
	nativeQuery = true)
	void replaceChartDataWithRecId(@Param("rec_id") BigInteger rec_id,
			@Param("date") Long date, @Param("prod_code") String prod_code, @Param("first_open") double first_open,
			@Param("high") double high, @Param("low") double low, @Param("last_close") double last_close,	
			@Param("prev_close") double prev_close,  @Param("qty") Integer qty, @Param("turnover") BigInteger turnover,
			@Param("instmnt_code") String instmnt_code);	
	
	@Transactional
	@Modifying	
	@Override	
	@Query( value = "REPLACE INTO chartdata_second (mkt_datetime, prod_code, open, high, low, close, prev_close, qty, turnover, instmnt_code)"
	+ " VALUES(FROM_UNIXTIME(:date), :prod_code, :first_open, :high, :low, :last_close, :prev_close, :qty, :turnover, :instmnt_code)",
	nativeQuery = true)
	void replaceChartData(@Param("date") Long date, @Param("prod_code") String prod_code, @Param("first_open") double first_open,
			@Param("high") double high, @Param("low") double low, @Param("last_close") double last_close,	
			@Param("prev_close") double prev_close,  @Param("qty") Integer qty, @Param("turnover") BigInteger turnover,
			@Param("instmnt_code") String instmnt_code);		
	
	@Query( "SELECT c FROM ChartdataSecond c WHERE c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<ChartdataSecond> getListChartDataSecond(@Param("from") Date from, @Param("to") Date to);

	@Query( "SELECT open FROM ChartdataSecond WHERE log_no = :log_no" )	
	double getChartDataOpen(@Param("log_no") BigInteger log_no);		
	
	@Query( "SELECT close FROM ChartdataSecond WHERE log_no = :log_no" )	
	double getChartDataClose(@Param("log_no") BigInteger log_no);
	
	@Cacheable("getInstrumentCode")	
	@Query( value = "SELECT instmnt_code FROM chartdata_second WHERE prod_code = :code LIMIT 1", nativeQuery = true )	
	String getInstrumentCode(@Param("code") String code);	
	
	@Override	
	//@Cacheable("getListChartDataSecond")
	@Query( "SELECT c FROM ChartdataSecond c WHERE c.prod_code = :prodcode AND c.mkt_datetime >= :from AND c.mkt_datetime < :to ORDER BY c.mkt_datetime ASC" )	
	List<Object[]> getListChartData(@Param("prodcode") String prodcode, @Param("from") Date from, @Param("to") Date to);	
	
	@Query( "SELECT FLOOR(UNIX_TIMESTAMP(mkt_datetime)/:sel_sec)*:sel_sec AS log_time, prod_code, open," + 
	"		MAX(high) AS high, MIN(low) AS low, close, prev_close, SUM(qty) AS qty," + 
	"		MAX(log_no) AS log_no_max," + 
	"		MIN(log_no) AS log_no_min," + 
	"		SUM(turnover) AS turnover, instmnt_code" + 
	"		FROM ChartdataSecond" + 
	"		WHERE DATE(mkt_datetime) = :date" + 
	"		GROUP BY prod_code, FLOOR(UNIX_TIMESTAMP(mkt_datetime)/:seconds)*:seconds")	
	List<Object[]> getListChartDataObject(@Param("sel_sec") Long sel_sec, @Param("date") Date date, @Param("seconds") Long seconds);			
}

