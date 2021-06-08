package com.sp.entities;

import java.io.Serializable;
import javax.persistence.*;

import org.springframework.cache.annotation.Cacheable;

import com.sp.models.ChartdataData;

import java.util.Date;
import java.math.BigInteger;


/**
 * The persistent class for the chartdata_ticker database table.
 * 
 */

// https://www.eclipse.org/webtools/dali/docs/3.2/user_guide/tasks006.htm

@Entity
@Table(name="chartdata_ticker")
@NamedQuery(name="ChartdataTicker.findAll", query="SELECT c FROM ChartdataTicker c")
public class ChartdataTicker implements Serializable {
	private static final long serialVersionUID = 1L;

	private int deal_src;

	@Id
	private BigInteger log_no;

	@Temporal(TemporalType.TIMESTAMP)
	private Date mkt_datetime;

	private double price;

	private String prod_code;

	private int qty;

	private BigInteger rec_id;

	private String side;

	public ChartdataTicker() {
	}

	public int getDealSrc() {
		return this.deal_src;
	}

	public void setDealSrc(int dealSrc) {
		this.deal_src = dealSrc;
	}

	public BigInteger getLogNo() {
		return this.log_no;
	}

	public void setLogNo(BigInteger logNo) {
		this.log_no = logNo;
	}

	public Date getMktDatetime() {
		return this.mkt_datetime;
	}

	public void setMktDatetime(Date mktDatetime) {
		this.mkt_datetime = mktDatetime;
	}

	public double getPrice() {
		return this.price;
	}

	public void setPrice(double price) {
		this.price = price;
	}

	public String getProdCode() {
		return this.prod_code;
	}

	public void setProdCode(String prodCode) {
		this.prod_code = prodCode;
	}

	public int getQty() {
		return this.qty;
	}

	public void setQty(int qty) {
		this.qty = qty;
	}

	public BigInteger getRecId() {
		return this.rec_id;
	}

	public void setRecId(BigInteger recId) {
		this.rec_id = recId;
	}

	public String getSide() {
		return this.side;
	}

	public void setSide(String side) {
		this.side = side;
	}
	
	// data_mode = 2 [ticker]
	//@Cacheable("getTickerTextCSV")		
	public static String getTickerTextCSV(Iterable<ChartdataTicker> cdt) { 		
		StringBuilder data_str = new StringBuilder();   		
		for (ChartdataTicker c: cdt) {						
			long time = c.getMktDatetime().getTime() / 1000;
			int qty = c.getQty();
			
			data_str.append(c.getPrice());
			data_str.append(',');
			data_str.append(qty);
			data_str.append(',');
			data_str.append(time);
			data_str.append("\r\n");   			
		}		
		return data_str.toString();
	}	
}