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
		"""Make Iterator out of function or generator.
		
		INPUTS
			function or generator f of signature f()->object: object to make iterator out of
		RETURNS
			iterator (returning f() or next(f()) when next(...) is called)"""
		if hasattr(f(), '__next__'):
			return f()
		else:
			return (f() for i in iter(int, 1))


	def timer(file):
		"""Time execution of function with info to ``file``.
		
		INPUTS
			(object that supports open(...)) file: file to write info to
		RETURNS
			decorator (decorator to use)
		USE MODES
			1:
			@auxilliaries.Aux.timer(file)
			def function_to_time(*args, **kwargs): pass
			
			2: (for classmethod with Class().log_file being well defined.
				will use Class().log_file for output if exists, raise ValueError otherwise.)
			@auxilliaries.Aux.timer(None)
			def bound_method_to_time(self, *args, **kwargs): pass"""
		def actual_decorator(function):
			@wraps(function)
			def wrapper(*args, **kwargs):
				loc_file = file
				if loc_file is None:
					try:
						loc_file = getattr(args[0], 'log_file')
					except:
						raise ValueError("Class does not implement log_file. Unable to find outputpath.")
				t_before = perf_counter()
				tmp = function(*args, **kwargs)
				t_after = perf_counter()
				with open(loc_file, 'a+') as f:
					print(f'Finished execution of {function.__name__} in {round(t_after - t_before, 6)} sek.', file=f)
				return tmp
			return wrapper
		return actual_decorator

		
	def get_next_nonadjacent(in_list, index, adjacency=(lambda x, y: x==y-1)):
		"""Get next nonadjacent element from a list.
		
		INPUTS
			list(...) ``in_list``: list to use
			int ``index``: the index to start from
			bool func(list_el, list_el) = (lambda x, y: x==y-1): adjacency rule to use
		RETURNS
			int (next nonadjacent index)
		RAISES
			IndexError if index >= len(in_list)
			reraises all Errors"""
		if index >= len(in_list):
			raise IndexError(f"List index out of range: {index} in {in_list}")
		while True:
			if index == len(in_list) - 1: # if index is last element: return length of list
				return index + 1
			if adjacency(in_list[index], in_list[index+1]): # sift through as long as adjacent.
				index += 1
			else:
				return index + 1

	def round_down(x):
		"""Round down ``x`` to the next integer smaller then x.
		
		INPUTS
			``x``: typically float to round down
		
		RETURNS
			int (``x`` rounded down)"""
		return (x//1)

	def round_up(x):
		"""Round up ``x`` to the next integer bigger then ``x``.

		INPUTS
			``x``: typically float to round up

		RETURNS
			int (``x``rounded up)"""
		return -(-x//1)

	def prod_dict(**kwargs):
		keys = kwargs.keys()
		vals = kwargs.values()
		for inst in product(*vals):
			yield dict(zip(keys, inst))