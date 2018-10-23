// ConsoleCpp.cpp : Defines the entry point for the console application.
//



int _tmain(int argc, _TCHAR* argv[])
{

	int num[] = {1, 3, 0, 2, 2, 4};


	int count = 6;
		int p = 0;
		for (int i = 1; i < count-1; i++)
		{
			if (num[i] > num[i-1])
				p=p+1;
		}


	printf("Hello World %d", p);
	return 0;
}

