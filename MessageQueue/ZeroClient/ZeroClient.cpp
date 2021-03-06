// ZeroClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "zmq.h"

#include <string>
#include <iostream>

using namespace std;

int main()
{
	const char* hello = "hello\0";
	fprintf(stderr, "Connecting to hello world server…\n");
	void *context = zmq_ctx_new();
	void *requester = zmq_socket(context, ZMQ_REQ);
	zmq_connect(requester, "tcp://localhost:5555");

	int request_nbr = 0;
	char buffer[10] = { 0 };

	while (1)
	{
		memset(buffer, 0, sizeof(buffer));
		fprintf(stderr, ">> Sending %d : %s\n", request_nbr, hello);
		zmq_send(requester, hello, 5, 0);
		zmq_recv(requester, buffer, 10, 0);
		fprintf(stderr, "\t<< Received %d : %s\n", request_nbr, buffer);
		request_nbr++;
	}
	zmq_close(requester);
	zmq_ctx_destroy(context);
	return 0;
}

