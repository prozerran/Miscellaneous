

#include <vector>

using namespace std;

/*
Big root tree problem.
- given a root int as source
- look through each leave determine if node >= root number
- return total number of leaves >= root
- numbers can be in any order, any node

Sample:
				5
			20		10
		2		3 6		4
	7		8 1       40  3

	All numbers >= 5 is : 5, 20, 7, 8, 10, 6, 40
	Return 7
*/

struct Node {
	int num;
	Node* l;
	Node* r;
};

int BigRootTree_Solutuion(Node* n)
{



	return 0;	// TODO...
}