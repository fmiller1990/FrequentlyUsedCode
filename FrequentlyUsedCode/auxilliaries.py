#!/usr/bin/env python3
import os
import functools
from time import perf_counter

# Use as os-independant /dev/null (os.devnull).
# Faster then os.devnull because os has overhead to go to os-namespace
class FakeSink():
	def write(self, *args):
		pass
	def writelines(self, *args):
		pass
	def close(self, *args):
		pass
	def fileno():
		return os.open(os.devnull, os.O_RDWR)

class Aux(object):
	# Evaluate Function at two boundries
	def bounds(f, a, b):
		if a >= b:
			return 0
		return (f(b) - f(a))
		
	# for use of assignement in lambdas
	def fill_slot(list, element, value): 
		list[element] = value
		
	# takes function or function that returns iterator and makes it generator
	def make_iterator(f): 
		if hasattr(f(), '__next__'):
			return f()
		else:
			return (f() for i in iter(int, 1))
			
	# times CPU-time for function
	def timer(file: str):
		def wrap_file(function):
			@functools.wraps(function)
			def wrap_timer(*args, **kwargs):
				t_before = perf_counter()
				tmp = function(*args, **kwargs)
				t_after = perf_counter()
				with open(file, 'a+') as f:
					print(f'Finished execution of {function.__name__} in {round(t_after - t_before, 6)} sek.', file=f)
				return tmp
			return wrap_timer
		return wrap_file