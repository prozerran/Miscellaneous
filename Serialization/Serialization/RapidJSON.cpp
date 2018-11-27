
#include "pch.h"

#include <iostream>
#include <cassert>

// include headers that implement a archive in simple text format
#include "rapidjson/document.h"
#include "rapidjson/writer.h"
#include "rapidjson/stringbuffer.h"

// https://github.com/Tencent/rapidjson

using namespace rapidjson;

void Test_RapidJSON()
{
	// 1. Parse a JSON string into DOM.
	const char* json = "{\"project\":\"rapidjson\",\"stars\":10}";
	Document d;
	d.Parse(json);

	// 2. Modify it by DOM.
	Value& s = d["stars"];

	// Verify, current DOM value
	assert(s.GetInt() == 10);

	// Modify, increment value
	s.SetInt(s.GetInt() + 1);

	// 3. Stringify the DOM
	StringBuffer buffer;
	Writer<StringBuffer> writer(buffer);
	d.Accept(writer);

	// Output {"project":"rapidjson","stars":11}
	//std::cout << buffer.GetString() << std::endl;

	// Verify, current DOM value
	assert(s.GetInt() == 11);
}