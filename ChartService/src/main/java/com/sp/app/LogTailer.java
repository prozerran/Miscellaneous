package com.sp.app;

import java.io.*;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.StandardOpenOption;

import org.apache.commons.io.input.ReversedLinesFileReader;

/**
 * 1. Create LogTailer object using one of the constructors:
 * LogTailer tailer = new LogTailer(File file)
 * LogTailer tailer = new LogTailer()
 * 2. If you created LogTailer with "default" constructor, don't forget to set tailing file using setFile setter:
 * tailer.setFile(File file)
 * 3. Add one or more listeners (implementations of LogTailerListener):
 * tailer.addListener(LogTailerListener listener) as an inner class or a separate class.
 * 4. Run it as a new Thread:
 * new Thread(tailer).start()  Mark thread as a daemon thread, if you need.
 */
public class LogTailer implements Runnable { 
    private LogTailerListener ltnotify = null;
    private File file;
    //private long startTime;
    //private long timeoutUntilInterrupt;
    private int sleepTimer;
    private boolean isStopped;
    private boolean isChanged;
    private ReadOrder rdorder = ReadOrder.FORWARD;
        
    public enum ReadOrder {
        FORWARD,	// start read from beginning of file, then continue reading as file gets appended        
        REVERSE,	// start read from EOF, then back up until start of file
        START_EOF,  // start read from end of file going down, then continue reading as file gets appended
        UNTIL_EOF	// start read from beginning of file, until EOF 
	}    

    public LogTailer() {
        //this.timeoutUntilInterrupt = 0;
        this.sleepTimer = 1000;
    }

    /**
     * Constructor method
     * timeoutUntilInterrupt - number of Hours before tailer thread will be terminated. 0 - unlimited. Default is 0.
     * sleepTimer - delay between each trying to read the completed log file. Measures in Milliseconds. Default is 1000 ms.
     *
     * @param file - File class with listened file.
     */
    public LogTailer(File file) {
        //this.timeoutUntilInterrupt = 0;
        this.sleepTimer = 1000;
        this.file = file;
    }

    public void setFile(File file) {
        this.file = file;
    }
    
    public void setReadOrder(ReadOrder order) {
    	this.rdorder = order;
    }

    public void setTimeoutUntilInterrupt(long timeoutUntilInterrupt) {
        //this.timeoutUntilInterrupt = timeoutUntilInterrupt * 3600000;
    }

    public void setSleepTimer(int sleepTimer) {
        this.sleepTimer = sleepTimer;
    }

    public void setStopped(boolean stopped) {
        isStopped = stopped;
    }

    public void RegisterCallback(LogTailerListener listener) {
    	ltnotify = listener;
    }

    public void UnRegisterCallback() {
    	ltnotify = null;
    }
    
    private void ForwardReader() {
    	
        //startTime = System.currentTimeMillis();
        isStopped = false;
        isChanged = false;
        String line;
        InputStream is = null;
    	ltnotify.OnStarted();
    	
        try {
        	
            is = Files.newInputStream(file.toPath(), StandardOpenOption.READ);
            InputStreamReader reader = new InputStreamReader(is, "UTF-8");
            BufferedReader lineReader = new BufferedReader(reader);
            
            // if tailer is not stopped, process all files
            if (!isStopped && file.exists()) {
            	lineReader.readLine();		// skip header info
            }
            	                      
            while (!isStopped && file.exists()) {                            	          
            	line = lineReader.readLine();          	                                                             
                if (line != null) {
                	isChanged = true;
                } else {
                	Thread.sleep(sleepTimer);
                	/*
                    try {
                        TimeUnit.MILLISECONDS.sleep(sleepTimer);
                        // check if life timer exceeded available live of object
                        if (timeoutUntilInterrupt != 0 && (System.currentTimeMillis() - startTime > timeoutUntilInterrupt)) {
                            setStopped(true);
                        }
                    } catch (InterruptedException e) {
                    	ltnotify.OnError(e);
                    }
                    */
                }
                
                // update subscriber
                if (isChanged) {
                	ltnotify.OnUpdate(line);
              	}
            	isChanged = false;
            }
        } catch (NullPointerException e) {
            ltnotify.OnFileNotFound(file);
        } catch (IOException e) {
        	ltnotify.OnFileNotFound(file);
        } catch (RuntimeException e) {
        	ltnotify.OnError(e);
        } catch (Exception e) {
        	ltnotify.OnError(e);
        } finally {
            try {
                if (is != null) {
                    is.close();
                }
                ltnotify.OnStopped();                
            } catch (IOException e) {
            	ltnotify.OnError(e);
            }
        }    	
    }
    
    // https://www.programcreek.com/java-api-examples/index.php?api=org.apache.commons.io.input.ReversedLinesFileReader
    private void ReverseReader() {    
    	
    	ReversedLinesFileReader is = null;
    	ltnotify.OnStarted();
    	
        try {
            if (file == null || file.getPath().equals(""))
                ltnotify.OnFileNotFound(file);

            String line = "";
            is = new ReversedLinesFileReader(file, Charset.defaultCharset());                       

            while ((line = is.readLine()) != null) {            	
            	ltnotify.OnUpdate(line);            
            	/*
                if (Pattern.matches("*.header.*", line))	// possible way to skip line
                    continue;
                */
            }
        } catch (Exception e) {
        	ltnotify.OnError(e);
        } finally {
            try {
                if (is != null) {
                    is.close();
                }
                ltnotify.OnStopped();                
            } catch (IOException e) {
            	ltnotify.OnError(e);
            }
        }        
    }  
    
    private void StartEOFReader() {  
    	
        //startTime = System.currentTimeMillis();
        isStopped = false;
        isChanged = false;
        String line;  	    	
    	String filename = file.getAbsolutePath();
    	RandomAccessFile is = null;
    	ltnotify.OnStarted();  
    	
		try {			
			is = new RandomAccessFile(filename, "r");			
			is.seek(is.length());	// SEEK_END, move pointer to EOF
			              
            while (!isStopped && file.exists()) {
                line = is.readLine();
                if (line != null) {
                	isChanged = true;
                } else {
                	Thread.sleep(sleepTimer);                	
                	/*
                    try {
                        TimeUnit.MILLISECONDS.sleep(sleepTimer);
                        // check if life timer exceeded available live of object
                        if (timeoutUntilInterrupt != 0 && (System.currentTimeMillis() - startTime > timeoutUntilInterrupt)) {
                            setStopped(true);
                        }
                    } catch (InterruptedException e) {
                    	ltnotify.OnError(e);
                    }
                    */
                }
                
                // update subscriber
                if (isChanged) {
                	ltnotify.OnUpdate(line);
              	}
            	isChanged = false;
            }			
        } catch (NullPointerException e) {
            ltnotify.OnFileNotFound(file);
        } catch (IOException e) {
        	ltnotify.OnFileNotFound(file);
        } catch (RuntimeException e) {
        	ltnotify.OnFileDeleted(file);
        } catch (Exception e) {
        	ltnotify.OnError(e);
        } finally {
            try {
                if (is != null) {
                    is.close();
                }
                ltnotify.OnStopped();                
            } catch (IOException e) {
            	ltnotify.OnError(e);
            }
        }    	
    }   
    
    private void UntilEOFReader() {
        isStopped = false;
        isChanged = false;
        String line;
        InputStream is = null;
    	ltnotify.OnStarted();
    	
        try {
        	
            is = Files.newInputStream(file.toPath(), StandardOpenOption.READ);
            InputStreamReader reader = new InputStreamReader(is, "UTF-8");
            BufferedReader lineReader = new BufferedReader(reader);
            
            // if tailer is not stopped, process all files
            if (!isStopped && file.exists()) {
            	lineReader.readLine();		// skip header info
            }
            	                      
            while ((line = lineReader.readLine()) != null) {            	
            	ltnotify.OnUpdate(line);            
            }            
            
        } catch (NullPointerException e) {
            ltnotify.OnFileNotFound(file);
        } catch (IOException e) {
        	ltnotify.OnFileNotFound(file);
        } catch (RuntimeException e) {
        	ltnotify.OnError(e);
        } catch (Exception e) {
        	ltnotify.OnError(e);
        } finally {
            try {
                if (is != null) {
                    is.close();
                }
                ltnotify.OnStopped();                
            } catch (IOException e) {
            	ltnotify.OnError(e);
            }
        }    	
    }    

    public void run() {    	
    	switch (rdorder) {    	
	    	case FORWARD:
	        	ForwardReader();
	    		break;
	    		
	    	case REVERSE:
	    		ReverseReader();
	    		break;
	    		
	    	case START_EOF:
	    		StartEOFReader();
	    		break;
	    		
	    	case UNTIL_EOF:
	    		UntilEOFReader();
	    		break;
	    	
			default:
				break;    	
    	}    
    }  
}

