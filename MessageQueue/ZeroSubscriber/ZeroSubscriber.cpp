// ZeroPublisher.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"

#include <iostream>
#include <chrono>
#include <thread>

#include "zmq.hpp"
#include "..\ZeroPublisher\zhelpers.hpp"

int main(int argc, char** argv)
{
	zmq::context_t context(1);

	//  Connect our subscriber socket
	zmq::socket_t subscriber(context, ZMQ_SUB);
	subscriber.setsockopt(ZMQ_IDENTITY, "Hello", 5);
	subscriber.setsockopt(ZMQ_SUBSCRIBE, "TOPICS1", 7);
	subscriber.connect("tcp://localhost:5565");

	//  Synchronize with publisher
	zmq::socket_t sync(context, ZMQ_PUSH);
	sync.connect("tcp://localhost:5564");
	s_send(sync, "");

	//  Get updates, expect random Ctrl-C death
	while (1) 
	{
		//  Read envelope with address
		std::string topic_name = s_recv(subscriber);
		//  Read message contents
		std::string contents = s_recv(subscriber);

		std::cout << "[" << topic_name << "] " << contents << std::endl;

		if (contents.compare("END") == 0)
			break;
	}
	return 0;
}