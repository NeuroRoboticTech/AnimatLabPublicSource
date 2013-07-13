#include "stdafx.h"
#include <boost/test/unit_test.hpp> 

BOOST_AUTO_TEST_SUITE( Tree_Suite )

BOOST_AUTO_TEST_CASE( test_case1 )
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
	
	//kptree::print_tree_bracketed(tr);

	tr.move_below(banana, three);

	//kptree::print_tree_bracketed(tr);
}

BOOST_AUTO_TEST_SUITE_END()