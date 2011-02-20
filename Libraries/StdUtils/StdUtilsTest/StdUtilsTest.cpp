// StdUtilsTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


void TestTrees()
{
   tree<string> tr;
   tree<string>::iterator top, one, two, loc, banana, three;

	top=tr.begin();
	one=tr.insert(top, "one");
	two=tr.append_child(one, "two");
	tr.append_child(two, "apple");
	banana=tr.append_child(two, "banana");
	tr.append_child(banana,"cherry");
	tr.append_child(two, "peach");
	three = tr.append_child(one,"three");
	
	kptree::print_tree_bracketed(tr);

	tr.move_below(banana, three);

	kptree::print_tree_bracketed(tr);
}

int _tmain(int argc, _TCHAR* argv[])
{
	TestTrees();

	return 0;
}

