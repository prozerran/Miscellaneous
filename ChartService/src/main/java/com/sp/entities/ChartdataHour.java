package com.sp.entities;

import java.io.Serializable;
import java.math.BigInteger;
import java.util.Date;

import javax.persistence.*;


/**
 * The persistent class for the chartdata_hour database table.
 * 
 */
@Entity
@Table(name="chartdata_hour")
@NamedQuery(name="ChartdataHour.findAll", query="SELECT c FROM ChartdataHour c")
public class ChartdataHour implements Serializable {
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

	private BigInteger turnover;

	public ChartdataHour() {
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

	public BigInteger getTurnover() {
		return this.turnover;
	}

	public void setTurnover(BigInteger turnover) {
		this.turnover = turnover;
	}	
}