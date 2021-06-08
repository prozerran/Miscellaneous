
package com.sp.repositories;

import java.util.List;

import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Repository;

import com.sp.entities.ChartdataDaily;

@Repository
@Component
public interface ChartdataGenericRepository extends CrudRepository<ChartdataDaily, Integer> {
				
	@Cacheable("getMarketCode")	
	@Query( value = "SELECT market_code FROM instrument WHERE instmnt_code = :code", nativeQuery = true )	
	String getMarketCode(@Param("code") String code);
	
	@Cacheable("getInstrumentRegionCutoff")	
	@Query( value = "SELECT winter_cutoff_time, summer_cutoff_time, region_name FROM chartdata_instrument WHERE instmnt_code = :code", nativeQuery = true )	
	List<Object[]> getInstrumentRegionCutoff(@Param("code") String code);	
	
	@Cacheable("getMarketRegionCutoff")	
	@Query( value = "SELECT winter_cutoff_time, summer_cutoff_time, region_name FROM chartdata_market WHERE market_code = :code", nativeQuery = true )	
	List<Object[]> getMarketRegionCutoff(@Param("code") String code);		
		
	@Cacheable("getRegionDate")	
	@Query( value = "SELECT winter_date, summer_date FROM chartdata_region WHERE region_name = :name AND winter_date > CURDATE() and summer_date < CURDATE()", nativeQuery = true )	
	List<Object[]> getRegionDate(@Param("name") String name);		
}



