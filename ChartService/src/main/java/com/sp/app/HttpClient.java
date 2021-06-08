package com.sp.app;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.Closeable;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
//import javax.net.ssl.HttpsURLConnection;

// https://juffalow.com/java/how-to-send-http-get-post-request-in-java

public class HttpClient {

	/*
    public static void main(String[] args) {
        try {
        	HttpClient hce = new HttpClient();
            String body = hce.get("http://httpbin.org/get");
            String body = hce.post("http://httpbin.org/post", "data=test data");
            System.out.println(body);
        } catch(IOException ioe) {
            ioe.printStackTrace();
        }
    }
    */

    public String get(String getUrl) throws IOException {
        URL url = new URL(getUrl);
        HttpURLConnection con = (HttpURLConnection) url.openConnection();
        con.setRequestMethod("GET");        
        //con.setRequestProperty("Content-Type", "application/json");

        return this.read(con.getInputStream());
    }
    
    public String post(String postUrl, String data) throws IOException {
        URL url = new URL(postUrl);
        HttpURLConnection con = (HttpURLConnection) url.openConnection();
        con.setRequestMethod("POST");
        con.setDoOutput(true);

        this.sendData(con, data);

        return this.read(con.getInputStream());
    }

    protected void sendData(HttpURLConnection con, String data) throws IOException {
        DataOutputStream wr = null;
        try {
            wr = new DataOutputStream(con.getOutputStream());
            wr.writeBytes(data);
            wr.flush();
            wr.close();
        } catch(IOException exception) {
            throw exception;
        } finally {
            this.closeQuietly(wr);
        }
    }    

    private String read(InputStream is) throws IOException {
        BufferedReader in = null;
        String inputLine;
        StringBuilder body;
        try {
            in = new BufferedReader(new InputStreamReader(is));

            body = new StringBuilder();

            while ((inputLine = in.readLine()) != null) {
                body.append(inputLine);
            }
            in.close();

            return body.toString();
        } catch(IOException ioe) {
            throw ioe;
        } finally {
            this.closeQuietly(in);
        }
    }

    protected void closeQuietly(Closeable closeable) {
        try {
            if( closeable != null ) {
                closeable.close();
            }
        } catch(IOException ex) {

        }
    }
}