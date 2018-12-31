// ZMQServer.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "zmq.h"
#include "FBSchema_generated.h"

//#include "flatbuffers/idl.h"
//#include "flatbuffers/util.h"

#include <string>
#include <iostream>

using namespace OrderBook::Book;
using PTR_FBB = flatbuffers::FlatBufferBuilder*;

// 1. Uses ZeroMQ
// 2. Uses FlatBuffers
//		- https://google.github.io/flatbuffers/flatbuffers_guide_use_cpp.html
//		- /drives/c/Users/tim.hsu/Documents/Github/vcpkg/packages/flatbuffers_x86-windows/tools/flatbuffers/flatc.exe -c FBSchema.fbs
// 3. Security
//		- http://curvezmq.org/
//		- https://github.com/zeromq/czmq
//		- https://jaxenter.com/using-zeromq-security-part-2-119353.html

static long g_id = 10070;

PTR_FBB BuildFlatBuffer(const std::string& market)
{
	// Build up a serialized buffer algorithmically:
	static flatbuffers::FlatBufferBuilder fbb;
	fbb.Clear();

	// First, lets serialize some weapons for the Monster: A 'sword' and an 'axe'.
	auto instr1 = fbb.CreateString("PERBTC30");
	auto instr2 = fbb.CreateString("PERBTC60");

	// Use the `CreateWeapon` shortcut to create Weapons with all fields set.
	auto cont1 = CreateContract(fbb, instr1, 899.3);
	auto cont2 = CreateContract(fbb, instr2, 102.1);

	// Create a FlatBuffer's `vector` from the `std::vector`.
	std::vector<flatbuffers::Offset<Contract>> cont_vector;
	cont_vector.push_back(cont1);
	cont_vector.push_back(cont2);
	auto contracts = fbb.CreateVector(cont_vector);

	// Second, serialize the rest of the objects needed by the Monster.
	auto price = Price(123.4f, 456.7f, 789.0f);

	// fill in the rest of the order
	auto broker = fbb.CreateString(market);
	auto stockcode = fbb.CreateString("BTCUSD");

	unsigned char inv_data[] = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
	auto inventory = fbb.CreateVector(inv_data, 10);

	// Shortcut for creating monster with all fields set:
	auto order = CreateOrder(fbb, ++g_id, &price, broker, stockcode, inventory,
		Side_Buy, contracts, Strategies_Contract, cont1.Union());

	fbb.Finish(order);  // Serialize the root of the object.

	// We now have a FlatBuffer we can store on disk or send over a network.

	// ** file/network code goes here :) **
	// access builder.GetBufferPointer() for builder.GetSize() bytes
	return &fbb;
}

int main()
{
	fprintf(stderr, "Server Started at: tcp://localhost:6666\n");

	//  Socket to talk to clients
	void *context = zmq_ctx_new();
	void *responder = zmq_socket(context, ZMQ_REP);
	int rc = zmq_bind(responder, "tcp://*:6666");
	int i = 0;
	char buffer[64] = { 0 };

	while (1)
	{
		memset(buffer, 0, sizeof(buffer));
		int nrec = zmq_recv(responder, buffer, sizeof(buffer), 0);	// ZMQ_DONTWAIT

		if (nrec > 0)
		{
			fprintf(stderr, "Received %d : %s\n", i++, buffer);
			Sleep(100);

			PTR_FBB fbb = BuildFlatBuffer(buffer);
			zmq_send(responder, fbb->GetBufferPointer(), fbb->GetSize(), 0);
		}
	}
	return 0;
}
