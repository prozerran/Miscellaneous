// ZeroClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "zmq.h"
#include "zmq_utils.h"

#include <string>
#include <iostream>

int main()
{
	const char* hello = "hello\0";
	printf("Connecting to hello world server…\n");
	void *context = zmq_ctx_new();
	void *requester = zmq_socket(context, ZMQ_REQ);
	zmq_connect(requester, "tcp://localhost:5555");

	int request_nbr = 0;

	while (1)
	{
		char buffer[10] = { 0 };
		memset(buffer, 0, sizeof(buffer));
		printf(">> Sending %d : %s\n", request_nbr, hello);
		zmq_send(requester, hello, 5, 0);
		zmq_recv(requester, buffer, 10, 0);
		printf("\t<< Received %d : %s\n", request_nbr, buffer);
		request_nbr++;
	}
	zmq_close(requester);
	zmq_ctx_destroy(context);
	return 0;
}

