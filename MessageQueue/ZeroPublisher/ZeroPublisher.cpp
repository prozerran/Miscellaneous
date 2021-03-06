// ZeroPublisher.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"

// C/C++ guide to ZeroMQ
// http://zguide.zeromq.org/page:all
// http://zguide.zeromq.org/cpp:_start
// http://zguide.zeromq.org/cs:_start

//#define USE_BOOST_ASIO
#ifdef USE_BOOST_ASIO

#include <array>
#include <azmq/socket.hpp>
#include <boost/asio.hpp>

namespace asio = boost::asio;

int main(int argc, char** argv)
{
	asio::io_service ios;
	azmq::sub_socket subscriber(ios);
	subscriber.connect("tcp://192.168.55.112:5556");
	subscriber.connect("tcp://192.168.55.201:7721");
	subscriber.set_option(azmq::socket::subscribe("NASDAQ"));

	azmq::pub_socket publisher(ios);
	publisher.bind("ipc://nasdaq-feed");

	std::array<char, 256> buf;

	while (true)
	{
		auto size = subscriber.receive(asio::buffer(buf));
		publisher.send(asio::buffer(buf));
	}
	return 0;
}

#else

#include <iostream>
#include <chrono>
#include <thread>
#include <stdint.h>

#include "zmq.hpp"
#include "zhelpers.hpp"
//#include "date.h"

int main(int argc, char** argv)
{
	zmq::context_t context(1);

	//  Subscriber tells us when it's ready here
	zmq::socket_t sync(context, ZMQ_PULL);
	sync.bind("tcp://*:5564");

	//  We send updates via this socket
	zmq::socket_t publisher(context, ZMQ_PUB);

	/*
	HWM (SNDHWM) on the producer limits how many messages can be queued in memory, 
	not counting any messages in transit over the network, or any messages waiting in the receiver's queue. 
	If you want to limit how many messages are permitted on the receiver's incoming queue,
	you would set RCVHWM on the receiver. In general, it's not a good idea to use HWM for flow control. 
	Hitting HWM should typically be considered an exceptional state to recover from, 
	and not normal behavior.	
	*/
	////  Prevent publisher overflow from slow subscribers
	//uint64_t hwm = 1;
	//publisher.setsockopt(ZMQ_HWM, &hwm, sizeof(hwm));

	////  Specify swap space in bytes, this covers all subscribers
	//uint64_t swap = 25000000;
	//publisher.setsockopt(ZMQ_SWAP, &swap, sizeof(swap));

	publisher.bind("tcp://*:5565");

	//  Wait for synchronization request
	s_recv(sync);

	//  Now broadcast exactly 10 updates with pause
	int update_nbr;
	for (update_nbr = 0; update_nbr < 1000; update_nbr++) 
	{
		std::ostringstream oss;
		oss << "Update " << update_nbr;

		s_sendmore(publisher, "TOPICS1");
		s_send(publisher, oss.str());
		
		std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	}

	s_send(publisher, "END");

	//  Give 0MQ/2.0.x time to flush output
	std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	return 0;

	/*
	// Pure C

	//  Prepare our context and publisher
	void *context = zmq_ctx_new();
	void *publisher = zmq_socket(context, ZMQ_PUB);
	zmq_bind(publisher, "tcp://*:5563");

	while (1) 
	{
		DateTime dt;
		dt.setNow();

		//  Write two messages, each with an envelope and content
		std::string sdt = dt.formatDateTime();

		zmq_send(publisher, sdt.c_str(), sdt.length(), 0);

		std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	}
	//  We never get here, but clean up anyhow
	zmq_close(publisher);
	zmq_ctx_destroy(context);
	return 0;
	*/
}

#endif


