import sys
import traceback

def my_excepthook(type, value, tb):
    print type.__name__
    print value
    print "".join(traceback.format_exception(type, value, tb))

sys.excepthook = my_excepthook # see http://docs.python.org/library/sys.html#sys.excepthook

# some code to generate a naturalistic exception
a = "text"
b = 5
error = a + b