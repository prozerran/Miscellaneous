package com.sp.app;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;

// https://www.baeldung.com/hibernate-entitymanager
// https://www.concretepage.com/java/jpa/jpa-entitymanager-and-entitymanagerfactory-example-using-hibernate-with-persist-find-contains-detach-merge-and-remove
// https://stackoverflow.com/questions/5350994/jpql-select-between-date-statement

public class JPAUtility {
	
 	private static final EntityManagerFactory emf;
 	
	static {
		emf = Persistence.createEntityManagerFactory("ChartService");
	}
	
	public static EntityManagerFactory getEntityManagerFactoryInstance() {
		return emf;
	}

	public static EntityManager getEntityManager() {
		return emf.createEntityManager();
	}	
	
	public static void close() {
		emf.close();
	}
} 
