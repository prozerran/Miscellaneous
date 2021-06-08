package com.sp.entities;

import java.io.Serializable;
import javax.persistence.*;
import java.util.Date;
import java.math.BigInteger;


/**
 * The persistent class for the chartdata_second database table.
 * 
 */
@Entity
@Table(name="chartdata_second")
@NamedQuery(name="ChartdataSecond.findAll", query="SELECT c FROM ChartdataSecond c")
public class ChartdataSecond implements Serializable {
	private static final long serialVersionUID = 1L;

	private double close;

	private double high;

	private String instmnt_code;

	@Id
	private BigInteger log_no;

	private double low;

	@Temporal(TemporalType.TIMESTAMP)
	private Date mkt_datetime;

	private double open;

	private double prev_close;

	private String prod_code;

	private int qty;

	private BigInteger rec_id;

	private BigInteger turnover;
	
	@Transient
	private int cutoff;		
	
	public ChartdataSecond() {
	}
	
	public ChartdataSecond(ChartdataMinute o) {
		this.log_no = o.getLogNo();
		this.mkt_datetime = o.getMktDatetime();
		this.prod_code = o.getProdCode();
		this.open = o.getOpen();
		this.high = o.getHigh();
		this.low = o.getLow();
		this.close = o.getClose();
		this.prev_close = o.getPrevClose();
		this.qty = o.getQty();	
		this.turnover = o.getTurnover();
		this.instmnt_code = o.getInstmntCode();		
	}
	
	public ChartdataSecond(ChartdataHour o) {
		this.log_no = o.getLogNo();
		this.mkt_datetime = o.getMktDatetime();
		this.prod_code = o.getProdCode();
		this.open = o.getOpen();
		this.high = o.getHigh();
		this.low = o.getLow();
		this.close = o.getClose();
		this.prev_close = o.getPrevClose();
		this.qty = o.getQty();	
		this.turnover = o.getTurnover();
		this.instmnt_code = o.getInstmntCode();		
	}	
	
	public ChartdataSecond(ChartdataDaily o) {
		this.log_no = o.getLogNo();
		this.mkt_datetime = o.getMktDatetime();
		this.prod_code = o.getProdCode();
		this.open = o.getOpen();
		this.high = o.getHigh();
		this.low = o.getLow();
		this.close = o.getClose();
		this.prev_close = o.getPrevClose();
		this.qty = o.getQty();	
		this.turnover = o.getTurnover();
		this.instmnt_code = o.getInstmntCode();		
	}	

	public double getClose() {
		return this.close;
	}

	public void setClose(double close) {
		this.close = close;
	}

	public double getHigh() {
		return this.high;
	}

	public void setHigh(double high) {
		this.high = high;
	}

	public String getInstmntCode() {
		return this.instmnt_code;
	}

	public void setInstmntCode(String instmntCode) {
		this.instmnt_code = instmntCode;
	}

	public BigInteger getLogNo() {
		return this.log_no;
	}

	public void setLogNo(BigInteger logNo) {
		this.log_no = logNo;
	}

	public double getLow() {
		return this.low;
	}

	public void setLow(double low) {
		this.low = low;
	}

	public Date getMktDatetime() {
		return this.mkt_datetime;
	}

	public void setMktDatetime(Date mktDatetime) {
		this.mkt_datetime = mktDatetime;
	}

	public double getOpen() {
		return this.open;
	}

	public void setOpen(double open) {
		this.open = open;
	}

	public double getPrevClose() {
		return this.prev_close;
	}

	public void setPrevClose(double prevClose) {
		this.prev_close = prevClose;
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

	public BigInteger getTurnover() {
		return this.turnover;
	}

	public void setTurnover(BigInteger turnover) {
		this.turnover = turnover;
	}
	
	public int getCutOff() {
		return this.cutoff;
	}

	public void setCutOff(int cutoff) {
		this.cutoff = cutoff;
	}	
}