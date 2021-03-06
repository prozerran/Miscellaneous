// ZeroServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "zmq.h"

#include <string>
#include <iostream>

using namespace std;

int main()
{
	fprintf(stderr, "Server Started at: tcp://localhost:5555\n");

	//  Socket to talk to clients
	void *context = zmq_ctx_new();
	void *responder = zmq_socket(context, ZMQ_REP);
	int rc = zmq_bind(responder, "tcp://*:5555");
	int i = 0;
	char buffer[10] = { 0 };

	while (1) 
	{
		memset(buffer, 0, sizeof(buffer));
		int nrec = zmq_recv(responder, buffer, 10, 0);	// ZMQ_DONTWAIT

		if (nrec > 0)
		{
			fprintf(stderr, "Received %d : %s\n", i++, buffer);
			Sleep(100);
			zmq_send(responder, "world", 5, 0);
		}
	}
	return 0;
}

