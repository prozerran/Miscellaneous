
#include "pch.h"

#include <fstream>
#include <cassert>

// include headers that implement a archive in simple text format
#include <boost/archive/text_oarchive.hpp>
#include <boost/archive/text_iarchive.hpp>

// https://www.boost.org/doc/libs/1_68_0/libs/serialization/doc/index.html

class gps_position
{
public:
	int degrees;
	int minutes;
	float seconds;
	gps_position() {};
	gps_position(int d, int m, float s) :
		degrees(d), minutes(m), seconds(s)
	{}
};

namespace boost {
	namespace serialization {

		template<class Archive>
		void serialize(Archive & ar, gps_position & g, const unsigned int version)
		{
			ar & g.degrees;
			ar & g.minutes;
			ar & g.seconds;
		}

	} // namespace serialization
} // namespace boost

void Test_Boost()
{
	// create and open a character archive for output
	std::ofstream ofs("boost.txt");

	// create class instance
	const gps_position g(35, 59, 24.567f);

	// save data to archive
	{
		boost::archive::text_oarchive oa(ofs);
		// write class instance to archive
		oa << g;
		// archive and stream closed when destructors are called
	}

	// ... some time later restore the class instance to its orginal state
	gps_position newg;
	{
		// create and open an archive for input
		std::ifstream ifs("boost.txt");
		boost::archive::text_iarchive ia(ifs);
		// read class state from archive
		ia >> newg;
		// archive and stream closed when destructors are called
	}

	// check
	assert(newg.degrees == g.degrees);
	assert(newg.minutes == g.minutes);
	assert(newg.seconds == g.seconds);
}