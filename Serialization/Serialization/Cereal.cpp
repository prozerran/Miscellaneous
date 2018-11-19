
#include "pch.h"

#include <cereal/types/unordered_map.hpp>
#include <cereal/types/memory.hpp>
#include <cereal/archives/binary.hpp>
#include <cereal/archives/xml.hpp>
#include <cereal/archives/json.hpp>
#include <fstream>

// https://uscilab.github.io/cereal/serialization_archives.html

struct MyData
{
	int age;
	std::string name;

	template <class Archive>
	void serialize(Archive & ar)
	{
		ar(age, name);
	}
};

struct MyRecord
{
	uint8_t x, y;
	float z;

	template <class Archive>
	void serialize(Archive & ar)
	{
		ar(x, y, z);
	}
};

struct SomeData
{
	int32_t id;
	std::shared_ptr<std::unordered_map<uint32_t, MyRecord>> data;

	template <class Archive>
	void save(Archive & ar) const
	{
		ar(data);
	}

	template <class Archive>
	void load(Archive & ar)
	{
		static int32_t idGen = 0;
		id = idGen++;
		ar(data);
	}
};

void Test_Cereal()
{
	{	// binary serialization

		std::ofstream os("cereal.bin", std::ios::binary);
		cereal::BinaryOutputArchive archive(os);

		SomeData myData;
		archive(myData);
	}

	{	// xml serialization
		std::ofstream os("cereal.xml");
		cereal::XMLOutputArchive archive(os);		// depending on the archive type, data may be
													// output to the stream as it is serialized, or
													// only on destruction
		MyData md;
		md.name = "Tim";
		md.age = 40;

		archive(md);
	} // when archive goes out of scope it is guaranteed to have flushed its
	  // contents to its stream

	{	// json serialization
		std::ofstream os("cereal.json");
		cereal::JSONOutputArchive archive(os);		// depending on the archive type, data may be
													// output to the stream as it is serialized, or
													// only on destruction
		MyRecord mr;
		mr.x = 7;
		mr.y = 8;
		mr.z = 9.3;

		archive(mr);
	} // when archive goes out of scope it is guaranteed to have flushed its
	  // contents to its stream
}