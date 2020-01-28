#!/usr/bin/env python3
from os import devnull
from functools import wraps
from collections.abc import Iterable
from itertools import product
from time import perf_counter


# and one more class to keep information about one issued command:
class Command:
    """Information for one Command issued. __init__ separates the command *nix-style."""
    def __init__(self, text):
        self.full_text = text
        comm_separated = Aux._collect_arguments(self.full_text)
        self.main = comm_separated['main']
        self.args = comm_separated['args']
        self.option = comm_separated['option']
        self.long_option = comm_separated['long_option']
        if not self.main:
            raise ValueError("Got an empty command.")

    def __getitem__(self, val):
        """Forward slicing to args, which is uniquely the only list in Command."""
        return self.args.__getitem__(val)


class Aux:
    # Evaluate Function at two boundries
    def bounds(f, a, b):
        if a >= b:
            return 0
        return (f(b) - f(a))

    def _collect_arguments(text):
        """gets a dict of options, args etc for the command (Unix-style)

        INPUTS
        string text: text to parse

        OUTPUTS
        dict{'main': strin main_command, 'args': list(main_args),
             'option': dict{string name: string args},
             'long_option':{string name: string args}

        RAISES
        reraises all Errors

        EXAMPLE
        exmpl first second -third fourth --fifth sixth seventh
        will return {'main': 'exmpl', 'args: ['first', 'second'],
                     'option': {'third': 'fourth'},
                     'long_option': {'fifth': 'sixth seventh'}}"""
        command = {'main': '', 'args': [], 'option': {}, 'long_option': {}}
        dashed = text.split(' -')
        subcommand = dashed[0]
        # main command and args
        args = subcommand.split()
        command['main'] = args[0]
        if len(args) > 1:
            command['args'] = args[1:]
        else:
            command['args'] = []
        for subcommand in dashed:
            # option or long option
            if subcommand[0] == ' ':
                raise ValueError("Option has no name.")
            elif subcommand[0] == '-':  # long_option
                if subcommand[1] == ' ':
                    raise ValueError("Long option has no name.")
                long_option = subcommand[1:].split(' ', 1)
                long_option_name = long_option[0]
                if len(long_option) == 1:
                    command['long_option'][long_option_name] = ''
                else:
                    command['long_option'][long_option_name] = long_option[1]
            else:  # option
                option = subcommand.split(' ', 1)
                option_name = option[0]
                if len(option) == 1:
                    command['option'][option_name] = ''
                else:
                    command['option'][option_name] = option[1]
        return command

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

    def get_next_nonadjacent(in_list, index, adjacency=(lambda x, y: x == y - 1)):
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
            if index == len(in_list) - 1:  # if index is last element: return length of list
                return index + 1
            if adjacency(in_list[index], in_list[index+1]):  # sift through as long as adjacent.
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

    def prod_dict(in_dict):
        """Produce a multidimensional cartesian product over a dict.

        INPUTS
            dict{hashable_type: object or iterator(object)}: The arguments to loop over

        RETURNS
            generator(dict product over all dimensions)
                mathematically a multiple cartesian product

        EXAMPLE
            {'d1': 1, 'd2': (3, 4)} returns
            gen({'d1': 1, 'd2': 3}, {'d1': 1, 'd2': 4}"""
        # names of the dimensions
        keys = in_dict.keys()
        # value set for the dimensions
        vals = in_dict.values()
        # for every product
        for inst in product(*(el if isinstance(el, Iterable) else (el,)
                            for el in vals)):
            # yield a dict that names the dimensions
            yield dict(zip(keys, inst))
