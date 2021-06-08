
package com.sp.services;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Comparator;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import javax.persistence.EntityManager;

import java.lang.Math;
import java.math.BigInteger;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneId;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Service;

import com.sp.app.AppConstants.ChartInterval;
import com.sp.app.JPAUtility;
import com.sp.app.SPCommon;
import com.sp.app.SPConfig;
import com.sp.entities.ChartdataDaily;
import com.sp.entities.ChartdataHour;
import com.sp.entities.ChartdataMinute;
import com.sp.entities.ChartdataSecond;
import com.sp.entities.ChartdataTicker;
import com.sp.models.ChartdataData;
import com.sp.models.ChartdataLegacy;
import com.sp.models.ChartdataObject;
import com.sp.repositories.ChartdataDailyRepository;
import com.sp.repositories.ChartdataGenericRepository;
import com.sp.repositories.ChartdataHourRepository;
import com.sp.repositories.ChartdataMinuteRepository;
import com.sp.repositories.ChartdataSecondRepository;
import com.sp.repositories.ChartdataTickerRepository;
import com.sp.repositories.IChartData;

import com.alibaba.fastjson.JSON;

/**
 * Product service implement.
 */
@Service
@Component
public class ChartdataServiceImpl implements ChartdataService {
	
    private ChartdataSecondRepository cdsr;
    private ChartdataMinuteRepository cdmr;
    private ChartdataHourRepository cdhr;
    private ChartdataDailyRepository cddr;
    private ChartdataTickerRepository cdtr;
    private ChartdataGenericRepository cdgr;
	
    @Autowired
    public void setChartdataSecondRepository(ChartdataSecondRepository chartdataSecondRepository) {
        this.cdsr = chartdataSecondRepository;
    }    
    
    @Autowired
    public void setChartdataMinuteRepository(ChartdataMinuteRepository chartdataMinuteRepository) {
        this.cdmr = chartdataMinuteRepository;
    } 
    
    @Autowired
    public void setChartdataHourlyRepository(ChartdataHourRepository chartdataHourRepository) {
        this.cdhr = chartdataHourRepository;
    }  
    
    @Autowired
    public void setChartdataDailyRepository(ChartdataDailyRepository chartdataDailyRepository) {
        this.cddr = chartdataDailyRepository;
    }
    
    @Autowired
    public void setChartdataTickerRepository(ChartdataTickerRepository chartdataTickerRepository) {
        this.cdtr = chartdataTickerRepository;
    }  
    
    @Autowired
    public void setChartdataTickerRepository(ChartdataGenericRepository chartdataGenericRepository) {
        this.cdgr = chartdataGenericRepository;
    }  
    
	@Scheduled(fixedRate = 5000)
	@CacheEvict(value = "getChartDataCached", allEntries = true)
	public void cacheEvictionEvent() throws Exception {
		//System.out.println("Cache Evicted!");
	}
	
	@Override
	public void doEvictCache() {
		System.out.println("Evicting all cache!");		
		net.sf.ehcache.CacheManager.getInstance().clearAll();
	} 	
		
	@Override
    public Iterable<ChartdataSecond> getChartDataSecond(Date from, Date to) {	
		return cdsr.getListChartDataSecond(from, to);
	}
	
	@Override
    public Iterable<ChartdataMinute> getChartDataMinute(Date from, Date to) {	
		return cdmr.getListChartDataMinute(from, to);
	}
	
	@Override
    public Iterable<ChartdataHour> getChartDataHour(Date from, Date to) {	
		return cdhr.getListChartDataHour(from, to);
	}
	
	@Override
    public Iterable<ChartdataDaily> getChartDataDaily(Date from, Date to) {	
		return cddr.getListChartDataDaily(from, to);
	}	

	@Override
	public void setChartDataSecond(String json_data) {
		List<ChartdataSecond> lcds = JSON.parseArray(json_data, ChartdataSecond.class);		
		for (ChartdataSecond cds : lcds) {					
			cdsr.replaceChartDataWithRecId(cds.getRecId(),
					cds.getMktDatetime().getTime() / 1000,
					cds.getProdCode(), cds.getOpen(), cds.getHigh(), cds.getLow(), cds.getClose(), cds.getPrevClose(),
					cds.getQty(), cds.getTurnover(), cds.getInstmntCode());					
		}		
	}	
	
	@Override
	public void setChartDataMinute(String json_data) {
		List<ChartdataMinute> lcds = JSON.parseArray(json_data, ChartdataMinute.class);		
		for (ChartdataMinute cds : lcds) {					
			cdmr.replaceChartData(cds.getMktDatetime().getTime() / 1000,
					cds.getProdCode(), cds.getOpen(), cds.getHigh(), cds.getLow(), cds.getClose(), cds.getPrevClose(),
					cds.getQty(), cds.getTurnover(), cds.getInstmntCode());
		}		
	}	
	
	@Override
	public void setChartDataHour(String json_data) {
		List<ChartdataHour> lcds = JSON.parseArray(json_data, ChartdataHour.class);		
		for (ChartdataHour cds : lcds) {					
			cdhr.replaceChartData(cds.getMktDatetime().getTime() / 1000,
					cds.getProdCode(), cds.getOpen(), cds.getHigh(), cds.getLow(), cds.getClose(), cds.getPrevClose(),
					cds.getQty(), cds.getTurnover(), cds.getInstmntCode());
		}		
	}
	
	@Override
	public void setChartDataDaily(String json_data) {
		List<ChartdataDaily> lcds = JSON.parseArray(json_data, ChartdataDaily.class);		
		for (ChartdataDaily cds : lcds) {					
			cddr.replaceChartData(cds.getMktDatetime().getTime() / 1000,
					cds.getProdCode(), cds.getOpen(), cds.getHigh(), cds.getLow(), cds.getClose(), cds.getPrevClose(),
					cds.getQty(), cds.getTurnover(), cds.getInstmntCode());					
		}		
	}	
	
	@Override
	public void putChartDataLegacy(String json_data, ChartInterval intv) {		
		long rec_id_cnt = 0;
    	IChartData pcd = null;    	
    	
    	switch (intv) {
	    	case MINUTE:	pcd = cdmr;	break;
	    	case HOUR:		pcd = cdhr;	break;
	    	case DAILY:		pcd = cddr;	break;
	    	default:		pcd = null; break;
    	}			
		
		List<ChartdataLegacy> lcdl = JSON.parseArray(json_data, ChartdataLegacy.class);		
		for (ChartdataLegacy cdl : lcdl) {			
			double open = cdl.open.doubleValue() / Math.pow(10,  cdl.dec_in_price);
			double high = cdl.high.doubleValue() / Math.pow(10,  cdl.dec_in_price);
			double low = cdl.low.doubleValue() / Math.pow(10,  cdl.dec_in_price);
			double close = cdl.close.doubleValue() / Math.pow(10,  cdl.dec_in_price);
			double prev_close = cdl.prev_close.doubleValue() / Math.pow(10,  cdl.dec_in_price);
			
			if (intv == ChartInterval.SECOND) {				
				String rec_id_s = String.format("%d%d", cdl.log_time, rec_id_cnt++);
				BigInteger rec_id = new BigInteger(rec_id_s);
				
				//String str = String.format("%s, %d, %d, %d, %d, %s", rec_id_s, open, high, low, close, cdl.prod_code);
				//System.out.println(str);
				
				cdsr.replaceChartDataWithRecId(rec_id, cdl.log_time,
						cdl.prod_code, open, high, low, close, prev_close,
						cdl.qty, cdl.turnover, cdl.instmnt_code);
			} else {				
				pcd.replaceChartData(cdl.log_time,
						cdl.prod_code, open, high, low, close, prev_close,
						cdl.qty, cdl.turnover, cdl.instmnt_code);								
			}						
		}
	}			
	
	@Override
    public void doHouseClean(String tablename) {
    	
    	IChartData pcd = null;    	
    	
    	switch ( tablename ) {
    		case "tb.chartdata_ticker":	pcd = cdtr;	break;    	
	    	case "tb.chartdata_second":	pcd = cdsr;	break;
	    	case "tb.chartdata_minute":	pcd = cdmr;	break;
	    	case "tb.chartdata_hour":	pcd = cdhr;	break;
	    	case "tb.chartdata_daily":	pcd = cddr;	break;
	    	default:					pcd = null; break;
    	}    	
    	
	    String days = SPConfig.getInstance().GetProperty(tablename); 	           
        Date date = Date.from(LocalDate.now().atStartOfDay()
        		.minusDays(Long.parseLong(days))
        		.atZone(ZoneId.systemDefault())
        		.toInstant());
	    	    
        pcd.purgeDataByDate(date);
        pcd.optimizeTable();	
        
        SPCommon.ConsoleLog("%s purged before: %s", tablename, date);   
    }
    
	@Override
    public void consolidateChartDataTables(String updatetable, LocalDate dt, Long seconds) {
    	
    	IChartData pcd = null;    	
    	Date date = Timestamp.valueOf(dt.atStartOfDay());     	
    	
    	switch (updatetable ) {
	    	case "ChartdataMinute":	pcd = cdmr;	break;
	    	case "ChartdataHour":	pcd = cdhr;	break;
	    	case "ChartdataDaily":	pcd = cddr;	break;
	    	default:				pcd = null; break;
    	}
   
    	// delete target rows first    	
    	pcd.deleteDataByDate(date);
    	
    	Long sel_sec = seconds;
    	if (seconds == 86400L)
    		sel_sec = 1L;    	
    	
    	// get records in ChartDataSecond table
    	List<Object[]> lcds = cdsr.getListChartDataObject(sel_sec, date, seconds);		
		List<ChartdataObject> lcdo = new ArrayList<ChartdataObject>();    	    
		Iterator<Object[]> it = lcds.iterator();
		
		// massage data
		while(it.hasNext()) {
		     Object[] row = (Object[]) it.next();	
		     ChartdataObject cdo = new ChartdataObject();
		     
		     Integer log_time = (Integer) row[0];
		     cdo.mkt_datetime = log_time.longValue();
		     cdo.prod_code = (String) row[1];
		     cdo.open = (double) row[2];
		     cdo.high = (double) row[3];
		     cdo.low = (double) row[4];
		     cdo.close = (double) row[5];
		     cdo.prev_close = (double) row[6];		     
		     cdo.qty = (long) row[7];		     
		     cdo.log_no_max = (BigInteger) row[8];
		     cdo.log_no_min = (BigInteger) row[9];
		     cdo.turnover = (BigInteger) row[10];
		     cdo.instmnt_code = (String) row[11];

		     // override open + close
		     cdo.last_close = cdsr.getChartDataClose(cdo.log_no_max);
		     cdo.first_open = cdsr.getChartDataOpen(cdo.log_no_min);	
		     
		     lcdo.add(cdo);
		}  	
		
		// replace into DB
		for (ChartdataObject cdo : lcdo) {				
			pcd.replaceChartData(cdo.mkt_datetime, cdo.prod_code, cdo.first_open, cdo.high, cdo.low, 
					cdo.last_close, cdo.prev_close, cdo.qty.intValue(), cdo.turnover, cdo.instmnt_code);		
		}		
        SPCommon.ConsoleLog("%s consolidated for: %s", updatetable, date);
    }    
	
	@Override
	public Iterable<ChartdataTicker> getTickerData(String prodcode, Integer bars) {			
		String sql = "SELECT c FROM ChartdataTicker c WHERE c.prod_code = :prodcode ORDER BY c.mkt_datetime DESC";		
		final EntityManager em = JPAUtility.getEntityManager();
		
		List<ChartdataTicker> lcdt = em.createQuery(sql, ChartdataTicker.class)
				.setParameter("prodcode", prodcode)	
				.setMaxResults(bars)
				.getResultList();
		
		//em.close();		
		return lcdt;			
	}	
	
	@Override
	@Cacheable("getCutOffTime")			
	@SuppressWarnings("deprecation")
	public LocalTime getCutOffTime(String prodcode) {		
		LocalTime cutoff_time = null;	
		String instr_code = cdsr.getInstrumentCode(prodcode);
		
		if (instr_code == null || instr_code.isEmpty())
			return cutoff_time;
		
		String mkt_code = cdgr.getMarketCode(instr_code);				
		List<Object[]> lirc = cdgr.getInstrumentRegionCutoff(instr_code);		
		
		if (lirc.size() > 0) {
			Iterator<Object[]> it = lirc.iterator();	
			
			if (it.hasNext()) {
			    Object[] row = (Object[]) it.next();	
			    
			 	Timestamp w = (Timestamp) row[0]; 
				Timestamp s = (Timestamp) row[1]; 	
			    String region_name = (String) row[2];			   			    
				LocalTime win_time = LocalTime.of(w.getHours(), w.getMinutes(), w.getSeconds());
				LocalTime sum_time = LocalTime.of(s.getHours(), s.getMinutes(), s.getSeconds());							
				List<Object[]> lrd = cdgr.getRegionDate(region_name);
				
				if (lrd.size() > 0)
					cutoff_time = sum_time;
				else
					cutoff_time = win_time;
			}			
		}
		
		if (cutoff_time == null) {
			List<Object[]> lmrc = cdgr.getMarketRegionCutoff(mkt_code);		
			
			if (lmrc.size() > 0) {
				Iterator<Object[]> it = lmrc.iterator();	
				
				if (it.hasNext()) {
				    Object[] row = (Object[]) it.next();	
				    
				 	Timestamp w = (Timestamp) row[0]; 
					Timestamp s = (Timestamp) row[1]; 	
				    String region_name = (String) row[2];				    				    				  				   
					LocalTime win_time = LocalTime.of(w.getHours(), w.getMinutes(), w.getSeconds());
					LocalTime sum_time = LocalTime.of(s.getHours(), s.getMinutes(), s.getSeconds());
					
					if (region_name == null || region_name.isEmpty()) 
						cutoff_time = win_time;
					else {
						List<Object[]> lrd = cdgr.getRegionDate(region_name);
						
						if (lrd.size() > 0)
							cutoff_time = sum_time;
						else
							cutoff_time = win_time;						
					}				
				}			
			}			
		}
		return cutoff_time;
	}  	
        	
	//@Cacheable("GetOHLC")		
	private ChartdataSecond GetOHLC(List<ChartdataSecond> lcds) {
		ChartdataSecond cds = new ChartdataSecond();	
		cds.setMktDatetime(lcds.get(0).getMktDatetime());
		cds.setOpen(lcds.get(0).getOpen());
		cds.setClose(lcds.get(lcds.size()-1).getClose());
		cds.setPrevClose(lcds.get(lcds.size()-1).getPrevClose());
		
		double high = lcds.get(0).getHigh();
		double low = lcds.get(0).getLow();	
		
	  	BigInteger turnover = new BigInteger("0");
		Integer qty = 0;
		
		for (ChartdataSecond item : lcds) {
			if (item.getHigh() > high) // >
				high = item.getHigh();
			
			if (item.getLow() < low) // <
				low = item.getLow();		
			
			turnover = turnover.add(item.getTurnover());
			qty += item.getQty();
			
			// todo: cutoff			
		}
		
		cds.setHigh(high);
		cds.setLow(low);
		cds.setQty(qty);
		cds.setTurnover(turnover);
		cds.setCutOff(0);	// TODO: need fix once we have cutoff
		
		return cds;
	}
	
	//@Cacheable("CDStoCDD")		
	private ChartdataData CDStoCDD(ChartdataSecond cds) {
		ChartdataData cdd = new ChartdataData();
		cdd.O = cds.getOpen();
		cdd.H = cds.getHigh();
		cdd.L = cds.getLow();
		cdd.C = cds.getClose();
		cdd.V = cds.getQty();
		cdd.TO = cds.getTurnover();
		cdd.CO = cds.getCutOff();				
		
		long t = cds.getMktDatetime().getTime() / 1000;
		cdd.T = t;		
		return cdd;
	}
	
	@Cacheable("getChartDataMap")		
	private Map<Integer, List<ChartdataSecond>> getChartDataMap(List<ChartdataSecond> lcds, Integer second) {
		// map items into [timestamp][list]
		Map<Integer, List<ChartdataSecond>> mkeylist = new HashMap<Integer, List<ChartdataSecond>>(); 
						
		lcds.forEach(item->{						
			Date dt = item.getMktDatetime();
			Integer epochtime = (int) (dt.getTime() / 1000);	
			Integer key = ((int) Math.floor(epochtime / second)) * second; 				

			if (!mkeylist.containsKey(key))
				mkeylist.put(key, new ArrayList<>());

			mkeylist.get(key).add(item);				
			//System.out.println("key: " + key + " value: " + dt.toString());
		});
		return mkeylist;
	}		
	
	@Cacheable("getChartDataMapDaily")		
	private Map<Integer, List<ChartdataSecond>> getChartDataMap(List<ChartdataSecond> lcds, LocalTime lt, Integer second) {
		// map items into [timestamp][list]
		Map<Integer, List<ChartdataSecond>> mkeylist = new HashMap<Integer, List<ChartdataSecond>>(); 		
		Integer prev_time = 0;
						
		lcds.forEach(item->{	
			Date dt = item.getMktDatetime();
			Integer epochtime = (int) (dt.getTime() / 1000);	
			
			// TODO: May contain bug, probably rewrite code as logic is based on PHP code
			LocalDateTime ldt = dt.toInstant().atZone(ZoneId.systemDefault()).toLocalDateTime();					
			Calendar cal = Calendar.getInstance();			
			cal.set(ldt.getYear(), ldt.getMonthValue(), ldt.getDayOfMonth(), lt.getHour(), lt.getMinute(), lt.getSecond());
			Integer cutofftime = (int) (cal.getTimeInMillis() / 1000);	
			
			// set cut off?
			if (prev_time < cutofftime && epochtime >= cutofftime)
				item.setCutOff(1);
			else
				item.setCutOff(0);
			
			// why?
			//prev_time = epochtime;
			
			if (epochtime < cutofftime) {
				cutofftime -= 86400;
			}
			Integer key = cutofftime;	// may be error here?						

			if (!mkeylist.containsKey(key))
				mkeylist.put(key, new ArrayList<>());

			mkeylist.get(key).add(item);				
			//System.out.println("key: " + key + " value: " + dt.toString());
		});
		return mkeylist;
	}	
		
	//@Cacheable("getChartDataMap")		
	private Map<Integer, List<ChartdataSecond>> getChartDataMap(String prodcode, Integer second, Date from, Date to) {
		
		List<ChartdataSecond> lcds = new ArrayList<ChartdataSecond>();		
		List<Object[]> lcdo = cdsr.getListChartData(prodcode, from, to);
		
		for(Object o: lcdo){        
			lcds.add((ChartdataSecond)o);        
		}		
		
		// TODO: may need to move to getChartDataHistory
		// this is daily chart, need to make adjustment for OHLC base on different market trading hours
		if (second >= 86400) {					
			LocalTime lt = this.getCutOffTime(prodcode);				
			return getChartDataMap(lcds, lt, second);
		}		
		return getChartDataMap(lcds, second);
	}
	
	@Cacheable("getChartDataHistory")		
	private List<ChartdataData> getChartDataHistory(String prodcode, Integer second, Date from, Date to) {
		
    	IChartData pcd = null;		
    	    	
		if (second < 3600)			pcd = cdmr;		
		else if (second < 86400)	pcd = cdhr;
		else						pcd = cddr;	    	  

		List<ChartdataSecond> lcds = new ArrayList<ChartdataSecond>();		
		List<Object[]> lcdo = pcd.getListChartData(prodcode, from, to);			
		
		if (pcd instanceof ChartdataMinuteRepository) {
			for(Object o: lcdo)
				lcds.add(new ChartdataSecond((ChartdataMinute) o));  				
		} else if (pcd instanceof ChartdataHourRepository) { 
			for(Object o: lcdo)
				lcds.add(new ChartdataSecond((ChartdataHour) o)); 	
		} else {
			for(Object o: lcdo)
				lcds.add(new ChartdataSecond((ChartdataDaily) o)); 		
		}
		
		Map<Integer, List<ChartdataSecond>> mkeylist = getChartDataMap(lcds, second);
		
		// map items into [time][ChartdataSecond] with OHLC
		return getListChartdataData(mkeylist);
	}
	
	@Cacheable("setCutOffLine")	
	@SuppressWarnings("deprecation")
	private List<ChartdataData> setCutOffLine(String prodcode, List<ChartdataData> lcdj) {
		// indicate cutoff time, fast hack, may need re-factoring
		LocalTime lt = this.getCutOffTime(prodcode);	
		boolean cutoff_first_found = false;
		
		if (lt != null) {
			for(ChartdataData cdd: lcdj) {
				Date date = SPCommon.GetDateTime(cdd.T);
				
				if (date.getHours() == lt.getHour()) {				
					if (cutoff_first_found == true)
						continue;
					
					cutoff_first_found = true;				
					cdd.CO = 1;				
				}
				else
					cutoff_first_found = false;								
				//SPCommon.ConsoleLog("%s : %s : %s : %s", date, cdd.O, cdd.C, cdd.CO);			
			}
		}		
		return lcdj;
	}
	
	//@Cacheable("sortChartDataData")	
	private List<ChartdataData> sortChartDataData(List<ChartdataData> lcdj) {
		// sort by time
		Collections.sort(lcdj, new Comparator<ChartdataData>(){
			@Override
			public int compare(ChartdataData o1, ChartdataData o2) {
				Date d1 = new Date(o1.T * 1000);
				Date d2 = new Date(o2.T * 1000);
				return d1.compareTo(d2);
			}
		});	
		return lcdj;
	}
	
	@Cacheable("getListChartdataData")		
	private List<ChartdataData> getListChartdataData(Map<Integer, List<ChartdataSecond>> mkeylist) {
		// map items into [time][ChartdataSecond] with OHLC
		List<ChartdataData> lcdj = new ArrayList<>();
		
		mkeylist.forEach((k, v)->{				
			ChartdataSecond cds = GetOHLC(v);			
			ChartdataData cdj = CDStoCDD(cds);			
			lcdj.add(cdj);			
			//System.out.println("key: " + k + " value: " + v.get(0).getMktDatetime().toString() + cdj.T.toString());	
		});				
		return lcdj;
	}
	
	@Override
	@Cacheable("getChartDataCached")		
	public Iterable<ChartdataData> getChartDataCached(String prodcode, Integer second, Date from, Date to) {		
		Map<Integer, List<ChartdataSecond>> mkeylist = new HashMap<Integer, List<ChartdataSecond>>(); 
		
		// get today data (fill the gap)
		Date today = SPCommon.GetDateTime(SPCommon.GetDayStartDate(0)); 						
		mkeylist = getChartDataMap(prodcode, second, today, to);
			
		// map items into [time][ChartdataSecond] with OHLC		
		List<ChartdataData> lcdj = getListChartdataData(mkeylist);
			
		// get from history tables if needed
		List<ChartdataData> lcdh = getChartDataHistory(prodcode, second, from, to);
		
		// append history data to today's data
		lcdj.addAll(lcdh);			
		
		// sort by time
		lcdj = sortChartDataData(lcdj);
		
		// set daily cutoff time (line in chart)
		lcdj = setCutOffLine(prodcode, lcdj);
											
		return lcdj;
	}	
	
	@Override
	public Iterable<ChartdataData> getChartDataCurrent(String prodcode, Integer second, Date from, Date to) {		
		Map<Integer, List<ChartdataSecond>> mkeylist = new HashMap<Integer, List<ChartdataSecond>>(); 		
		mkeylist = getChartDataMap(prodcode, second, from, to);	
			
		// map items into [time][ChartdataSecond] with OHLC		
		List<ChartdataData> lcdj = getListChartdataData(mkeylist);			
		
		// sort by time
		lcdj = sortChartDataData(lcdj);
		
		// set daily cutoff time (line in chart)
		lcdj = setCutOffLine(prodcode, lcdj);
											
		return lcdj;
	}	
}









