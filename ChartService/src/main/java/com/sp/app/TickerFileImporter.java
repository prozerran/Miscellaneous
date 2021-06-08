package com.sp.app;

public class TickerFileImporter extends BaseImporter {
	
    private static TickerFileImporter _instance = null; 	
	  
    private TickerFileImporter() {} 
  
    public static TickerFileImporter getInstance() { 
        if (_instance == null) 
        	_instance = new TickerFileImporter(); 
  
        return _instance; 
    }	
    
	public String GetFileName() {			
		return new String("ticker");
	}    

	public void OnUpdate(String line) {		
			
		try {
			
			String[] col = line.split(",");		
			long rec_id = Long.parseUnsignedLong(col[0]);
			int date = Integer.parseInt(col[1]);
			int time = Integer.parseInt(col[2]);
			String prodcode = col[3];
			double price = Double.parseDouble(col[4]);
			int qty = Integer.parseInt(col[5]);
			int deal_src = Integer.parseInt(col[6]);
			String side = "";
			
			if (col.length > 12)
				side = col[13];
			
			dba.InsertTickerData(rec_id, date, time, prodcode, price, qty, deal_src, side);					
		}	
		catch (Exception ex) {
			SPCommon.ConsoleLog("Error: %s", ex.getMessage());	
		}		
	}			
}

