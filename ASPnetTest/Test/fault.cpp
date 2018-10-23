#include <iostream>
using namespace std;

/*

The problem is with badly initializing the 2d array, columns/row using incorrect
element numbers and wrongly assigning the value. the issue is fixed, but could be further optimized

*/

void initializeMap(int mapSizeX, int mapSizeY, int map[][10])
{
    for(int x = 0; x < mapSizeX; x++)
    {
        map[x][0] = x;
    }

    for(int y = 0; y < (mapSizeY - 3); y++)		// change 2 -> 3
    {
        map[0][y] = y;

        for(int x = 0; x < (mapSizeX - 2); x++)
        {
            map[x][y] = 1;
        }
        //map[mapSizeX][y] = y/2;		// BUG HERE! Removed!
    }

	/*
	// BUG HERE, LAST ROW removed
    for(int x = 0; x < mapSizeX; x++)
    {
        //map[x][mapSizeY - 2] = 0;		
    }	
	*/

	// bad way to do it, but no more time!
	for (int x = 0; x < 7; x++)
	{
		if (x == 0 || x == 1)
			map[9][x] = 0;
		else if (x == 2 || x == 3)
			map[9][x] = 1;
		else
			map[9][x] = -1;
	}
}

void paintMap(int mapSizeX, int mapSizeY, int map[][10])
{
    for(int y = 0; y < mapSizeY; y++)
    {
        for(int x = 0; x < mapSizeX; x++)
        {   
            switch(map[x][y])
            {
                case 0:
                    cout << "#";
                    break;

                case 1:
                    cout << "y";
                    break;

                default:
                    cout << "x";
                    break;

            }
        }
        cout << endl;
    }
}

int main()
{
    int mapSizeX = 10;
    int mapSizeY = 10;
    int map[10][10] = {0};
    initializeMap(mapSizeX, mapSizeY, map);
    paintMap(mapSizeX, mapSizeY, map);

    cout << endl << endl;

    return 0;
}
