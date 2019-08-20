#!/usr/bin/env python3
from os import devnull
from functools import wraps
from itertools import product
from time import perf_counter

class Aux:
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
	# use: @auxilliaries.Aux.timer(file | None)
	#      def function_to_time(*args, **kwargs): pass
	def timer(file):
		def actual_decorator(function):
			@wraps(function)
			def wrapper(*args, **kwargs):
				loc_file = file
				if loc_file is None:
					try:
						loc_file = getattr(args[0], 'log_file')
					except:
						pass
				t_before = perf_counter()
				tmp = function(*args, **kwargs)
				t_after = perf_counter()
				with open(loc_file, 'a+') as f:
					print(f'Finished execution of {function.__name__} in {round(t_after - t_before, 6)} sek.', file=f)
				return tmp
			return wrapper
		return actual_decorator

		
	def get_next_nonadjacent(in_list, index, adjacency=(lambda x, y: x==y-1)):
		# Gets next nonadjacent element from a list
		while True:
			if index == len(in_list) - 1: # if index is last element: return length of list
				return index + 1
			if adjacency(in_list[index], in_list[index+1]): # sift through as long as adjacent.
				index += 1
			else:
				return index + 1

	def round_down(x):
		return (x//1)

	def round_up(x):
		return -(-x//1)

	def prod_dict(**kwargs):
		keys = kwargs.keys()
		vals = kwargs.values()
		for inst in product(*vals):
			yield dict(zip(keys, inst))