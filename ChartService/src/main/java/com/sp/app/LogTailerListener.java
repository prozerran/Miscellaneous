package com.sp.app;

import java.io.File;

/**
 * LogTailerListener interface. Implement this interface to create listener object
 */
public interface LogTailerListener {
	
    /**
     * This method when reading starts
     */
    void OnStarted();	
	
    /**
     * This method when reading stopped
     */
    void OnStopped();	
    
    /**
     * This method is called if the tailed file is not found.
     */
    void OnFileNotFound(File file);

    /**
     * This method handles the response from tailer
     */
    void OnUpdate(String line);

    /**
     * This method handles any exception, except IOException (file not found)
     */
    void OnError(Exception exception);

    /**
     * This method is called if tailed file was deleted and tailer is termination
     */
    void OnFileDeleted(File file);
}

