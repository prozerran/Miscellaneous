// Serialization.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>

// flat buffers
extern void Test_FlatBuffers();

// protocol buffers
extern int Write_ProtocolBuffers(int, char*[]);
extern int Read_ProtocolBuffers(int, char*[]);

// cereal
extern void Test_Cereal();
extern void Test_RapidJSON();

// boost
extern void Test_Boost();

int main(int argc, char* argv[])
{
	std::cout << "Testing Serialiation Libraries\n";

	if (argc > 1)
	{
		// test protocol buffers, with filename
		// Serialization.exe filename.bin

		std::cout << "Protocol Buffers: WRITE\n";
		Write_ProtocolBuffers(argc, argv);

		std::cout << "Protocol Buffers: READ\n";
		Read_ProtocolBuffers(argc, argv);
	}
	else
	{
		std::cout << "Flat Buffers:\n";
		Test_FlatBuffers();

		std::cout << "Cereal Serialization:\n";
		Test_Cereal();

		std::cout << "Boost Serialization:\n";
		Test_Boost();

		std::cout << "RapidJSON Serialization:\n";
		Test_RapidJSON();
	}
	return 0;
}


// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
