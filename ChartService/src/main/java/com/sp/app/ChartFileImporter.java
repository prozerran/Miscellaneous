package com.sp.app;

public class ChartFileImporter extends BaseImporter {
	
    private static ChartFileImporter _instance = null; 	
	  
    private ChartFileImporter() {} 
  
    public static ChartFileImporter getInstance() { 
        if (_instance == null) 
        	_instance = new ChartFileImporter(); 
  
        return _instance; 
    }	
    
	public String GetFileName() {			
		return new String("chartdata");
	}    

	public void OnUpdate(String line) {		
		
		try {
			
			String[] col = line.split(",");		
			
			long rec_id = Long.parseUnsignedLong(col[0]); 			
			int date = Integer.parseInt(col[1]);
			int time = Integer.parseInt(col[2]);
			String prodcode = col[3];
			
			double open = Double.parseDouble(col[4]);
			double high = Double.parseDouble(col[5]);			
			double low = Double.parseDouble(col[6]);
			double close = Double.parseDouble(col[7]);
			double prev_close = Double.parseDouble(col[8]);
			
			int qty = Integer.parseInt(col[9]);
			long turnover = Long.parseLong(col[10]);
			String instrcode = col[11];
			
			dba.InsertChartSecond(rec_id, date, time, prodcode, open, high, low, close, prev_close, qty, turnover, instrcode);						
		}	
		catch (Exception ex) {
			SPCommon.ConsoleLog("Error: %s", ex.getMessage());	
		}		
	}			
}

