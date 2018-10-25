// ZeroServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "zmq.h"
#include "zmq_utils.h"

#include <string>
#include <iostream>

// CAN ONLY BUILD IND DEBUG

int main()
{
	printf("Server Started at: tcp://localhost:5555\n");

	//  Socket to talk to clients
	void *context = zmq_ctx_new();
	void *responder = zmq_socket(context, ZMQ_REP);
	int rc = zmq_bind(responder, "tcp://*:5555");
	int i = 0;

	while (1) 
	{
		char buffer[10] = { 0 };
		memset(buffer, 0, sizeof(buffer));
		int nrec = zmq_recv(responder, buffer, 10, 0);	// ZMQ_DONTWAIT

		if (nrec > 0)
		{
			printf("Received %d : %s\n", i++, buffer);
			Sleep(1000);
			zmq_send(responder, "world", 5, 0);
		}
	}
	return 0;
}

